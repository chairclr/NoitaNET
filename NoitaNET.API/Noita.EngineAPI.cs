using System.Xml.Linq;
using NoitaNET.API.Lua;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NoitaNET.API;

unsafe partial class Noita
{
    public void ComponentGetValue2(int component_id, string field_name, out nint? int_value)
    {
        LuaNative.lua_pushinteger(L, component_id);
        LuaNative.lua_pushstring(L, field_name);
        EngineAPIFunctionTable.ComponentGetValue2(L);

        if (LuaNative.lua_isnil(L, -1) == 1)
        {
            int_value = null;
        }
        else
        {
            int_value = LuaNative.lua_tointeger(L, -1);
        }

        LuaNative.lua_settop(L, 0);
    }

    public void ComponentGetValue2(int component_id, string field_name, out double? number_value)
    {
        LuaNative.lua_pushinteger(L, component_id);
        LuaNative.lua_pushstring(L, field_name);
        EngineAPIFunctionTable.ComponentGetValue2(L);

        if (LuaNative.lua_isnil(L, -1) == 1)
        {
            number_value = null;
        }
        else
        {
            number_value = LuaNative.lua_tonumber(L, -1);
        }

        LuaNative.lua_settop(L, 0);
    }

    public void ComponentGetValue2(int component_id, string field_name, out string? string_value)
    {
        LuaNative.lua_pushinteger(L, component_id);
        LuaNative.lua_pushstring(L, field_name);
        EngineAPIFunctionTable.ComponentGetValue2(L);

        if (LuaNative.lua_isnil(L, -1) == 1)
        {
            string_value = null;
        }
        else
        {
            string_value = LuaNative.lua_tostring(L, -1);
        }

        LuaNative.lua_settop(L, 0);
    }

    public void ComponentSetValue2(int component_id, string field_name, nint int_value)
    {
        LuaNative.lua_pushinteger(L, component_id);
        LuaNative.lua_pushstring(L, field_name);
        LuaNative.lua_pushinteger(L, int_value);
        EngineAPIFunctionTable.ComponentSetValue2(L);
        LuaNative.lua_settop(L, 0);
    }

    public void ComponentSetValue2(int component_id, string field_name, double number_value)
    {
        LuaNative.lua_pushinteger(L, component_id);
        LuaNative.lua_pushstring(L, field_name);
        LuaNative.lua_pushnumber(L, number_value);
        EngineAPIFunctionTable.ComponentSetValue2(L);
        LuaNative.lua_settop(L, 0);
    }

    public void ComponentSetValue2(int component_id, string field_name, string string_value)
    {
        LuaNative.lua_pushinteger(L, component_id);
        LuaNative.lua_pushstring(L, field_name);
        LuaNative.lua_pushstring(L, string_value);
        EngineAPIFunctionTable.ComponentSetValue2(L);
        LuaNative.lua_settop(L, 0);
    }

    // TODO
    public void ComponentObjectGetValue2()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void ComponentObjectSetValue2()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void EntityAddComponent2()
    {
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// 'type_stored_in_vector' should be "int", "float" or "string".
    /// </summary>
    public void ComponentGetVectorValue()
    {
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// 'type_stored_in_vector' should be "int", "float" or "string".
    /// </summary>
    public void ComponentGetVector()
    {
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// Returns a string-indexed table of string.
    /// </summary>
    public void ComponentGetMembers()
    {
        //LuaNative.lua_pushinteger(L, component_id);
        //EngineAPIFunctionTable.ComponentGetMembers(L);
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// Returns a string-indexed table of string or nil.
    /// </summary>
    public void ComponentObjectGetMembers()
    {
        //LuaNative.lua_pushinteger(L, component_id);
        //LuaNative.lua_pushstring(L, object_name);
        //EngineAPIFunctionTable.ComponentObjectGetMembers(L);
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    public void BiomeSetValue(string filename, string field_name, nint int_value)
    {
        LuaNative.lua_pushstring(L, filename);
        LuaNative.lua_pushstring(L, field_name);
        LuaNative.lua_pushinteger(L, int_value);
        EngineAPIFunctionTable.ComponentSetValue2(L);

        LuaNative.lua_settop(L, 0);
    }

    public void BiomeSetValue(string filename, string field_name, double number_value)
    {
        LuaNative.lua_pushstring(L, filename);
        LuaNative.lua_pushstring(L, field_name);
        LuaNative.lua_pushnumber(L, number_value);
        EngineAPIFunctionTable.ComponentSetValue2(L);

        LuaNative.lua_settop(L, 0);
    }

    public void BiomeSetValue(string filename, string field_name, string string_value)
    {
        LuaNative.lua_pushstring(L, filename);
        LuaNative.lua_pushstring(L, field_name);
        LuaNative.lua_pushstring(L, string_value);
        EngineAPIFunctionTable.ComponentSetValue2(L);

        LuaNative.lua_settop(L, 0);
    }

    public void BiomeGetValue(string filename, string field_name, out nint? int_value)
    {
        LuaNative.lua_pushstring(L, filename);
        LuaNative.lua_pushstring(L, field_name);
        EngineAPIFunctionTable.BiomeGetValue(L);

        if (LuaNative.lua_isnil(L, -1) == 1)
        {
            int_value = null;
        }
        else
        {
            int_value = LuaNative.lua_tointeger(L, -1);
        }

        LuaNative.lua_settop(L, 0);
    }

    public void BiomeGetValue(string filename, string field_name, out double? number_value)
    {
        LuaNative.lua_pushstring(L, filename);
        LuaNative.lua_pushstring(L, field_name);
        EngineAPIFunctionTable.BiomeGetValue(L);

        if (LuaNative.lua_isnil(L, -1) == 1)
        {
            number_value = null;
        }
        else
        {
            number_value = LuaNative.lua_tonumber(L, -1);
        }

        LuaNative.lua_settop(L, 0);
    }

    public void BiomeGetValue(string filename, string field_name, out string? string_value)
    {
        LuaNative.lua_pushstring(L, filename);
        LuaNative.lua_pushstring(L, field_name);
        EngineAPIFunctionTable.BiomeGetValue(L);

        if (LuaNative.lua_isnil(L, -1) == 1)
        {
            string_value = null;
        }
        else
        {
            string_value = LuaNative.lua_tostring(L, -1);
        }

        LuaNative.lua_settop(L, 0);
    }

    // TODO
    public void BiomeGetValue()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void BiomeObjectSetValue()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void BiomeVegetationSetValue()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void BiomeMaterialSetValue()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void BiomeMaterialGetValue()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void GameCreateCosmeticParticle()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns <see cref="double"/> between 0.0 and 1.0
    /// </summary>
    public void Random(out double return_value_number)
    {
        EngineAPIFunctionTable.Random(L);
        return_value_number = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="int"/> between 0 and <paramref name="a"/>
    /// </summary>
    public void Random(nint a, out nint return_value_int)
    {
        LuaNative.lua_pushinteger(L, a);
        EngineAPIFunctionTable.Random(L);
        return_value_int = LuaNative.lua_tointeger(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="int"/> between <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    public void Random(nint a, nint b, out nint return_value_int)
    {
        LuaNative.lua_pushinteger(L, a);
        LuaNative.lua_pushinteger(L, b);
        EngineAPIFunctionTable.Random(L);
        return_value_int = LuaNative.lua_tointeger(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="double"/> between 0.0 and 1.0
    /// </summary>
    public void Randomf(out double return_value_number)
    {
        EngineAPIFunctionTable.Randomf(L);
        return_value_number = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="double"/> between 0.0 and <paramref name="max"/>
    /// </summary>
    public void Randomf(double max, out double return_value_number)
    {
        LuaNative.lua_pushnumber(L, max);
        EngineAPIFunctionTable.Randomf(L);
        return_value_number = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="double"/> between <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    public void Randomf(double min, double max, out double return_value_number)
    {
        LuaNative.lua_pushnumber(L, min);
        LuaNative.lua_pushnumber(L, max);
        EngineAPIFunctionTable.Randomf(L);
        return_value_number = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="double"/> between 0.0 and 1.0
    /// </summary>
    public void ProceduralRandom(double x, double y, out double return_value_number)
    {
        LuaNative.lua_pushnumber(L, x);
        LuaNative.lua_pushnumber(L, y);
        EngineAPIFunctionTable.ProceduralRandom(L);
        return_value_number = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="nint"/> between 0 and <paramref name="a"/>
    /// </summary>
    public void ProceduralRandom(double x, double y, nint a, out nint return_value_int)
    {
        LuaNative.lua_pushnumber(L, x);
        LuaNative.lua_pushnumber(L, y);
        LuaNative.lua_pushnumber(L, a);
        EngineAPIFunctionTable.ProceduralRandom(L);
        return_value_int = LuaNative.lua_tointeger(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    /// <summary>
    /// Returns <see cref="double"/> between <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    public void ProceduralRandom(double x, double y, double a, double b, out double return_value_number)
    {
        LuaNative.lua_pushnumber(L, x);
        LuaNative.lua_pushnumber(L, y);
        LuaNative.lua_pushnumber(L, a);
        LuaNative.lua_pushnumber(L, b);
        EngineAPIFunctionTable.ProceduralRandom(L);
        return_value_number = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
    }

    // TODO 
    public void PhysicsApplyForceOnArea()
    {
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// Returns the value of a mod setting. 'id' should normally be in the format 'mod_name.setting_id'. Cache the returned value in your lua context if possible.
    /// </summary>
    public void ModSettingGet()
    {
        //LuaNative.lua_pushstring(L, id);
        //EngineAPIFunctionTable.ModSettingGet(L);
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// Sets the value of a mod setting. 'id' should normally be in the format 'mod_name.setting_id'.
    /// </summary>
    public void ModSettingSet()
    {
        //LuaNative.lua_pushstring(L, id);
        //EngineAPIFunctionTable.ModSettingSet(L);
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// Returns the latest value set by the user, which might not be equal to the value that is used in the game (depending on the 'scope' value selected for the setting).
    /// </summary>
    public void ModSettingGetNextValue()
    {
        //LuaNative.lua_pushstring(L, id);
        //EngineAPIFunctionTable.ModSettingGetNextValue(L);
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// Sets the latest value set by the user, which might not be equal to the value that is displayed to the game (depending on the 'scope' value selected for the setting).
    /// </summary>
    public void ModSettingSetNextValue()
    {
        //LuaNative.lua_pushstring(L, id);
        //LuaNative.lua_pushboolean(L, is_default ? 1 : 0);
        //EngineAPIFunctionTable.ModSettingSetNextValue(L);
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    // TODO
    /// <summary>
    /// 'index' should be 0-based index. Returns nil if 'index' is invalid.
    /// </summary>
    public void ModSettingGetAtIndex()
    {
        //LuaNative.lua_pushinteger(L, index);
        //EngineAPIFunctionTable.ModSettingGetAtIndex(L);
        //return_value_(name = LuaNative.lua_tostring(L, -1)!;
        //LuaNative.lua_settop(L, 0);
        throw new NotImplementedException();
    }

    // TODO
    public void dofile()
    {
        throw new NotImplementedException();
    }

    // TODO
    public void dofile_once()
    {
        throw new NotImplementedException();
    }
}
