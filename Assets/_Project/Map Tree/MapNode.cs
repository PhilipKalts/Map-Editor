using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

/// <summary>
/// This is the template for the nodes in the Map Editor
/// </summary>
namespace MapEditor
{
    public abstract class MapNode : ScriptableObject
    {
        [Scene, OnValueChanged(nameof(ChangeScene))]
        public string SceneName;
        
        [OnValueChanged(nameof(ChangeSprite))]
        public Sprite Sprite;

        [HideInInspector] public string Guid;
        [HideInInspector] public Vector2 Position;

        [Space(15)]
        [Header("Exits")]
        [HorizontalLine(color: EColor.Red)]
        public List<MapNode> LeftExits = new List<MapNode>();
        public List<MapNode> RightExits = new List<MapNode>();


        // For the Nodes
        public System.Action OnChangeSceneName, OnChangeSprite;
        void ChangeSprite() => OnChangeSprite?.Invoke();
        void ChangeScene() => OnChangeSceneName?.Invoke();
    }
}