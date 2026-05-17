using System.Collections.Generic;
using UnityEngine;

public static class QuizParser
{
    public static bool TryParseQuizBatch(string rawResponse, out QuizBatchData batchData)
    {
        batchData = null;

        try
        {
            ChatResponse response = JsonUtility.FromJson<ChatResponse>(rawResponse);

            if (response == null || response.choices == null || response.choices.Length == 0)
                return false;

            string content = response.choices[0].message.content;
            content = content.Replace("```json", "").Replace("```", "").Trim();

            int startIndex = content.IndexOf('{');
            int endIndex = content.LastIndexOf('}');

            if (startIndex == -1 || endIndex == -1)
                return false;

            string pureJson = content.Substring(startIndex, endIndex - startIndex + 1);
            batchData = JsonUtility.FromJson<QuizBatchData>(pureJson);

            return batchData != null && batchData.quizzes != null;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidQuiz(QuizData quiz)
    {
        if (quiz == null) return false;
        if (string.IsNullOrEmpty(quiz.category)) return false;
        if (string.IsNullOrEmpty(quiz.question)) return false;
        if (quiz.options == null) return false;
        if (quiz.options.Length != 4) return false;
        if (quiz.answerIndex < 0 || quiz.answerIndex > 3) return false;

        HashSet<string> optionSet = new HashSet<string>();

        foreach (string option in quiz.options)
        {
            if (string.IsNullOrWhiteSpace(option)) return false;
            if (optionSet.Contains(option)) return false;

            optionSet.Add(option);
        }

        return true;
    }
}