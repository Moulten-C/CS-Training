using Raylib_cs;
using System.Diagnostics; // Permet d'utiliser des outils de débogage
using System.IO; // Permet de manipuler des fichiers et répertoires
using System.Text.Json;

public class OptionsFile
{
    const string FILENAME = "options.json";

    private string AppName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName); // Récupère le nom de l'application sans extension, via System.IO

    private string fullPath;

    protected Dictionary<string, string> options;

    public OptionsFile()
    {
        options = new Dictionary<string, string>();
        fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName, FILENAME); // Définit le chemin complet où les options seront sauvegardées
        Debug.WriteLine($"Nom du fichier d'options : {fullPath}");
    }

    public void Clear()
    {
        options.Clear();
    }

    public void AddOption(string key, string value) // Ajoute ou met à jour une option sous forme de chaîne
    {
        options[key] = value;
    }

    public void AddOption(string key, float value) // Ajoute ou met à jour une option de type float en la convertissant en string (Surcharge)
    {
        options[key] = value.ToString();
    }

    public void AddOption(string key, int value) // Ajoute ou met à jour une option de type int en la convertissant en string (Surcharge)
    {
        options[key] = value.ToString();
    }

    public void AddOption(string key, object value) // Ajoute ou met à jour une option de tout type d'objet (Surcharge)
    {
        string json = JsonSerializer.Serialize(value); // Sérialise l'objet en JSON
        options[key] = json;
    }

    public bool IsOptionExists(string key) // Vérifie si une option existe dans le dictionnaire
    {
        return options.ContainsKey(key);
    }

    public string GetOptionString(string key) // Récupère la valeur de l'option sous forme de chaîne    (pas de surcharge possible car même type de paramètres)
    {
        if (options.ContainsKey(key))
        {
            return options[key];
        }
        else
            return ""; // Retourne une chaîne vide si la clé n'existe pas
    }

    public int GetOptionInt(string key) // Récupère la valeur de l'option sous forme d'entier
    {
        if (options.ContainsKey(key))
        {
            try   // Pour eviter les plantages avec le Parse.
            {
                return int.Parse(GetOptionString(key)); // Tente de convertir en int
            }
            catch
            {
                return 0; // En cas d'erreur de conversion, retourne 0
            }
        }
        else
            return 0; // Si la clé n'existe pas, retourne 0
    }

    public float GetOptionFloat(string key) // Récupère la valeur de l'option sous forme de float
    {
        if (options.ContainsKey(key))
        {
            try
            {
                return float.Parse(GetOptionString(key)); // Tente de convertir en float
            }
            catch
            {
                return 0f; // En cas d'erreur de conversion, retourne 0
            }
        }
        else
            return 0f; // Si la clé n'existe pas, retourne 0f
    }

    public bool GetOptionBool(string key) // Récupère la valeur de l'option sous forme de booléen
    {
        if (options.ContainsKey(key))
        {
            try
            {
                return bool.Parse(GetOptionString(key)); // Tente de convertir en booléen
            }
            catch
            {
                return false; // En cas d'erreur de conversion, retourne false
            }
        }
        else
            return false; // Si la clé n'existe pas, retourne false
    }

    public void Load() // Charge les options depuis le fichier JSON
    {
        if (File.Exists(fullPath))
        {
            var jsonString = File.ReadAllText(fullPath); // Lit tout le contenu du fichier JSON
            options = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString)!; // Désérialise le JSON en dictionnaire
            if (options == null) // Vérifie si le dictionnaire est nul (fichier corrompu)
            {
                throw new JsonException($"Fichier d'options invalide {fullPath}");
            }
        }
        else
        {
            options.Clear(); // Si le fichier n'existe pas, on vide le dictionnaire
        }
    }

    public void Save() // Sauvegarde les options dans le fichier JSON
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!); // Crée le répertoire s'il n'existe pas
        var jsonString = JsonSerializer.Serialize(options); // Sérialise le dictionnaire en JSON
        Debug.WriteLine($"JSON : {jsonString}"); // Affiche le JSON pour débogage
        File.WriteAllText(fullPath, jsonString); // Écrit le JSON dans le fichier
    }
}
