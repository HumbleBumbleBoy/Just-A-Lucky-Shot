using Godot;
using System;

[Tool]
public partial class NavigationalButton : TextureButton
{
    [Export] public string Text;
    [Export] public string Path;

    private RichTextLabel textContainer;

    public override void _Ready()
    {
        textContainer = GetNode<RichTextLabel>("Text");
        updateButtonText();
    }

    private void onPressed()
    {
        if (Path == "Quit") { GetTree().Quit(); }
        
        if (!string.IsNullOrEmpty(Path))
        {
            GetTree().ChangeSceneToFile(Path);
        }
    }

    private void updateButtonText()
    {
        if (Text != null && textContainer != null)
        {
            textContainer.Text = Text;
        }
    }
}