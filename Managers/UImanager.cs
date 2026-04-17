using Godot;


public partial class UImanager : Control
{
    [Export] private bool debugMode = false; // Helps troubleshoot focus issues
    
    public override void _Ready()
    {
        // Defer focus grabbing to ensure everything is fully loaded
        
        CallDeferred(nameof(SetInitialFocus));
        CollectAllControls(this);
    }

    private void CollectAllControls(Node node)
    {
        foreach (Node child in node.GetChildren())
            CollectAllControls(child);
    }
    
    private void SetInitialFocus()
    {
        // Get the topmost parent (the scene root)
        Node rootNode = this;
        while (rootNode.GetParent() != null)
            rootNode = rootNode.GetParent();
        
        // Search from the scene root
        Control focusTarget = FindFirstFocusableControl(rootNode);
        
        if (focusTarget != null)
        {
            if (debugMode)
                GD.Print($"Setting focus to: {focusTarget.Name} ({focusTarget.GetType()})");
            
            focusTarget.CallDeferred("grab_focus");
        }
        else if (debugMode)
        {
            GD.Print("No focusable control found in this UI");
        }
    }
    
    private Control FindFirstFocusableControl(Node node)
    {
        // Check if current node is a focusable control
        if (IsFocusableControl(node))
            return (Control)node;
        
        // Recursively search children
        foreach (Node child in node.GetChildren())
        {
            Control found = FindFirstFocusableControl(child);
            if (found != null)
                return found;
        }
        
        return null;
    }
    
    private bool IsFocusableControl(Node node)
    {
        // Must be a Control node
        if (node is not Control control)
            return false;
        
        // Check if it's a type that can receive focus
        bool isFocusableType =  control is Button ||
                                control is TextureButton || 
                                control is HSlider ||
                                control is VSlider ||
                                control is CheckBox ||
                                control is CheckButton ||
                                control is OptionButton ||
                                control is ColorPickerButton ||
                                control is LineEdit ||
                                control is TextEdit ||
                                control is SpinBox ||
                                control is Tree ||
                                control is ItemList ||
                                control is MenuButton;
        
        if (!isFocusableType)
            return false;
        
        // Check if the control is actually focusable (not disabled/hidden)
        bool canActuallyFocus = control.FocusMode != FocusModeEnum.None &&
                                control.Visible &&
                                control.GetRect().Size.X > 0 &&
                                control.GetRect().Size.Y > 0;
        
        return canActuallyFocus;
    }
}