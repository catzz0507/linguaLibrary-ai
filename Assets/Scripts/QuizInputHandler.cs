using System;
using UnityEngine;

public class QuizInputHandler : MonoBehaviour
{
    public bool inputEnabled = false;
    public Action<int> OnAnswerSelected;

    void Update()
    {
        if (!inputEnabled) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            OnAnswerSelected?.Invoke(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            OnAnswerSelected?.Invoke(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            OnAnswerSelected?.Invoke(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            OnAnswerSelected?.Invoke(3);
    }
}