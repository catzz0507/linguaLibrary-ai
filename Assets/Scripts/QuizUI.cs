using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [Header("Quiz Text")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] optionTexts;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI queueText;

    [Header("Option Highlight")]
    public Image[] optionImages;
    public Color normalOptionColor = Color.white;
    public Color selectedOptionColor = Color.yellow;

    [Header("HP Bar")]
    public Image playerHpFillImage;
    public Image monsterHpFillImage;

    [Header("HP Text")]
    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI monsterHpText;

    [Header("Timer Bar")]
    public RectTransform timerFillRect;
    public TextMeshProUGUI timerText;
    public Image timerFillImage;
    public Color timerSafeColor = Color.green;
    public Color timerWarningColor = Color.yellow;
    public Color timerDangerColor = Color.red;

    private float playerOriginalWidth;
    private float monsterOriginalWidth;
    private float timerOriginalWidth;

    void Start()
    {
        playerOriginalWidth =
            playerHpFillImage.rectTransform.sizeDelta.x;

        monsterOriginalWidth =
            monsterHpFillImage.rectTransform.sizeDelta.x;

        if (timerFillRect != null)
            timerOriginalWidth = timerFillRect.sizeDelta.x;

    }

    public void ShowLoading(string message)
    {
        statusText.text = message;
        questionText.text = "";

        foreach (var optionText in optionTexts)
        {
            optionText.text = "";
        }
    }

    public void ShowQuiz(QuizData quiz)
    {
        questionText.text = $"[{quiz.category}]\n{quiz.question}";

        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = $"{i + 1}. {quiz.options[i]}";
        }

        statusText.text = "Select 1, 2, 3, or 4";

        ResetOptionColors();
    }

    public void SetTimer(float currentTime, float maxTime)
    {
        float ratio = Mathf.Clamp01(currentTime / maxTime);

        if (timerText != null)
            timerText.text = $"{Mathf.CeilToInt(currentTime)}";

        if (timerFillRect != null)
        {
            Vector2 size = timerFillRect.sizeDelta;
            size.x = timerOriginalWidth * ratio;
            timerFillRect.sizeDelta = size;
        }

        if (timerFillImage != null)
        {
            if (ratio < 0.25f)
                timerFillImage.color = timerDangerColor;
            else if (ratio < 0.5f)
                timerFillImage.color = timerWarningColor;
            else
                timerFillImage.color = timerSafeColor;
        }
    }
    public void SetQueueCount(int count)
    {
        if (queueText != null)
            queueText.text = $"Quiz Queue: {count}";
    }

    public void ShowCorrect()
    {
        statusText.text = "<color=green>CORRECT!</color>";
    }

    public void ShowWrong()
    {
        statusText.text = "<color=red>WRONG!</color>";
    }

    public void ShowTimeOver()
    {
        statusText.text = "<color=red>TIME OVER!</color>";
    }

    public void ShowBattleEnd(bool playerWin)
    {
        statusText.text = playerWin
            ? "<color=green>YOU WIN!</color>"
            : "<color=red>YOU LOSE...</color>";
    }

    public void UpdateHp(CharacterStatus player, CharacterStatus monster)
    {
        UpdateHpBar(
            playerHpFillImage.rectTransform,
            playerOriginalWidth,
            (float)player.currentHp / player.maxHp
        );

        UpdateHpBar(
            monsterHpFillImage.rectTransform,
            monsterOriginalWidth,
            (float)monster.currentHp / monster.maxHp
        );

        if (playerHpText != null)
        {
            playerHpText.text =
                $"{player.currentHp} / {player.maxHp}";
        }

        if (monsterHpText != null)
        {
            monsterHpText.text =
                $"{monster.currentHp} / {monster.maxHp}";
        }
    }

    public void ResetOptionColors()
    {
        if (optionImages == null) return;

        for (int i = 0; i < optionImages.Length; i++)
        {
            if (optionImages[i] != null)
                optionImages[i].color = normalOptionColor;
        }
    }

    public void HighlightSelectedOption(int index)
    {
        ResetOptionColors();

        if (optionImages == null) return;
        if (index < 0 || index >= optionImages.Length) return;
        if (optionImages[index] == null) return;

        optionImages[index].color = selectedOptionColor;
    }

    private void UpdateHpBar(
        RectTransform rectTransform,
        float originalWidth,
        float ratio)
    {
        ratio = Mathf.Clamp01(ratio);

        Vector2 size = rectTransform.sizeDelta;
        size.x = originalWidth * ratio;

        rectTransform.sizeDelta = size;
    }
}