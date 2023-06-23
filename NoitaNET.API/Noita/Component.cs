using NoitaNET.API.Noita.Tags;

namespace NoitaNET.API.Noita;

public abstract class Component : NoitaObject
{
    internal nint Id;

    public Entity Entity;

    public bool Enabled
    {
        get
        {
            EngineAPI.ComponentGetIsEnabled(Id, out bool enabled);

            return enabled;
        }
        set
        {
            EngineAPI.EntitySetComponentIsEnabled(Entity.Id, Id, value);

        }
    }

    public ComponentTagCollection Tags
    {
        get
        {
            return new ComponentTagCollection(this);
        }
    }

    public string Name
    {
        get
        {
            EngineAPI.ComponentGetTypeName(Id, out string typeName);

            return typeName;
        }
    }

    internal Component(LuaManager luaManager, Entity entity, nint id)
        : base(luaManager)
    {
        Entity = entity;

        Id = id;
    }
}
