using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public string selectedLanguage = "Japanese";
    public string selectedJlptLevel = "N5";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLanguage(string language)
    {
        selectedLanguage = language;
    }

    public void SetJlptLevel(string level)
    {
        selectedJlptLevel = level;
    }
}