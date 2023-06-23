using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoitaNET.API.Noita;

public abstract class NoitaObject
{
    internal readonly LuaManager LuaManager;

    protected EngineAPI EngineAPI => LuaManager.ThreadLocalEngineAPI.Value!;

    public NoitaObject(LuaManager luaManager)
    {
        LuaManager = luaManager;
    }
}
