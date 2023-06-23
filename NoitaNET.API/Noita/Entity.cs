using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoitaNET.API.Noita;

public class Entity : NoitaObject
{
    internal nint Id;

    internal Entity(LuaManager luaManager, nint id) 
        : base(luaManager)
    {
        Id = id;
    }
}
