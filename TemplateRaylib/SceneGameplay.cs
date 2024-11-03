using Raylib_cs;

public class SceneGameplay : Scene
{
    private float timer;
    public SceneGameplay()
    {
        GameState.Instance.debugMagic.Clear();
    }

    public override void Draw()
    {
        Raylib.DrawText("GAMEPLAY", 5, 5, 35, Color.Black);

        foreach (Entity sprite in Entity.ALL)                   // exemple Utilisation entités
        {
            sprite.Draw();
        }

        base.Draw();
    }

    public override void Update()
    {
        Entity.CleanUp();
        timer += 0.5f;

#if DEBUG   // if de compilation
        GameState.Instance.debugMagic.AddOption("FPS", Raylib.GetFPS());
        GameState.Instance.debugMagic.AddOption("Le timer", timer);
        GameState.Instance.debugMagic.AddOption("Nom", this.name);
#endif

        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            GameState.Instance.ChangeScene("menu");
        }

        foreach (Entity sprite in Entity.ALL)                   // exemple Utilisation entités
        {
            sprite.Update();
            sprite.DebugLabel = sprite.Velocity.Y.ToString();

            if (sprite.Position.Y > GameState.Instance.gameScreenHeight)
            {
                sprite.Destroy();
            }
        }

        base.Update();
    }

    public override void Show()
    {
        //Entity.ALL.Clear();                                       // exemple utilisation entités
        // Entity example = new Entity(...........);            
        // example.Velocity = new Vector2(...........);
        // ball.Debug = true;

        base.Show();
    }
}