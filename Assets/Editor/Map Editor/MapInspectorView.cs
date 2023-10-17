using UnityEditor;
using UnityEngine.UIElements;

/// <summary>
/// When we select a node we draw the left panel as the inpector of this node
/// </summary>
namespace MapEditor
{
    public class MapInspectorView : VisualElement
    {
        // Create this class for the UI Builder
        public new class UxmlFactory : UxmlFactory<MapInspectorView, VisualElement.UxmlTraits> { }
        Editor editor;

        public MapInspectorView() { }
        

        internal void UpdateSelection(MapNodeView nodeView)
        {
            // Clear the previous inspector
            Clear();
            UnityEngine.Object.DestroyImmediate(editor);

            // Create the new one
            editor = Editor.CreateEditor(nodeView.MapNode);
            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}