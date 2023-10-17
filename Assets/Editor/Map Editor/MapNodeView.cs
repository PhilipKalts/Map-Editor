using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// Controls the Individual Nodes inside the editor graph
/// Creates the ports, title, position, notifies when it get's selected etc.
/// </summary>
namespace MapEditor
{
    public class MapNodeView : UnityEditor.Experimental.GraphView.Node
    {
        #region Variables

        public Action<MapNodeView> OnNodeSelected;
        public MapNode MapNode;
        public Port InputPorts, OutputPorts;
        Image image;

        #endregion



        #region Create Node

        // Constructor
        public MapNodeView(MapNode node)
        {
            this.MapNode = node;
            viewDataKey = node.Guid;

            MapNode.OnChangeSprite = ChangeSprite;
            MapNode.OnChangeSceneName = AssignTitle;

            style.left = MapNode.Position.x;
            style.top = MapNode.Position.y;

            AssignTitle();
            CreateImage();
            ChangeSprite();
            CreateInputPorts();
            CreateOutputPorts();
        }


        void AssignTitle() => title = MapNode.SceneName;


        void ChangeSprite() => image.sprite = MapNode.Sprite;


        void CreateImage()
        {
            image = new Image();
            mainContainer.Add(image);
        }


        void CreateInputPorts()
        {
            InputPorts = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            if (InputPorts == null) return;

            InputPorts.portName = "Left Exits";
            inputContainer.Add(InputPorts);
        }


        void CreateOutputPorts()
        {
            OutputPorts = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            if (OutputPorts == null) return;

            OutputPorts.portName = "Right Exits";
            outputContainer.Add(OutputPorts);
        }

        #endregion



        #region Overrides

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            MapNode.Position.x = newPos.x;
            MapNode.Position.y = newPos.y;
        }
        

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        #endregion
    }
}