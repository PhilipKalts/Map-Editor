using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// A test to see the Map Editor in Action
/// Press the button from the inspector and see which are the exits from the left and right
/// on the chosen scene
/// </summary>
namespace MapEditor
{
    public class TestTree : MonoBehaviour
    {
        [SerializeField] MapTree mapTree;
        [SerializeField, Scene] string sceneToFindExit;

        [Button]
        void FindExits()
        {
            if (mapTree == null) return;

            MapNode mapNode = mapTree.AllNodes.Find(x => x.SceneName == sceneToFindExit);

            if (mapNode == null) return;

            string rightExits = "";
            string leftExits = "";

            for (int i = 0; i < mapNode.LeftExits.Count; i++) leftExits += mapNode.LeftExits[i].SceneName + "\n";
            for (int i = 0; i < mapNode.RightExits.Count; i++) rightExits += mapNode.RightExits[i].SceneName + "\n";

            print($"The left exits are \n{leftExits} \nand the right exits are \n{rightExits}");
        }
    }
}