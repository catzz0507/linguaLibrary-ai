using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProblemSelectController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject languagePanel;
    public GameObject difficultyPanel;
    public GameObject loadingPanel;

    [Header("Language Buttons")]
    public Button japaneseButton;
    public Button englishButton;
    public Button koreanButton;
    public Button chineseButton;

    [Header("Scene")]
    public string inGameSceneName = "InGameScene";

    private bool isLoading = false;

    private void Start()
    {
        languagePanel.SetActive(true);
        difficultyPanel.SetActive(false);
        loadingPanel.SetActive(false);

        SetUnavailableButton(englishButton);
        SetUnavailableButton(koreanButton);
        SetUnavailableButton(chineseButton);
    }

    public void OnClickJapanese()
    {
        if (isLoading) return;

        GameSession.Instance.SetLanguage("Japanese");

        languagePanel.SetActive(false);
        difficultyPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    public void OnClickJlptN5() => StartCoroutine(SelectDifficultyAndLoad("N5"));
    public void OnClickJlptN4() => StartCoroutine(SelectDifficultyAndLoad("N4"));
    public void OnClickJlptN3() => StartCoroutine(SelectDifficultyAndLoad("N3"));
    public void OnClickJlptN2() => StartCoroutine(SelectDifficultyAndLoad("N2"));
    public void OnClickJlptN1() => StartCoroutine(SelectDifficultyAndLoad("N1"));

    private IEnumerator SelectDifficultyAndLoad(string jlptLevel)
    {
        if (isLoading) yield break;

        isLoading = true;

        GameSession.Instance.SetJlptLevel(jlptLevel);

        languagePanel.SetActive(false);
        difficultyPanel.SetActive(false);
        loadingPanel.SetActive(true);

        yield return StartCoroutine(
            AppRoot.Instance.quizProvider.Initialize(jlptLevel)
        );

        SceneManager.LoadScene(inGameSceneName);
    }

    private void SetUnavailableButton(Button button)
    {
        if (button == null) return;

        button.interactable = false;

        Image image = button.GetComponent<Image>();
        if (image != null)
            image.color = new Color(0.35f, 0.35f, 0.35f, 0.7f);
    }
}