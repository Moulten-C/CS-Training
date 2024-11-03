using Raylib_cs;

public class DebugMagic : OptionsFile
{
    private Rectangle debugFrame;
    private bool showDebug = false;

    public DebugMagic() : base()
    {
        debugFrame = new Rectangle(0, 0, 300, Raylib.GetScreenHeight());
    }

    public void ToggleDebug()
    {
        showDebug = !showDebug;
    }

    public void Update()
    {
        debugFrame.Height = Raylib.GetScreenHeight();
        if (Raylib.IsKeyPressed(KeyboardKey.D))
        {
            ToggleDebug();
        }
    }

    public void Draw()
    {
        if (showDebug)
        {
            Raylib.DrawRectangleRec(debugFrame, new Color(0, 0, 0, 150));

            int y = 20;
            foreach (var option in options)
            {
                Raylib.DrawText(option.Key + " : " + option.Value, 20, y, 10, Color.White);
                y += 12;
            }
        }
    }

}