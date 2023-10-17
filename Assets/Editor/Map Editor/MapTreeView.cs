using System;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// This class is responsible for creating and adjusting the graph editor for the Map
/// When something changes in the graph editor this class knows it and notifies everything else
/// </summary>
namespace MapEditor
{
    public class MapTreeView : GraphView
    {
        // This class is so it can visible / added to the UI Builder
        public new class UxmlFactory : UxmlFactory<MapTreeView, GraphView.UxmlTraits> { }

        #region Variables

        public Action<MapNodeView> OnNodeSelected;
        MapTree tree;

        #endregion


        // Constructor
        public MapTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Map Editor/MapEditor.uss");
            styleSheets.Add(styleSheet);
        }

        

        #region Populate Nodes

        internal void PopulateView(MapTree tree)
        {
            this.tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            // Create Nodes
            tree.AllNodes.ForEach(n => CreateNodeView(n));
            
            // Create Edges
            tree.AllNodes.ForEach(n => CreateEdges(n));

            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }


        void CreateNodeView(MapNode node)
        {
            MapNodeView nodeView = new MapNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }


        void CreateNode(System.Type type)
        {
            MapNode node = tree.CreateNode(type);
            CreateNodeView(node);
        }


        void CreateEdges(MapNode node)
        {
            node.RightExits.ForEach(child =>
            {
                MapNodeView parentView = FindNodeView(node);
                MapNodeView childView = FindNodeView(child);

                Edge edge = parentView.OutputPorts.ConnectTo(childView.InputPorts);
                AddElement(edge);
            });

            MapNodeView FindNodeView(MapNode node)
            {
                return GetNodeByGuid(node.Guid) as MapNodeView;
            }
        }
        

        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null) RemoveElements();
            if (graphViewChange.edgesToCreate != null) CreateEdges();

            AssetDatabase.SaveAssets();
            return graphViewChange;



            void RemoveElements()
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    MapNodeView nodeView = elem as MapNodeView;
                    if (nodeView != null) tree.DeleteNode(nodeView.MapNode);

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        MapNodeView parentView = edge.output.node as MapNodeView;
                        MapNodeView childView = edge.input.node as MapNodeView;

                        tree.RemoveExitLeft(parentView.MapNode, childView.MapNode);
                        tree.RemoveExitRight(parentView.MapNode, childView.MapNode);
                    }
                });
            }


            void CreateEdges()
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    MapNodeView parentView = edge.output.node as MapNodeView;
                    MapNodeView childView = edge.input.node as MapNodeView;

                    if (parentView.MapNode.RightExits.Contains(childView.MapNode)) return;

                    tree.AddExitLeft(parentView.MapNode, childView.MapNode);
                    tree.AddExitRight(parentView.MapNode, childView.MapNode);
                });
            }
        }

        #endregion



        #region Overrides

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<MapNode>();
            foreach (var type in types)
                evt.menu.AppendAction("New Map Node", (a) => CreateNode(type));
        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }

        #endregion
    }
}