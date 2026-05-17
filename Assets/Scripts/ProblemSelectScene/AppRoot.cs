using UnityEngine;

public class AppRoot : MonoBehaviour
{
    public static AppRoot Instance { get; private set; }

    [Header("Core")]
    public GameSession gameSession;

    [Header("Quiz System")]
    public QuizProvider quizProvider;
    public JLPTPromptBuilder promptBuilder;
    public LMStudioClient lmStudioClient;

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
}