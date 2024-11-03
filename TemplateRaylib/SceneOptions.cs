using System.Diagnostics;
using Raylib_cs;

public class SceneOptions : Scene
{
    // Déclaration du bouton "Retour" avec ses propriétés.
    Button backButton = new Button
    {
        Rec = new Rectangle(10, 90, 200, 40),
        Text = "Retour",
        Color = Color.White
    };

    // Déclaration du bouton "OK" avec ses propriétés.
    Button okButton = new Button
    {
        Rec = new Rectangle(10 + 200 + 5, 90, 200, 40),
        Text = "OK",
        Color = Color.White
    };

    private ButtonsList buttonsList = new ButtonsList(); // Liste des boutons à afficher.
    private bool isFullScreen; // Booléen pour vérifier si le mode plein écran est activé.

    public SceneOptions()
    {
        buttonsList.AddButton(okButton);
        buttonsList.AddButton(backButton);
        isFullScreen = GameState.Instance.fullScreen; // Définit l'état du plein écran à partir de l'instance de GameState.
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Update()
    {
        buttonsList.Update(); // Met à jour l'état de chaque bouton dans la liste.

        // Augmente le volume
        if (Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            if (GameState.Instance.masterVolume < 1f)
            {
                GameState.Instance.SetVolume(GameState.Instance.masterVolume + .01f);
            }
        }
        // Diminue le volume
        if (Raylib.IsKeyPressed(KeyboardKey.Left))
        {
            if (GameState.Instance.masterVolume > 0f)
            {
                GameState.Instance.SetVolume(GameState.Instance.masterVolume - .01f);
            }
        }

        // Plein écran si la touche "F" est pressée.
        if (Raylib.IsKeyPressed(KeyboardKey.F))
        {
            isFullScreen = !isFullScreen;
        }


        if (backButton.IsClicked)
        {
            GameState.Instance.ChangeScene("menu");
        }

        else if (okButton.IsClicked)
        {
            // Sauvegarde les options seulement si le statut de plein écran a changé.
            if (isFullScreen != GameState.Instance.fullScreen)
            {
                //Raylib.ToggleFullscreen(); // Active/désactive le plein écran.
            }
            OptionsFile optionsFile = new OptionsFile();

            // Sauvegarde du volume et de l'état plein écran dans options.
            optionsFile.AddOption("volume", GameState.Instance.masterVolume);
            optionsFile.AddOption("fullscreen", isFullScreen);
            // var testObject = new { mana =100, arrows = 10, PV = 10};    // test pour sauvegarde objet dans options.

            optionsFile.Save(); // Sauvegarde les options dans le fichier options.json.

            GameState.Instance.ChangeScene("menu");
        }

        base.Update();
    }

    public override void Draw()
    {
        Raylib.DrawText("OPTIONS", 5, 5, 25, Color.Black);
        int screenWidth = Raylib.GetScreenWidth();
        Raylib.DrawLine(0, 30, screenWidth, 30, Color.Black);

        int pourcent = (int)(GameState.Instance.masterVolume * 100); // Convertit le volume en pourcentage.
        Raylib.DrawText($"Volume : {pourcent} %", 10, 35, 20, Color.Black);

        // Affiche l'état du plein écran (Oui/Non).
        string bFull = "Non";
        if (isFullScreen)
        {
            bFull = "Oui";
        }
        Raylib.DrawText($"Plein écran : {bFull}", 10, 65, 20, Color.Black);

        buttonsList.Draw();

        base.Draw();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Close()
    {
        Debug.WriteLine("Destruction de la scene options");
        base.Close();
    }
}
