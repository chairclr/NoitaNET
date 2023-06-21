using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        IncrementalValueProvider<INamedTypeSymbol> realTypeSymbol = context.CompilationProvider.Select((x, c) => x.GetTypeByMetadataName("NoitaNET.API.Noita.EngineAPI")!);

        // Skip(2) is to skip the first two lines of the API docs
        IncrementalValueProvider<string[]> contentArray = apiDocs.Select((x, c) => x[0].GetText(c)!.ToString().Replace("\r", "").Replace("  ", " ").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray());

        IncrementalValueProvider<(INamedTypeSymbol noitaSymbol, string[] contents)> union = realTypeSymbol.Combine(contentArray);

        IncrementalValuesProvider<InitUnionType> manyUnion = union.SelectMany((x, _) => x.contents.Select<string, InitUnionType>(y => new InitUnionType(x.noitaSymbol, y)));

        context.RegisterSourceOutput(manyUnion.Collect(), (spc, collectedUnionInfo) =>
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("using NoitaNET.API;");
            builder.AppendLine("using NoitaNET.API.Lua;");
            builder.AppendLine("/// Auto-generated ///");
            builder.AppendLine("namespace NoitaNET.API.Noita;");
            builder.AppendLine("#nullable enable");
            builder.AppendLine("unsafe partial class EngineAPI");
            builder.AppendLine("{");

            List<string> functionPointersToAddToTable = new List<string>(collectedUnionInfo.Length);

            foreach (InitUnionType unionInfo in collectedUnionInfo)
            {
                string content = unionInfo.Content;

                // Case for GameGetDateAndTimeLocal() ->year:int,month:int,day:int,hour:int,minute:int,second:int
                content = content.Replace("->y", "-> y");

                content = content.Replace("-> x, y", "-> x:number, y:number");

                content = content.Replace("-> (x:number,y:number)|nil", "-> x:number,y:number|nil");

                content = content.Replace("(Debugish", "[(Debugish");

                content = content.Replace("multiple types", "multiple_types");

                if (content.Contains("[") && !content.EndsWith("]"))
                {
                    content += "]";
                }

                Match match = APIFunctionRegex.Match(content);
                if (!match.Success)
                {
                    throw new Exception("Invalid function documentation format");
                }

                string functionName = match.Groups[1].Value.Trim();

                functionPointersToAddToTable.Add(functionName);

                // Method has already been defined in Noita class by a human
                if (unionInfo.NoitaSymbol.GetMembers().Any(x => x.Kind == SymbolKind.Method && x.Name == functionName))
                {
                    continue;
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
                builder.AppendLine();
            }

            builder.AppendLine("}");

            spc.AddSource($"Noita.EngineAPI.g.cs", builder.ToString());

            builder.Clear();

            builder.AppendLine("using unsafe APIFunction = delegate* unmanaged[Cdecl, SuppressGCTransition]<NoitaNET.API.Lua.LuaNative.lua_State*, void>;");
            builder.AppendLine("/// Auto-generated ///");
            builder.AppendLine("namespace NoitaNET.API.Noita;");
            builder.AppendLine("#nullable enable");
            builder.AppendLine("unsafe partial class EngineAPIFunctionTable");
            builder.AppendLine("{");
            foreach (string fname in functionPointersToAddToTable)
            {
                builder.AppendLine($"public static APIFunction {fname} = default;");
            }
            builder.AppendLine("}");

            spc.AddSource($"Noita.EngineAPITable.g.cs", builder.ToString());
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
                    typeAndOptionalDefault = "bool = false";
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

        List<LuaDocReturnValue> result = new List<LuaDocReturnValue>();

        string[] retvals = returnValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string retval in retvals.Select(x => x.Trim()))
        {
            // table
            string replacedRetval = retval;
            bool nullable = false;
            if (retval.Contains("|nil"))
            {
                nullable = true;

                replacedRetval = retval.Replace("|nil", "");
            }

            if (replacedRetval.StartsWith("{") && replacedRetval.EndsWith("}"))
            {
                string type = replacedRetval;

                if (replacedRetval.Contains(":"))
                {
                    type = replacedRetval.Remove(1, replacedRetval.IndexOf(":"));
                }

                result.Add(new LuaDocReturnValue("return_value_table", replacedRetval, GetCSharpType(type) + (nullable ? "?" : "")));
            }
            // otherwise
            else
            {
                string[] nameTypeSplits = replacedRetval.Split(':');
                string name = nameTypeSplits[0].Trim();
                string type;

                if (nameTypeSplits.Length == 1)
                {
                    type = name;

                    // For cases like -> name and -> component_id
                    if (type == "name")
                    {
                        type = "string";
                    }
                    if (type == "component_id")
                    {
                        type = "int";
                    }

                    name = $"return_value_{type}";
                }
                else
                {
                    name = $"return_value_{name}";
                    type = nameTypeSplits[1].Trim();
                }

                if (type == "int_body_id")
                {
                    type = "int";
                    name = "return_value_body_id";
                }
                else if (type == "bool_is_new")
                {
                    type = "bool";
                    name = "return_value_is_new";
                }
                else if (type == "new_text")
                {
                    type = "string";
                    name = "return_new_text";
                }

                result.Add(new LuaDocReturnValue(name, type, GetCSharpType(type) + (nullable ? "?" : "")));
            }
        }

        return result;
    }

    private static string GetCSharpType(string luaType)
    {
        if (luaType.Contains("|"))
        {
            return luaType;
        }

        return luaType switch
        {
            "string" => "string",
            "int" => "nint",
            "uint" => "nuint",
            "number" or "float" => "double",
            "bool" => "bool",
            "{item_entity_id}" or "{component_id}" or "{int}" => "nint[]",
            "{string}" => "string[]",
            "{string-string}" => "System.Collections.Generic.Dictionary<string, string>",
            "obj" => "NoitaNET.API.Gui.GuiUserData",
            _ => throw new Exception($"Unknown lua type {luaType}"),
        };
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

#if DEBUG_ALL_API_CALLS
            builder.AppendLine($"NoitaNET.API.Logging.Logger.Instance.LogDebug($\"Calling {name}, EngineAPIFunctionTable.{name}: {{((nint)EngineAPIFunctionTable.{name}):X}}\");");
#endif

            if (parameters is not null)
            {
                foreach (LuaDocParameter parameter in parameters)
                {
                    switch (parameter.CSharpType)
                    {
                        case "nint":
                            builder.AppendLine($"LuaNative.lua_pushinteger(L, {parameter.Name});");
                            break;
                        case "nuint":
                            builder.AppendLine($"unchecked {{ LuaNative.lua_pushinteger(L, (nint){parameter.Name}); }}");
                            break;
                        case "double":
                            builder.AppendLine($"LuaNative.lua_pushnumber(L, {parameter.Name});");
                            break;
                        case "string":
                            builder.AppendLine($"LuaNative.lua_pushstring(L, {parameter.Name});");
                            break;
                        case "bool":
                            builder.AppendLine($"LuaNative.lua_pushboolean(L, {parameter.Name} ? 1 : 0);");
                            break;
                        case "NoitaNET.API.Gui.GuiUserData":
                            builder.AppendLine($"LuaNative.lua_pushlightuserdata(L, {parameter.Name}.InternalPointer);");
                            break;
                    }
                }
            }

            builder.AppendLine($"EngineAPIFunctionTable.{name}(L);");

            if (returnValues is not null)
            {
                int tableCount = 0;


                for (int i = 0; i < returnValues.Count; i++)
                {
                    int stackIndex = -(i) - 1;

                    int returnValueIndex = returnValues.Count - i - 1;

                    LuaDocReturnValue returnValue = returnValues[returnValueIndex];

                    void WriteTable(string type)
                    {
                        string lua_call = "";

                        switch (type)
                        {
                            case "nint":
                                lua_call = $"LuaNative.lua_tointeger(L, {stackIndex})";
                                break;
                            case "double":
                                lua_call = $"LuaNative.lua_todouble(L, {stackIndex})";
                                break;
                            case "string":
                                lua_call = $"LuaNative.lua_tostring(L, {stackIndex})!";
                                break;
                        }

                        builder.AppendLine($"nuint __tableLength{tableCount} = LuaNative.lua_objlen(L, {stackIndex});");
                        builder.AppendLine($"{returnValue.Name} = new {type}[__tableLength{tableCount}];");

                        builder.AppendLine(
                           $$"""
                            for (int i = 0; (nuint)i < __tableLength{{tableCount}}; i++)
                            {
                                LuaNative.lua_rawgeti(L, {{stackIndex}}, i + 1);
                                {{returnValue.Name}}[i] = {{lua_call}};

                                LuaNative.lua_pop(L, 1); 
                            }
                            """);
                    }

                    switch (returnValue.CSharpType)
                    {
                        case "nint":
                            builder.AppendLine($"{returnValue.Name} = LuaNative.lua_tointeger(L, {stackIndex});");
                            break;
                        case "nint?":
                            builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 1) {{ {returnValue.Name} = null; }} else {{ {returnValue.Name} = LuaNative.lua_tointeger(L, {stackIndex}); }}");
                            break;
                        case "nuint":
                            builder.AppendLine($"unchecked {{ {returnValue.Name} = (nuint)LuaNative.lua_tointeger(L, {stackIndex}); }}");
                            break;
                        case "nuint?":
                            builder.AppendLine($"unchecked {{ if (LuaNative.lua_isnil(L, {stackIndex}) == 1) {{ {returnValue.Name} = null; }} else {{ {returnValue.Name} = (nuint)LuaNative.lua_tointeger(L, {stackIndex}); }} }}");
                            break;
                        case "double":
                            builder.AppendLine($"{returnValue.Name} = LuaNative.lua_tonumber(L, {stackIndex});");
                            break;
                        case "double?":
                            builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 1) {{ {returnValue.Name} = null; }} else {{ {returnValue.Name} = LuaNative.lua_tonumber(L, {stackIndex}); }}");
                            break;
                        case "string":
                            builder.AppendLine($"{returnValue.Name} = LuaNative.lua_tostring(L, {stackIndex})!;");
                            break;
                        case "string?":
                            builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 1) {{ {returnValue.Name} = null; }} else {{ {returnValue.Name} = LuaNative.lua_tostring(L, {stackIndex})!; }}");
                            break;
                        case "bool":
                            builder.AppendLine($"{returnValue.Name} = LuaNative.lua_toboolean(L, {stackIndex}) == 1 ? true : false;");
                            break;
                        case "bool?":
                            builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 1) {{ {returnValue.Name} = null; }} else {{ {returnValue.Name} = LuaNative.lua_toboolean(L, {stackIndex}) == 1 ? true : false; }}");
                            break;
                        case "nint[]":
                            {
                                WriteTable("nint");
                            }
                            break;
                        case "nint[]?":
                            {
                                builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 0) {{");
                                WriteTable("nint");
                                builder.AppendLine($"}} else {{ {returnValue.Name} = null; }}");
                            }
                            break;
                        case "double[]":
                            {
                                WriteTable("double");
                            }
                            break;
                        case "double[]?":
                            {
                                builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 0) {{");
                                WriteTable("double");
                                builder.AppendLine($"}} else {{ {returnValue.Name} = null; }}");
                            }
                            break;
                        case "string[]":
                            {
                                WriteTable("string");
                            }
                            break;
                        case "string[]?":
                            {
                                builder.AppendLine($"if (LuaNative.lua_isnil(L, {stackIndex}) == 0) {{");
                                WriteTable("string");
                                builder.AppendLine($"}} else {{ {returnValue.Name} = null; }}");
                            }
                            break;
                        case "NoitaNET.API.Gui.GuiUserData":
                            builder.AppendLine($"{returnValue.Name} = new NoitaNET.API.Gui.GuiUserData(LuaNative.lua_touserdata(L, {stackIndex}));");
                            break;
                    }
                }
            }

            builder.AppendLine($"LuaNative.lua_settop(L, 0);");

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
