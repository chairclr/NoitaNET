using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;
using NoitaNET.API.Lua;

namespace NoitaNET.API;

public unsafe partial class Noita
{
    private readonly LuaNative.lua_State* L;

    public Noita()
    {
        L = LuaNative.luaL_newstate();
    }

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

    // TODO 
    public void PhysicsApplyForceOnArea()
    {
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


    //public int EntityLoad(string filename)
    //{
    //    LuaNative.lua_pushstring(L, filename);
    //    EngineAPIFunctionTable.EntityLoad(L);

    //    return (int)LuaNative.lua_tointeger(L, -1);
    //}

    //public int EntityLoad(string filename, Vector2 position)
    //{
    //    LuaNative.lua_pushstring(L, filename);
    //    LuaNative.lua_pushnumber(L, position.X);
    //    LuaNative.lua_pushnumber(L, position.Y);
    //    EngineAPIFunctionTable.EntityLoad(L);

    //    int result = (int)LuaNative.lua_tointeger(L, -1);

    //    LuaNative.lua_settop(L, 0);

    //    return result;
    //}

    //public void EntityKill(int id)
    //{
    //    LuaNative.lua_pushnumber(L, id);
    //    EngineAPIFunctionTable.EntityKill(L);
    //    LuaNative.lua_settop(L, 0);
    //}

    //public int[] EntityGetWithTag(string tag)
    //{
    //    LuaNative.lua_pushstring(L, tag);
    //    EngineAPIFunctionTable.EntityGetWithTag(L);

    //    int length = (int)LuaNative.lua_objlen(L, 2);

    //    int[] result = new int[length]; 

    //    for (int i = 0; i < length; i++)
    //    {
    //        LuaNative.lua_rawgeti(L, -1, i + 1);
    //        result[i] = (int)LuaNative.lua_tointeger(L, -1);

    //        LuaNative.lua_pop(L, 1); 
    //    }

    //    LuaNative.lua_settop(L, 0);

    //    return result;
    //}

    //public double Random()
    //{
    //    EngineAPIFunctionTable.Random(L);
    //    double n = LuaNative.lua_tonumber(L, -1);
    //    LuaNative.lua_settop(L, 0);
    //    return n;
    //}
}
