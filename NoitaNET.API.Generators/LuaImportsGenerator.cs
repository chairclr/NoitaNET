using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace NoitaNET.API.Generators;

[Generator(LanguageNames.CSharp)]
public class LuaImportsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        const string LuaNativeImportAttributeName = "NoitaNET.API.Lua.LuaNativeImport";

        bool Filter(SyntaxNode node, CancellationToken cancellationToken)
        {
            return true;
        };

        LuaFunctionNameSignatureTuple Transform(GeneratorAttributeSyntaxContext attributeSyntaxContext, CancellationToken cancellationToken)
        {
            LuaFunctionNameSignatureTuple fnSig = new LuaFunctionNameSignatureTuple();

            IMethodSymbol methodSymbol = ((IMethodSymbol)attributeSyntaxContext.TargetSymbol);

            fnSig.FunctionName = methodSymbol.Name;

            fnSig.FunctionReturnType = methodSymbol.ReturnType.ToDisplayString();

            StringBuilder parameterBuilder = new StringBuilder();
            StringBuilder argumentBuilder = new StringBuilder();
            StringBuilder signatureBuilder = new StringBuilder();

            List<string> stringMarshalingToDo = new List<string>();

            for (int i = 0; i < methodSymbol.Parameters.Length; i++)
            {
                IParameterSymbol parameter = methodSymbol.Parameters[i];

                if (parameter.Type.SpecialType == SpecialType.System_String)
                {
                    stringMarshalingToDo.Add(parameter.Name);
                }

                if (parameter.RefKind == RefKind.Ref)
                {
                    parameterBuilder.Append($"ref ");
                }
                else if (parameter.RefKind == RefKind.Out)
                {
                    parameterBuilder.Append($"out ");
                }

                parameterBuilder.Append($"{parameter.Type} ");

                parameterBuilder.Append(parameter.Name);

                if (parameter.HasExplicitDefaultValue)
                {
                    parameterBuilder.Append(" = ");
                    if (parameter.ExplicitDefaultValue is null)
                    {
                        parameterBuilder.Append("null");
                    }
                    else
                    {
                        if (parameter.ExplicitDefaultValue is bool booleanValue)
                        {
                            parameterBuilder.Append(booleanValue ? "true" : "false");
                        }
                        else
                        {
                            parameterBuilder.Append(parameter.ExplicitDefaultValue!.ToString());
                        }
                    }
                }

                if (i + 1 < methodSymbol.Parameters.Length)
                    parameterBuilder.Append(", ");

                if (parameter.Type.SpecialType == SpecialType.System_String)
                {
                    argumentBuilder.Append($"__marshaledString{parameter.Name}");
                }
                else
                {
                    argumentBuilder.Append(parameter.Name);
                }

                if (i + 1 < methodSymbol.Parameters.Length)
                {
                    argumentBuilder.Append(", ");
                }

                if (parameter.Type.SpecialType == SpecialType.System_String)
                {
                    signatureBuilder.Append($"nint, ");
                }
                else
                {
                    signatureBuilder.Append($"{parameter.Type.ToDisplayString()}, ");
                }
            }

            if (methodSymbol.ReturnType.SpecialType == SpecialType.System_String)
            {
                signatureBuilder.Append($"nint");
            }
            else
            {
                signatureBuilder.Append($"{methodSymbol.ReturnType.ToDisplayString()}");
            }

            fnSig.FunctionParameters = parameterBuilder.ToString();

            fnSig.FunctionCallArguments = argumentBuilder.ToString();

            fnSig.FunctionPointerSignature = signatureBuilder.ToString();

            if (stringMarshalingToDo.Count > 0)
            {
                StringBuilder marshalParameterBuilder = new StringBuilder();
                StringBuilder marshalParameterFreeBuilder = new StringBuilder();

                for (int i = 0; i < stringMarshalingToDo.Count; i++)
                {
                    marshalParameterBuilder.AppendLine($"nint __marshaledString{stringMarshalingToDo[i]} = 0;");
                    marshalParameterBuilder.AppendLine($"byte[]? __sharedBuffer{i} = null;");
                    marshalParameterBuilder.AppendLine($"if ({stringMarshalingToDo[i]} is not null)");
                    marshalParameterBuilder.AppendLine("{");
                    marshalParameterBuilder.AppendLine($"__sharedBuffer{i} = System.Buffers.ArrayPool<byte>.Shared.Rent({stringMarshalingToDo[i]}.Length + 1);");
                    marshalParameterBuilder.AppendLine($"System.Span<byte> __byteSpan{i} = new System.Span<byte>(__sharedBuffer{i}, 0, {stringMarshalingToDo[i]}.Length + 1);");
                    marshalParameterBuilder.AppendLine($"__byteSpan{i}[^1] = 0;");
                    marshalParameterBuilder.AppendLine($"System.Text.Encoding.UTF8.GetBytes(System.MemoryExtensions.AsSpan({stringMarshalingToDo[i]}), __byteSpan{i});");
                    marshalParameterBuilder.AppendLine($"__marshaledString{stringMarshalingToDo[i]} = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref __byteSpan{i}[0]);");
                    marshalParameterBuilder.AppendLine("}");

                    marshalParameterFreeBuilder.AppendLine($"if ({stringMarshalingToDo[i]} is not null)");
                    marshalParameterFreeBuilder.AppendLine("{");
                    marshalParameterFreeBuilder.AppendLine($"System.Buffers.ArrayPool<byte>.Shared.Return(__sharedBuffer{i}!);");
                    marshalParameterFreeBuilder.AppendLine("}");
                }

                fnSig.MarshalString = marshalParameterBuilder.ToString();
                fnSig.MarshalStringFree = marshalParameterFreeBuilder.ToString();
            }

            return fnSig;
        }

        IncrementalValuesProvider<LuaFunctionNameSignatureTuple> values = context.SyntaxProvider.ForAttributeWithMetadataName(LuaNativeImportAttributeName, Filter, Transform);

        context.RegisterSourceOutput(values.Collect(), (spc, functions) =>
        {
            StringBuilder source = new StringBuilder();

            source.AppendLine("/// Auto-generated ///");
            source.AppendLine("#nullable enable");
            source.AppendLine();

            source.AppendLine("namespace NoitaNET.API.Lua;");
            source.AppendLine("public unsafe partial class LuaNative");
            source.AppendLine("{");

            foreach (LuaFunctionNameSignatureTuple function in functions)
            {
                // public static delegate* unmanaged[Cdecl, SuppressGCTransition]<lua_State*, int, double> Raw_lua_tonumber = (delegate* unmanaged[Cdecl, SuppressGCTransition]<lua_State*, int, double>)GetLuaExport("lua_tonumber");

                string functionPointerType = $"delegate* unmanaged[Cdecl, SuppressGCTransition]<{function.FunctionPointerSignature}>";
                source.AppendLine($"private static {functionPointerType} __Raw_{function.FunctionName} = ({functionPointerType})GetLuaExport(\"{function.FunctionName}\");");

                source.AppendLine($"public static partial {function.FunctionReturnType} {function.FunctionName}({function.FunctionParameters})");
                source.AppendLine("{");
                if (function.FunctionReturnType == "void")
                {
                    if (function.MarshalString is not null)
                    {
                        source.AppendLine(function.MarshalString);
                    }

                    source.AppendLine($"__Raw_{function.FunctionName}({function.FunctionCallArguments});");

                    if (function.MarshalStringFree is not null)
                    {
                        source.AppendLine(function.MarshalStringFree);
                    }
                }
                else
                {
                    if (function.MarshalString is not null)
                    {
                        source.AppendLine(function.MarshalString);
                    }

                    if (function.FunctionReturnType.StartsWith("string"))
                    {
                        source.AppendLine($"{function.FunctionReturnType} __retvalue = string.Empty;");
                        source.AppendLine($"nint __rawRetvalue = __Raw_{function.FunctionName}({function.FunctionCallArguments});");
                        source.AppendLine($"if (__rawRetvalue == 0) {{ __retvalue = null; }} else {{ __retvalue = System.Text.Encoding.UTF8.GetString(System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)__rawRetvalue)); }} ");
                    }
                    else
                    {
                        source.AppendLine($"{function.FunctionReturnType} __retvalue = __Raw_{function.FunctionName}({function.FunctionCallArguments});");
                    }

                    if (function.MarshalStringFree is not null)
                    {
                        source.AppendLine(function.MarshalStringFree);
                    }

                    source.AppendLine("return __retvalue;");
                }
                source.AppendLine("}");
            }

            source.AppendLine("}");

            spc.AddSource("RawLuaImports.g.cs", source.ToString());
        });
    }

    private record struct LuaFunctionNameSignatureTuple(string FunctionName, string FunctionReturnType, string FunctionParameters, string FunctionCallArguments, string FunctionPointerSignature, string? MarshalString, string? MarshalStringFree);
}
