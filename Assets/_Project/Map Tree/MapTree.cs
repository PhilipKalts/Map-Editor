using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// This is the main SO we reference and all of our Map Nodes are stored
/// </summary>
namespace MapEditor
{
    [CreateAssetMenu(fileName = "New Map Editor", menuName = "Map Editor")]
    public class MapTree : ScriptableObject
    {
        public List<MapNode> AllNodes = new List<MapNode>();



        #region Unity Editor
#if UNITY_EDITOR

        #region Create / Delete Nodes

        public MapNode CreateNode(System.Type type)
        {
            MapNode node = ScriptableObject.CreateInstance(type) as MapNode;
            node.name = type.Name;
            node.Guid = GUID.Generate().ToString();
            AllNodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(MapNode node)
        {
            AllNodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        #endregion



        #region Add / Remove Nodes From Lists

        public void AddExitLeft(MapNode parent, MapNode child)
        {
            child.LeftExits.Add(parent);
            AssetDatabase.SaveAssets();
        }

        public void RemoveExitLeft(MapNode parent, MapNode child)
        {
            child.LeftExits.Remove(parent);
            AssetDatabase.SaveAssets();
        }

        public void AddExitRight(MapNode parent, MapNode child)
        {
            parent.RightExits.Add(child);
            AssetDatabase.SaveAssets();
        }

        public void RemoveExitRight(MapNode parent, MapNode child)
        {
            parent.RightExits.Remove(child);
            AssetDatabase.SaveAssets();
        }

        #endregion

#endif
        #endregion


        public MapNode GetCurrentSceneData()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            return AllNodes.Find(node => node.SceneName == currentScene);
        }
    }
}