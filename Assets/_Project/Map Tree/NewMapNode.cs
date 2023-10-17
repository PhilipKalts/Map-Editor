using UnityEngine;

/// <summary>
/// This class only purpose is cause when the editor searches for what classes
/// it can create as nodes it doesn't take the original one "MapNode" but only the
/// classes derived from it. So this empty class is just to show the option to add a Map Node
/// </summary>
namespace MapEditor
{
    public class NewMapNode : MapNode
    {

    }
}