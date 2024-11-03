using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

public class SceneMenu : Scene
{
    private Button playButton;
    private Button optionsButton;
    private Button quitButton;
    private ButtonsList buttonsList = new ButtonsList();
    public SceneMenu()
    {
        optionsButton = new Button { Rec = new Rectangle(Raylib.GetScreenWidth() / 2 - 200 / 2, Raylib.GetScreenHeight() / 2 - 20 * 2, 200, 40), Text = "Options", Color = Color.White };
        playButton = new Button { Rec = new Rectangle(optionsButton.Rec.X, optionsButton.Rec.Y - optionsButton.Rec.Height - 5, 200, 40), Text = "Jouer", Color = Color.White };
        quitButton = new Button { Rec = new Rectangle(playButton.Rec.X, optionsButton.Rec.Y + optionsButton.Rec.Height + 5, 200, 40), Text = "Quitter", Color = Color.White };
        buttonsList.AddButton(playButton);
        buttonsList.AddButton(optionsButton);
        buttonsList.AddButton(quitButton);
    }
    public override void Draw()
    {
        Raylib.DrawText("MENU", 5, 5, 20, Color.Black);

        buttonsList.Draw();

        base.Draw();
    }

    public override void Update()
    {
        GameState.Instance.debugMagic.AddOption("Scale", GameState.Instance.gameScreenScale);   // monitoring debug

        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            GameState.Instance.ChangeScene("gameplay");
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.O))
        {
            GameState.Instance.ChangeScene("options");
        }

        buttonsList.Update();
        if (playButton.IsClicked)
        {
            GameState.Instance.ChangeScene("gameplay");
        }
        else if (optionsButton.IsClicked)
        {
            GameState.Instance.ChangeScene("options");
        }
        base.Update();

        // if (quitButton.IsClicked)
        // {
        //     GameState.Instance.RemoveScene("gameplay");
        //     GameState.Instance.RemoveScene("options");
        //     GameState.Instance.RemoveScene("menu");
        //     Raylib.EndDrawing();
        //     Raylib.CloseWindow();
        // }
    }
}