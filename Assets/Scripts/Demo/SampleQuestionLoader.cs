using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SampleQuizList
{
    public QuizData[] questions;
}

public static class SampleQuestionLoader
{
    public static List<QuizData> LoadSampleQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "sample_questions.json");

        if (!File.Exists(path))
        {
            Debug.LogError("[SampleQuestionLoader] sample_questions.json not found: " + path);
            return new List<QuizData>();
        }

        try
        {
            string rawJson = File.ReadAllText(path);

            // JsonUtility cannot parse a raw JSON array directly.
            string wrappedJson = "{\"questions\":" + rawJson + "}";

            SampleQuizList list = JsonUtility.FromJson<SampleQuizList>(wrappedJson);

            if (list == null || list.questions == null)
            {
                Debug.LogError("[SampleQuestionLoader] Failed to parse sample questions.");
                return new List<QuizData>();
            }

            Debug.Log("[SampleQuestionLoader] Loaded sample questions: " + list.questions.Length);
            return new List<QuizData>(list.questions);
        }
        catch (Exception e)
        {
            Debug.LogError("[SampleQuestionLoader] Error: " + e.Message);
            return new List<QuizData>();
        }
    }
}