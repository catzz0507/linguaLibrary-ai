using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizProvider : MonoBehaviour
{
    [Header("References")]
    public JLPTPromptBuilder promptBuilder;
    public LMStudioClient lmStudioClient;

    [Header("Quiz Queue Settings")]
    public int initialBatchSize = 15;
    public int minimumStartQuizCount = 10;
    public int refillBatchSize = 5;
    public int refillThreshold = 10;
    public int maxQueueSize = 25;

    private Queue<QuizData> quizQueue = new Queue<QuizData>();
    private bool isGenerating = false;

    private AppConfig config;
    private List<QuizData> sampleQuestions = new List<QuizData>();
    private int sampleIndex = 0;

    public bool IsGenerating => isGenerating;
    public int QueueCount => quizQueue.Count;

    private void Awake()
    {
        config = AppConfig.Load();
        sampleQuestions = SampleQuestionLoader.LoadSampleQuestions();

        Debug.Log($"[QuizProvider] Demo Mode: {config.useDemoMode}");
        Debug.Log($"[QuizProvider] Fallback Enabled: {config.fallbackToSampleQuestions}");
        Debug.Log($"[QuizProvider] Sample Question Count: {sampleQuestions.Count}");
    }

    public bool HasEnoughQuizzesToStart()
    {
        return quizQueue.Count >= minimumStartQuizCount;
    }

    public IEnumerator Initialize(string jlptLevel)
    {
        quizQueue.Clear();

        if (config.useDemoMode)
        {
            Debug.Log("[QuizProvider] Demo Mode enabled. Loading sample questions.");
            EnqueueSampleQuizzes(initialBatchSize);
            TryStartBackgroundRefill(jlptLevel, true);
            yield break;
        }

        yield return StartCoroutine(GenerateAndEnqueueOnce(jlptLevel, initialBatchSize));

        if (quizQueue.Count < minimumStartQuizCount)
        {
            Debug.LogWarning($"Not enough valid initial quizzes: {quizQueue.Count}/{minimumStartQuizCount}");

            while (quizQueue.Count < minimumStartQuizCount)
            {
                int previousCount = quizQueue.Count;
                int needCount = minimumStartQuizCount - quizQueue.Count;

                yield return StartCoroutine(GenerateAndEnqueueOnce(jlptLevel, needCount));

                if (quizQueue.Count == previousCount)
                {
                    Debug.LogWarning("No additional valid quizzes were generated.");

                    if (config.fallbackToSampleQuestions)
                    {
                        Debug.LogWarning("[QuizProvider] Falling back to sample questions during initialization.");
                        EnqueueSampleQuizzes(needCount);
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (sampleQuestions.Count == 0 && config.fallbackToSampleQuestions && quizQueue.Count == previousCount)
                {
                    Debug.LogError("[QuizProvider] Cannot continue. No AI quizzes and no sample questions available.");
                    break;
                }
            }
        }

        TryStartBackgroundRefill(jlptLevel, true);
    }

    public bool TryGetNextQuiz(string jlptLevel, out QuizData quiz)
    {
        quiz = null;

        if (quizQueue.Count <= refillThreshold)
        {
            TryStartBackgroundRefill(jlptLevel);
        }

        if (quizQueue.Count == 0)
        {
            if (config.useDemoMode || config.fallbackToSampleQuestions)
            {
                EnqueueSampleQuizzes(1);
            }
        }

        if (quizQueue.Count == 0)
        {
            return false;
        }

        quiz = quizQueue.Dequeue();

        if (quizQueue.Count <= refillThreshold)
        {
            TryStartBackgroundRefill(jlptLevel);
        }

        return true;
    }

    public void TryStartBackgroundRefill(string jlptLevel, bool ignoreThreshold = false)
    {
        if (isGenerating) return;
        if (quizQueue.Count >= maxQueueSize) return;

        if (!ignoreThreshold && quizQueue.Count > refillThreshold)
            return;

        if (config.useDemoMode)
        {
            EnqueueSampleQuizzes(refillBatchSize);
            return;
        }

        StartCoroutine(GenerateAndEnqueue(jlptLevel, refillBatchSize));
    }

    private IEnumerator GenerateAndEnqueue(string jlptLevel, int count)
    {
        isGenerating = true;

        int targetAddedCount = count;
        int totalAddedCount = 0;
        int retryCount = 0;
        int maxRetryCount = 3;

        while (totalAddedCount < targetAddedCount && retryCount < maxRetryCount)
        {
            int remainingCount = targetAddedCount - totalAddedCount;

            string systemPrompt = promptBuilder.BuildSystemPrompt();
            string userPrompt = promptBuilder.BuildUserPrompt(jlptLevel, remainingCount);

            bool completed = false;
            string rawResponse = null;
            string errorMessage = null;

            yield return StartCoroutine(
                lmStudioClient.RequestQuizBatch(
                    systemPrompt,
                    userPrompt,
                    response =>
                    {
                        rawResponse = response;
                        completed = true;
                    },
                    error =>
                    {
                        errorMessage = error;
                        completed = true;
                    }
                )
            );

            if (!completed)
            {
                Debug.LogWarning("Quiz generation request did not complete.");
                retryCount++;
                continue;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Debug.LogError($"LM Studio Error: {errorMessage}");
                retryCount++;
                continue;
            }

            int addedCount = AddParsedQuizzes(rawResponse);
            totalAddedCount += addedCount;

            if (addedCount == 0)
            {
                retryCount++;
                Debug.LogWarning($"No valid quizzes were added. Retrying... Retry: {retryCount}/{maxRetryCount}");
            }
            else
            {
                Debug.Log($"Added {addedCount} valid quizzes. Total added: {totalAddedCount}/{targetAddedCount}");
            }

            if (quizQueue.Count >= maxQueueSize)
                break;
        }

        if (totalAddedCount < targetAddedCount)
        {
            Debug.LogWarning($"Generated fewer quizzes than requested: {totalAddedCount}/{targetAddedCount}");

            if (config.fallbackToSampleQuestions)
            {
                int fallbackCount = targetAddedCount - totalAddedCount;
                Debug.LogWarning($"[QuizProvider] Falling back to {fallbackCount} sample quizzes.");
                int sampleAdded = EnqueueSampleQuizzes(fallbackCount);
                totalAddedCount += sampleAdded;
            }
        }

        isGenerating = false;
    }

    private IEnumerator GenerateAndEnqueueOnce(string jlptLevel, int count)
    {
        isGenerating = true;

        string systemPrompt = promptBuilder.BuildSystemPrompt();
        string userPrompt = promptBuilder.BuildUserPrompt(jlptLevel, count);

        string rawResponse = null;
        string errorMessage = null;

        yield return StartCoroutine(
            lmStudioClient.RequestQuizBatch(
                systemPrompt,
                userPrompt,
                response =>
                {
                    rawResponse = response;
                },
                error =>
                {
                    errorMessage = error;
                }
            )
        );

        if (!string.IsNullOrEmpty(errorMessage))
        {
            Debug.LogError($"LM Studio Error: {errorMessage}");

            if (config.fallbackToSampleQuestions)
            {
                Debug.LogWarning($"[QuizProvider] Falling back to {count} sample quizzes.");
                EnqueueSampleQuizzes(count);
            }
        }
        else
        {
            int addedCount = AddParsedQuizzes(rawResponse);
            Debug.Log($"Single generation request added {addedCount} valid quizzes. Current queue: {quizQueue.Count}");

            if (addedCount == 0 && config.fallbackToSampleQuestions)
            {
                Debug.LogWarning($"[QuizProvider] Parsed 0 valid quizzes. Falling back to {count} sample quizzes.");
                EnqueueSampleQuizzes(count);
            }
        }

        isGenerating = false;
    }

    private int AddParsedQuizzes(string rawResponse)
    {
        int addedCount = 0;

        bool parsed = QuizParser.TryParseQuizBatch(rawResponse, out QuizBatchData batchData);

        if (!parsed)
        {
            Debug.LogWarning("Failed to parse quiz batch.");
            return 0;
        }

        foreach (QuizData quiz in batchData.quizzes)
        {
            if (quizQueue.Count >= maxQueueSize)
                break;

            if (QuizParser.IsValidQuiz(quiz))
            {
                quizQueue.Enqueue(quiz);
                addedCount++;
            }
            else
            {
                Debug.LogWarning("Invalid quiz discarded.");
            }
        }

        Debug.Log($"Current quiz queue count: {quizQueue.Count}");
        return addedCount;
    }

    private int EnqueueSampleQuizzes(int count)
    {
        if (sampleQuestions == null || sampleQuestions.Count == 0)
        {
            Debug.LogError("[QuizProvider] No sample questions available.");
            return 0;
        }

        int addedCount = 0;

        for (int i = 0; i < count; i++)
        {
            if (quizQueue.Count >= maxQueueSize)
                break;

            QuizData quiz = GetNextSampleQuestion();

            if (quiz != null && QuizParser.IsValidQuiz(quiz))
            {
                quizQueue.Enqueue(quiz);
                addedCount++;
            }
            else
            {
                Debug.LogWarning("[QuizProvider] Invalid sample quiz discarded.");
            }
        }

        Debug.Log($"[QuizProvider] Added {addedCount} sample quizzes. Current queue: {quizQueue.Count}");
        return addedCount;
    }

    private QuizData GetNextSampleQuestion()
    {
        if (sampleQuestions == null || sampleQuestions.Count == 0)
        {
            return null;
        }

        QuizData quiz = sampleQuestions[sampleIndex];
        sampleIndex = (sampleIndex + 1) % sampleQuestions.Count;

        return quiz;
    }
}