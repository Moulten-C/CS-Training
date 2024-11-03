using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

public static class Program
{
    static SceneMenu sceneMenu = new SceneMenu();
    static SceneGameplay sceneGameplay = new SceneGameplay();
    static SceneOptions sceneOptions = new SceneOptions();

    public static void Main()
    {
        int gameScreenWidth = 800;
        int gameScreenHeight = 600;

        Raylib.InitWindow(gameScreenWidth, gameScreenHeight, "Raylib-cs Framework Gamecodeur");
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
        //Raylib.SetWindowState(ConfigFlags.UndecoratedWindow);                 // fenêtre sans cadre
        Raylib.SetTargetFPS(60);

        RenderTexture2D DisplayTarget = Raylib.LoadRenderTexture(gameScreenWidth, gameScreenHeight);   // Rendu ecran virtuel
        Raylib.SetTextureFilter(DisplayTarget.Texture, TextureFilter.Point);                           // désactive anti-aliasing

        Raylib.InitAudioDevice();
        Raylib.SetExitKey(KeyboardKey.Q);
        Raylib.SetExitKey(KeyboardKey.A);

        GameState gameState = GameState.Instance;                               // Singleton donc pas de "new"

        gameState.SetVirtualGameResolution(gameScreenWidth, gameScreenHeight);

        gameState.RegisterScene("menu", sceneMenu);                             // Ajoute la scene "menu" dans le dictionnaire de scenes
        gameState.RegisterScene("gameplay", sceneGameplay);
        gameState.RegisterScene("options", sceneOptions);

        gameState.ChangeScene("menu");                                          // On commence pas l'affichage du menu

        Debug.WriteLine("Lancement du programme");

        while (!Raylib.WindowShouldClose())
        {
            // Aspect Ratio (Render to Texture) \\
            gameState.gameScreenScale = Math.Min(
                (float)Raylib.GetScreenWidth() / gameScreenWidth,
                (float)Raylib.GetScreenHeight() / gameScreenHeight);

            float resizedGameWidth = gameScreenWidth * gameState.gameScreenScale;
            gameState.gameScreenOffsetX = (Raylib.GetScreenWidth() - resizedGameWidth) / 2;

            float resizedGameHeight = gameScreenHeight * gameState.gameScreenScale;
            gameState.gameScreenOffsetY = (Raylib.GetScreenHeight() - resizedGameHeight) / 2;
            //-----------------------------------\\

            gameState.debugMagic.Update();
            gameState.UpdateScene();

            Raylib.BeginDrawing();                          // Draw dans fenêtre réelle
            Raylib.ClearBackground(Color.Black);

            Raylib.BeginTextureMode(DisplayTarget);                // Draw dans fenêtre virtuelle
            Raylib.ClearBackground(Color.LightGray);

            gameState.DrawScene();

            Raylib.EndTextureMode();

            Rectangle sourceRec = new Rectangle(0f, 0f, (float)DisplayTarget.Texture.Width, -(float)DisplayTarget.Texture.Height);                            // - height car OpenGL dessine avec Y=0 en bas de la fenêtre
            Rectangle destRec = new Rectangle(gameState.gameScreenOffsetX, gameState.gameScreenOffsetY, resizedGameWidth, resizedGameHeight);

            Raylib.DrawTexturePro(DisplayTarget.Texture, sourceRec, destRec, new Vector2(0, 0), 0f, Color.White);      // Dessine cadre rendu ecran virtuel dans cadre fenêtre réel

#if DEBUG   // if de compilation
            gameState.debugMagic.Draw();
#endif

            Raylib.EndDrawing();
        }

        //gameState.RemoveScene("menu");
        //gameState.RemoveScene("gameplay");
        //gameState.RemoveScene("options");
        gameState.Close();

        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }
}