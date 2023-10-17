using UnityEngine.UIElements;

/// <summary>
/// Adds a split view element in the UI Builder
/// </summary>
public class SplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
}