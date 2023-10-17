using MapEditor;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;

/// <summary>
/// Opens the Editor Graph and adds 
/// </summary>
public class MapEditorWindow : EditorWindow
{
    #region Variables

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    MapTreeView treeView;
    MapInspectorView inspectorView;

    #endregion



    #region Open Window

    [MenuItem("Custom Editors/Map Editor")]
    public static void OpenWindow()
    {
        MapEditorWindow wnd = GetWindow<MapEditorWindow>();
        wnd.titleContent = new GUIContent("MapEditor");
    }

    // When we double click the asset from the project, the window opens
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is MapTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    #endregion



    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Map Editor/MapEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Map Editor/MapEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<MapTreeView>();
        inspectorView = root.Q<MapInspectorView>();

        treeView.OnNodeSelected = OnNodeSelectionChanged;

        OnSelectionChange();
    }



    private void OnSelectionChange()
    {
        MapTree tree = Selection.activeObject as MapTree;
        if (tree == null) return;

        treeView.PopulateView(tree);
    }


    void OnNodeSelectionChanged(MapNodeView nodeView) => inspectorView.UpdateSelection(nodeView);
}
