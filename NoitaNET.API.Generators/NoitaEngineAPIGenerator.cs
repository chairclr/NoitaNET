using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace NoitaNET.API.Generators;

[Generator(LanguageNames.CSharp)]
public class NoitaEngineAPIGenerator : IIncrementalGenerator
{
    private static readonly Regex APIFunctionRegex = new Regex(@"^([^(\n]+)\(([^)]*)\)(?: -> ([^\[]+))?(?: \[([^]]*)\])?$");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<ImmutableArray<AdditionalText>> apiDocs = context.AdditionalTextsProvider.Where(file => file.Path.EndsWith("lua_api_documentation.txt")).Collect();

        IncrementalValueProvider<INamedTypeSymbol> realTypeSymbol = context.CompilationProvider.Select((x, c) => x.GetTypeByMetadataName("NoitaNET.API.Noita")!);

        // Skip(2) is to skip the first two lines of the API docs
        IncrementalValueProvider<string[]> contentArray = apiDocs.Select((x, c) => x[0].GetText(c)!.ToString().Replace("\r", "").Replace("  ", " ").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray());

        IncrementalValueProvider<(INamedTypeSymbol noitaSymbol, string[] contents)> union = realTypeSymbol.Combine(contentArray);

        IncrementalValuesProvider<InitUnionType> manyUnion = union.SelectMany((x, _) => x.contents.Select<string, InitUnionType>(y => new InitUnionType(x.noitaSymbol, y)));

        context.RegisterSourceOutput(manyUnion, (spc, unionInfo) =>
        {
            StringBuilder builder = new StringBuilder();

            string content = unionInfo.Content;

            if (content.Contains("[") && !content.EndsWith("]"))
            {
                content += "]";
            }

            builder.AppendLine("/// Auto-generated ///");

            builder.AppendLine("namespace NoitaNET.API;");
            builder.AppendLine("unsafe partial class Noita");
            builder.AppendLine("{");

            Match match = APIFunctionRegex.Match(content);
            if (!match.Success)
            {
                return; // Invalid function documentation format
            }

            string functionName = match.Groups[1].Value.Trim();

            // Method has already been defined in Noita class by a human
            if (unionInfo.NoitaSymbol.GetMembers().Any(x => x.Kind == SymbolKind.Method && x.Name == functionName))
            {
                return; 
            }

            string rawParameters = match.Groups[2].Value.Trim();
            string rawReturnValues = match.Groups[3].Value.Trim();
            string description = match.Groups[4].Value.Trim();

            (List<LuaDocParameter>? parameters, List<LuaDocParameter>? overloadParameters) = GetParameters(rawParameters);
            List<LuaDocReturnValue>? returnValues = GetReturnValues(rawReturnValues);

            builder.AppendLine($"""
                                /// <summary>
                                /// {(string.IsNullOrEmpty(description) ? "No description provided" : description)}
                                /// </summary>
                                """);

            WriteMethod(builder, functionName, parameters, overloadParameters, returnValues);

            builder.AppendLine("}");

            Console.WriteLine(builder.ToString());

            spc.AddSource($"Noita.EngineAPI.{functionName}.g.cs", builder.ToString());
        });
    }

    private static (List<LuaDocParameter>?, List<LuaDocParameter>?) GetParameters(string parameters)
    {
        if (string.IsNullOrEmpty(parameters))
        {
            return (null, null);
        }

        List<LuaDocParameter> parsedParams = new List<LuaDocParameter>();

        List<LuaDocParameter> parsedOverloadParams = new List<LuaDocParameter>();

        string[] parameterList = parameters.Split(',');

        bool hadOverloads = false;

        foreach (string parameter in parameterList)
        {
            string[] parts = parameter.Split(':');
            string name = parts[0].Trim();

            string typeAndOptionalDefault;

            if (parts.Length == 1)
            {
                if (name == "component_type_name")
                {
                    typeAndOptionalDefault = "string";
                }
                else if (name == "bool rare_table = false")
                {
                    name = "rare_table";
                    typeAndOptionalDefault = "rare_table = false";
                }
                else if (name == "{table_of_xml_entities}")
                {
                    name = "table_of_xml_entities";
                    typeAndOptionalDefault = "{string}";
                }
                else
                {
                    throw new Exception("No parameter type");
                }
            }
            else
            {
                typeAndOptionalDefault = parts[1].Trim();
            }

            string[] defaultSplits = typeAndOptionalDefault.Split('=');

            if (defaultSplits.Length == 1)
            {
                parsedParams.Add(new LuaDocParameter(name, typeAndOptionalDefault, GetCSharpType(typeAndOptionalDefault)));
                parsedOverloadParams.Add(new LuaDocParameter(name, typeAndOptionalDefault, GetCSharpType(typeAndOptionalDefault)));
            }
            else if (defaultSplits.Length == 2)
            {
                hadOverloads = true;

                string reparsedType = defaultSplits[0].Trim();
                parsedOverloadParams.Add(new LuaDocParameter(name, reparsedType, GetCSharpType(reparsedType)));
            }
        }

        return (parsedParams, hadOverloads ? parsedOverloadParams : null);
    }

    private static List<LuaDocReturnValue>? GetReturnValues(string returnValues)
    {
        if (string.IsNullOrEmpty(returnValues))
        {
            return null;
        }

        return null;
    }

    private static string GetCSharpType(string luaType)
    {
        switch (luaType)
        {
            case "string":
                return "string";
            case "int":
                return "int";
            case "number":
                return "double";
            case "bool":
                return "bool";
            case "{int}":
                return "int[]";
            case "{string}":
                return "string[]";
            case "{string-string}":
                return "System.Collections.Generic.Dictionary<string, string>";
            default:
                throw new Exception($"Unknown lua type {luaType}");
        }
    }

    private static void WriteMethod(StringBuilder builder, string name, List<LuaDocParameter>? parameters, List<LuaDocParameter>? overloadParameters, List<LuaDocReturnValue>? returnValues)
    {
        void Write()
        {
            builder.Append($"public void {name}(");

            bool wroteAnyParameters = false;

            if (parameters is not null)
            {
                foreach (LuaDocParameter parameter in parameters)
                {
                    wroteAnyParameters = true;
                    builder.Append($"{parameter.CSharpType} {parameter.Name}, ");
                }
            }

            if (returnValues is not null)
            {
                foreach (LuaDocReturnValue returnValue in returnValues)
                {
                    wroteAnyParameters = true;
                    builder.Append($"out {returnValue.CSharpType} {returnValue.Name}, ");
                }
            }

            // Remove trailing ", "
            if (wroteAnyParameters)
            {
                builder.Length -= 2;
            }

            builder.AppendLine($")");

            builder.AppendLine("{");
            builder.AppendLine("}");
        }

        Write();

        if (overloadParameters is not null)
        {
            parameters ??= new List<LuaDocParameter>();
            parameters.AddRange(overloadParameters);

            Write();
        }
    }

    private record struct InitUnionType(INamedTypeSymbol NoitaSymbol, string Content);

    private record struct LuaDocParameter(string Name, string LuaType, string CSharpType);

    private record struct LuaDocReturnValue(string Name, string LuaType, string CSharpType);
}
