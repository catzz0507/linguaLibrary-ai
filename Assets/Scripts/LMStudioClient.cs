using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LMStudioClient : MonoBehaviour
{
    [Header("Local AI Server Settings")]
    [SerializeField] private string apiUrl = "http://127.0.0.1:1234/v1/chat/completions";
    [SerializeField] private string modelName = "gemma-4-e4b";
    [SerializeField, Range(0f, 1f)] private float temperature = 0.3f;
    [SerializeField] private int timeoutSeconds = 10;

    private void Awake()
    {
        AppConfig config = AppConfig.Load();

        apiUrl = config.apiUrl;
        modelName = config.model;
        timeoutSeconds = config.timeoutSeconds;

        Debug.Log("[LMStudioClient] Loaded local inference config.");
        Debug.Log("[LMStudioClient] API URL: " + apiUrl);
        Debug.Log("[LMStudioClient] Model: " + modelName);
    }

    public IEnumerator RequestQuizBatch(
        string systemPrompt,
        string userPrompt,
        Action<string> onSuccess,
        Action<string> onError)
    {
        string jsonPayload = $@"{{
            ""model"": ""{modelName}"",
            ""messages"": [
                {{ ""role"": ""system"", ""content"": ""{EscapeJson(systemPrompt)}"" }},
                {{ ""role"": ""user"", ""content"": ""{EscapeJson(userPrompt)}"" }}
            ],
            ""temperature"": {temperature}
        }}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = timeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    private string EscapeJson(string text)
    {
        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "");
    }
}