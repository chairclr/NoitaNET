using System.Collections;
using System.Diagnostics;
using System.Net;

namespace NoitaNET.API.Noita.Tags;

public class ComponentTagCollection : NoitaObject, ICollection
{
    private Component Component;

    public int Count
    {
        get
        {
            EngineAPI.ComponentGetTags(Component.Id, out string? stringTags);

            if (string.IsNullOrEmpty(stringTags))
                throw new Exception("Component no longer exists");

            // + 1 because tags don't end with ,
            return stringTags.Count(x => x == ',') + 1;
        }
    }

    public bool IsSynchronized => false;

    private object _syncRoot = new object();

    public object SyncRoot => _syncRoot;

    public ComponentTagCollection(Component component)
        : base (component.LuaManager)
    {
        Component = component;
    }

    public void Add(string tag)
    {
        EngineAPI.ComponentAddTag(Component.Id, tag);
    }

    public void Remove(string tag)
    {
        EngineAPI.ComponentRemoveTag(Component.Id, tag);
    }

    public bool Contains(string tag)
    {
        EngineAPI.ComponentHasTag(Component.Id, tag, out bool hasTag);

        return hasTag;
    }

    public string[] ToArray()
    {
        EngineAPI.ComponentGetTags(Component.Id, out string? stringTags);

        if (string.IsNullOrEmpty(stringTags))
            throw new Exception("Component no longer exists");

        return stringTags.Split(',');
    }

    public void CopyTo(Array array, int index)
    {
        if (array.GetType() != typeof(string[]))
        {
            throw new ArgumentException($"cannot convert from '{array.GetType()}' to 'string[]'", nameof(array));
        }

        EngineAPI.ComponentGetTags(Component.Id, out string? stringTags);

        if (string.IsNullOrEmpty(stringTags))
            throw new Exception("Component no longer exists");

        string[] splits = stringTags.Split(',');

        if (array.Length < splits.Length + index)
        {
            throw new ArgumentOutOfRangeException(nameof(array));
        }

        string[] arr = (string[])array;

        for (int i = index; i < splits.Length + index; i++)
        {
            arr[i] = splits[i];
        }
    }

    public IEnumerator GetEnumerator()
    {
        EngineAPI.ComponentGetTags(Component.Id, out string? stringTags);

        if (string.IsNullOrEmpty(stringTags))
            throw new Exception("Component no longer exists");

        string[] splits = stringTags.Split(',');

        for (int i = 0; i < splits.Length; i++)
        {
            yield return splits[i];
        }
    }
}
