
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

public class GameState
{
    private Scene? currentScene;
    private static GameState? instance;
    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameState();
            }
            return instance;
        }
    }

    public int gameScreenWidth { get; private set; }
    public int gameScreenHeight { get; private set; }
    public float gameScreenScale = 1;
    public float gameScreenOffsetX = 0;
    public float gameScreenOffsetY = 0;

    private Dictionary<string, Scene> scenes;   // On déclare notre dictionnaire

    public DebugMagic debugMagic = new DebugMagic();

    public float masterVolume { get; private set; }
    public bool fullScreen { get; private set; }

    OptionsFile optionsFile = new OptionsFile();

    public void SetVirtualGameResolution(int W, int H)
    {
        gameScreenWidth = W;
        gameScreenHeight = H;
    }

    public void SetVolume(float volume)
    {
        masterVolume = volume;
        Raylib.SetMasterVolume(masterVolume);
    }

    // Constructeur
    public GameState()
    {
        scenes = new Dictionary<string, Scene>();   // On créer notre dictionnaire

        OptionsFile optionsFile = new OptionsFile();
        optionsFile.Load();

        if (optionsFile.IsOptionExists("volume"))   // Vérifier si l'option existe au lieu de regarder si le volume = 0
        {
            float volume = optionsFile.GetOptionFloat("volume");
            masterVolume = volume;
        }
        else
        {
            masterVolume = 0.8f;
        }

        fullScreen = optionsFile.GetOptionBool("fullscreen");
        if (fullScreen)
        {
            Debug.WriteLine("Passe en plein écran");
            //Raylib.ToggleFullscreen();
        }
    }

    public void RegisterScene(string name, Scene scene)     // On ajoute une scene dans le dictionnaire
    {
        scenes[name] = scene;
        scene.name = name;
    }

    public void RemoveScene(string name)
    {
        if (scenes.ContainsKey(name))
        {
            currentScene = scenes[name];
            currentScene.Close();
            scenes.Remove(name);    // On retire une scene du dictionnaire
        }
    }

    public void ChangeScene(string name)
    {
        if (scenes.ContainsKey(name))
        {
            if (currentScene != null)
            {
                Debug.WriteLine($"La scene {currentScene.name} se cache");
                currentScene.Hide();
            }
            currentScene = scenes[name];    // Va cherche la scene dans le dictionnaire
            currentScene.Show();
            Debug.WriteLine("Change scene vers " + name);
        }
        else
        {
            string error = $"Scene {name} non trouvée dans le dictionnaire";
            Debug.WriteLine(error);
            throw new Exception(error);
        }
    }

    public void UpdateScene()
    {
        if (currentScene != null)
        {
            currentScene?.Update();
        }
    }

    public void DrawScene()
    {
        if (currentScene != null)
        {
            currentScene?.Draw();
        }
    }

    public void Close()
    {
        foreach (var item in scenes)
        {
            Debug.WriteLine($"La scene {item.Value.name} est détruite");
            item.Value.Close();                                                 // Préciser ".Value" pour récupérer la valeur du dictionnaire
        }
    }
}