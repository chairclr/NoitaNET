using System;
using System.Collections.Generic;
using System.Linq;
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
        IncrementalValuesProvider<AdditionalText> apiDocs = context.AdditionalTextsProvider.Where(file => file.Path.EndsWith("lua_api_documentation.txt"));

        // Skip(2) is to skip the first two lines of the API docs
        IncrementalValuesProvider<string> content = apiDocs.Select((x, c) => x.GetText(c)!.ToString().Replace("\r", "").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(2)).SelectMany((x, _) => x);

        context.RegisterSourceOutput(content, (spc, content) =>
        {
            StringBuilder builder = new StringBuilder();

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
            string parameters = match.Groups[2].Value.Trim();
            string returnValues = match.Groups[3].Value.Trim();
            string description = match.Groups[4].Value.Trim();

            builder.AppendLine($"""
                                /// <summary>
                                /// {(string.IsNullOrEmpty(description) ? "No description provided" : description)}
                                /// </summary>
                                """);

            builder.Append($"   public void {functionName}(");

            builder.AppendLine(")");
            builder.AppendLine("    {");

            // TODO: Generate code to call into Lua API here

            builder.AppendLine("    }");

            builder.AppendLine("}");

            Console.WriteLine(builder.ToString());

            spc.AddSource($"Noita.EngineAPI.{functionName}.g.cs", builder.ToString());
        });
    }

    private static List<LuaDocParameter> GetParameters(string parameters)
    {
        if (string.IsNullOrEmpty(parameters))
        {
            return new List<LuaDocParameter>();
        }

        List<LuaDocParameter> parsedParams = new List<LuaDocParameter>();

        string[] parameterList = parameters.Split(',');

        foreach (string parameter in parameterList)
        {
            string[] parts = parameter.Split(':');
            string name = parts[0].Trim();
            string type = parts[1].Trim();

            parsedParams.Add(new LuaDocParameter(name, type, GetCSharpType(type)));
        }

        return parsedParams;
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
            default:
                return "object";
        }
    }

    private record struct LuaDocParameter(string Name, string LuaType, string CSharpType);
}
