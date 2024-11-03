using Raylib_cs;
using System.Numerics;

public class Button
{
    public Rectangle Rec { get; set; }
    public string? Text { get; set; }
    public Color Color { get; set; }
    public Color OriginalColor { get; set; }
    public bool IsClicked { get; set; } = false;
}

public class ButtonsList
{
    private List<Button> buttons = new List<Button>();

    public void AddButton(Button button)
    {
        button.OriginalColor = button.Color;
        buttons.Add(button);
    }

    public void Update()
    {
        Vector2 mousePos = Raylib.GetMousePosition();
        // Calcul position souris pour la fenêtre redimensionnée
        mousePos.X -= GameState.Instance.gameScreenOffsetX;
        mousePos.X /= GameState.Instance.gameScreenScale;
        mousePos.Y -= GameState.Instance.gameScreenOffsetY;
        mousePos.Y /= GameState.Instance.gameScreenScale;

        foreach (Button button in buttons)
        {
            button.IsClicked = false;
            if (Raylib.CheckCollisionPointRec(mousePos, button.Rec))
            {
                button.Color = Color.LightGray;
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    button.IsClicked = true;
                }
            }
            else
            {
                button.Color = button.OriginalColor;
            }
        }
    }

    public void Draw()
    {
        foreach (Button button in buttons)
        {
            Raylib.DrawRectangleRec(button.Rec, button.Color);
            Raylib.DrawRectangleLinesEx(button.Rec, 2, Color.Black);
            Raylib.DrawText(button.Text, (int)button.Rec.X + 10, (int)button.Rec.Y + 10, 20, Color.Black);      // cast en int pour drawtext à cause du float de XY
        }
    }
}