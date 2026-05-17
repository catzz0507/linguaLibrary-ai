using UnityEngine;

public class BattleCharacterAnimator : MonoBehaviour
{
    public Animator animator;

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int PrepareHash = Animator.StringToHash("Prepare");
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int HitHash = Animator.StringToHash("Hit");
    private readonly int DeadHash = Animator.StringToHash("Dead");

    public void PlayIdle()
    {
        PlayState(IdleHash);
    }

    public void PlayPrepare()
    {
        PlayState(PrepareHash);
    }

    public void PlayAttack()
    {
        PlayState(AttackHash);
    }

    public void PlayHit()
    {
        PlayState(HitHash);
    }

    public void PlayDead()
    {
        PlayState(DeadHash);
    }

    private void PlayState(int stateHash)
    {
        if (animator == null)
            return;

        animator.Play(stateHash, 0, 0f);
        animator.Update(0f);
    }
}