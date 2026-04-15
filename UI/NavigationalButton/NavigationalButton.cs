using Godot;
using System;

[Tool]
public partial class NavigationalButton : TextureButton
{
    [Export] public string Text;
    [Export] public string Path;
    [Export] public AudioStreamPlayer AudioStreamPlayer;

    private RichTextLabel textContainer;

    public override void _Ready()
    {
        textContainer = GetNode<RichTextLabel>("Text");
        updateButtonText();
    }

    private async void onPressed()
    {
        AudioStreamPlayer.Play();
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout); // wait for sfx to load

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

    private void OnMouseHover()
    {
        textContainer.PivotOffsetRatio = new Vector2(0.5f, 0.5f);
        textContainer.Scale = new Vector2(1.1f, 1.1f);
        textContainer.SelfModulate = new Color(128, 32, 0, 1);
    }

    private void OnMouseHoverExit()
    {
        textContainer.PivotOffsetRatio = new Vector2(1.0f, 1.0f);
        textContainer.Scale = Vector2.One;
        textContainer.SelfModulate = new Color(255, 255, 255, 1);
    }
}