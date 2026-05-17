using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("JLPT")]
    public string jlptLevel = "N5";

    [Header("References")]

    public QuizInputHandler inputHandler;
    public QuizUI quizUI;

    [Header("Characters")]
    public CharacterStatus playerStatus;
    public CharacterStatus monsterStatus;
    public BattleCharacterAnimator playerAnimator;
    public BattleCharacterAnimator monsterAnimator;

    [Header("Effects")]
    public BattleEffectController effectController;

    [Header("Battle Settings")]
    public float questionTimeLimit = 10f;
    public float prepareDuration = 0.5f;
    public float attackDuration = 0.5f;
    public float hitDuration = 0.5f;
    public float nextQuestionDelay = 1f;

    public int playerDamage = 20;
    public int monsterDamage = 15;

    private QuizProvider quizProvider;
    private QuizData currentQuiz;
    private bool answerSelected = false;
    private int selectedAnswerIndex = -1;
    private bool battleEnded = false;


    private void Awake()
    {
        if (quizProvider == null)
        {
            quizProvider = AppRoot.Instance.quizProvider;
        }

        if (GameSession.Instance != null)
        {
            jlptLevel = GameSession.Instance.selectedJlptLevel;
        }
    }

    void Start()
    {
        inputHandler.OnAnswerSelected += OnAnswerSelected;
        StartCoroutine(BattleRoutine());
    }

    void OnDestroy()
    {
        inputHandler.OnAnswerSelected -= OnAnswerSelected;
    }

    private IEnumerator BattleRoutine()
    {
        battleEnded = false;
        inputHandler.inputEnabled = false;

        quizUI.UpdateHp(playerStatus, monsterStatus);

        if (!quizProvider.HasEnoughQuizzesToStart())
        {
            quizUI.ShowLoading("Quiz loading error.");
            yield break;
        }

        quizProvider.TryStartBackgroundRefill(jlptLevel, true);

        while (!battleEnded)
        {
            yield return StartCoroutine(QuestionTurnRoutine());

            if (CheckBattleEnd())
                yield break;

            yield return new WaitForSeconds(nextQuestionDelay);
        }
    }

    private IEnumerator QuestionTurnRoutine()
    {
        inputHandler.inputEnabled = false;
        answerSelected = false;
        selectedAnswerIndex = -1;

        while (!quizProvider.TryGetNextQuiz(jlptLevel, out currentQuiz))
        {
            quizUI.ShowLoading("Preparing next quiz...");
            quizUI.SetQueueCount(quizProvider.QueueCount);
            yield return null;
        }

        quizUI.ShowQuiz(currentQuiz);
        quizUI.SetQueueCount(quizProvider.QueueCount);

        float timer = questionTimeLimit;
        inputHandler.inputEnabled = true;

        while (timer > 0f && !answerSelected)
        {
            timer -= Time.deltaTime;
            quizUI.SetTimer(timer, questionTimeLimit);
            quizUI.SetQueueCount(quizProvider.QueueCount);
            yield return null;
        }

        inputHandler.inputEnabled = false;

        if (!answerSelected)
        {
            quizUI.ShowTimeOver();
            yield return StartCoroutine(ResolveWrongAnswerRoutine());
            yield break;
        }

        bool isCorrect = selectedAnswerIndex == currentQuiz.answerIndex;

        if (isCorrect)
        {
            quizUI.ShowCorrect();
            yield return StartCoroutine(ResolveCorrectAnswerRoutine());
        }
        else
        {
            quizUI.ShowWrong();
            yield return StartCoroutine(ResolveWrongAnswerRoutine());
        }
    }

    private IEnumerator ResolveCorrectAnswerRoutine()
    {
        playerAnimator.PlayPrepare();
        monsterAnimator.PlayIdle();

        yield return new WaitForSeconds(prepareDuration);

        playerAnimator.PlayAttack();
        monsterAnimator.PlayHit();

        monsterStatus.TakeDamage(playerDamage);
        quizUI.UpdateHp(playerStatus, monsterStatus);

        if (effectController != null)
        {
            effectController.ShowHit();
            effectController.ShowDamage(playerDamage);
        }

        yield return new WaitForSeconds(attackDuration);

        if (monsterStatus.IsDead)
        {
            monsterAnimator.PlayDead();   
            playerAnimator.PlayIdle();
        }
        else
        {
            playerAnimator.PlayIdle();
            monsterAnimator.PlayIdle();
        }
    }

    private IEnumerator ResolveWrongAnswerRoutine()
    {
        playerAnimator.PlayPrepare();
        monsterAnimator.PlayIdle();

        yield return new WaitForSeconds(prepareDuration);

        playerAnimator.PlayAttack();
        monsterAnimator.PlayIdle();

        if (effectController != null)
            effectController.ShowMiss();

        yield return new WaitForSeconds(attackDuration);

        monsterAnimator.PlayAttack();
        playerAnimator.PlayHit();

        playerStatus.TakeDamage(monsterDamage);
        quizUI.UpdateHp(playerStatus, monsterStatus);

        if (effectController != null)
            effectController.ShowDamage(monsterDamage);

        yield return new WaitForSeconds(hitDuration);

        if (playerStatus.IsDead)
        {
            playerAnimator.PlayDead();   
            monsterAnimator.PlayIdle();
        }
        else
        {
            playerAnimator.PlayIdle();
            monsterAnimator.PlayIdle();
        }
    }

    private bool CheckBattleEnd()
    {
        if (monsterStatus.IsDead)
        {
            battleEnded = true;
            inputHandler.inputEnabled = false;

            monsterAnimator.PlayDead();
            playerAnimator.PlayIdle();

            quizUI.ShowBattleEnd(true);
            return true;
        }

        if (playerStatus.IsDead)
        {
            battleEnded = true;
            inputHandler.inputEnabled = false;

            playerAnimator.PlayDead();
            monsterAnimator.PlayIdle();

            quizUI.ShowBattleEnd(false);
            return true;
        }

        return false;
    }

    private void OnAnswerSelected(int index)
    {
        if (answerSelected) return;

        answerSelected = true;
        selectedAnswerIndex = index;

        quizUI.HighlightSelectedOption(index);
    }
}