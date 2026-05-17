using System;
using System.IO;
using UnityEngine;

[Serializable]
public class AppConfig
{
    public string apiUrl = "http://127.0.0.1:1234/v1/chat/completions";
    public string model = "gemma-4-e4b";
    public bool useDemoMode = false;
    public bool fallbackToSampleQuestions = true;
    public int timeoutSeconds = 20;

    public static AppConfig Load()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("[AppConfig] config.json not found. Using default config.");
            return new AppConfig();
        }

        try
        {
            string json = File.ReadAllText(path);
            AppConfig config = JsonUtility.FromJson<AppConfig>(json);

            if (config == null)
            {
                Debug.LogWarning("[AppConfig] Failed to parse config.json. Using default config.");
                return new AppConfig();
            }

            return config;
        }
        catch (Exception e)
        {
            Debug.LogWarning("[AppConfig] Error loading config.json: " + e.Message);
            return new AppConfig();
        }
    }
}