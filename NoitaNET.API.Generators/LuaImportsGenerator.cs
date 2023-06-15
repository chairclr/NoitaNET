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

            for (int i = 0; i < methodSymbol.Parameters.Length; i++)
            {
                IParameterSymbol parameter = methodSymbol.Parameters[i];

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

                argumentBuilder.Append(parameter.Name);

                if (i + 1 < methodSymbol.Parameters.Length)
                {
                    argumentBuilder.Append(", ");
                }

                signatureBuilder.Append($"{parameter.Type.ToDisplayString()}, ");
            }

            signatureBuilder.Append($"{methodSymbol.ReturnType.ToDisplayString()}");

            fnSig.FunctionParameters = parameterBuilder.ToString();

            fnSig.FunctionCallArguments = argumentBuilder.ToString();

            fnSig.FunctionPointerSignature = signatureBuilder.ToString();

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
                    source.AppendLine($"__Raw_{function.FunctionName}({function.FunctionCallArguments});");
                }
                else
                {
                    source.AppendLine($"return __Raw_{function.FunctionName}({function.FunctionCallArguments});");
                }
                source.AppendLine("}");
            }

            source.AppendLine("}");

            spc.AddSource("RawLuaImports.g.cs", source.ToString());
        });
    }

    private record struct LuaFunctionNameSignatureTuple(string FunctionName, string FunctionReturnType, string FunctionParameters, string FunctionCallArguments, string FunctionPointerSignature);
}