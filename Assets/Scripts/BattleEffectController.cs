using System.Collections;
using TMPro;
using UnityEngine;

public class BattleEffectController : MonoBehaviour
{
    public GameObject missEffect;
    public GameObject hitEffect;
    public TextMeshProUGUI damageText;

    public void ShowMiss()
    {
        if (missEffect != null)
            StartCoroutine(ShowEffectRoutine(missEffect, 0.5f));
    }

    public void ShowHit()
    {
        if (hitEffect != null)
            StartCoroutine(ShowEffectRoutine(hitEffect, 0.5f));
    }

    public void ShowDamage(int damage)
    {
        if (damageText == null) return;

        damageText.text = $"-{damage}";
        StartCoroutine(ClearDamageTextRoutine());
    }

    private IEnumerator ShowEffectRoutine(GameObject effect, float duration)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(duration);
        effect.SetActive(false);
    }

    private IEnumerator ClearDamageTextRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        damageText.text = "";
    }
}