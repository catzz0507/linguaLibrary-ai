using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [Header("Scene Names")]
    public string problemSelectSceneName = "ProblemSelectScene";

    public void OnClickStart()
    {
        SceneManager.LoadScene(problemSelectSceneName);
    }

    public void OnClickSetting()
    {
        Debug.Log("Setting button clicked.");
    }

    public void OnClickExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}