[System.Serializable]
public class QuizData
{
    public string category;
    public string question;
    public string[] options;
    public int answerIndex;
}

[System.Serializable]
public class QuizBatchData
{
    public QuizData[] quizzes;
}

[System.Serializable]
public class ChatResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}