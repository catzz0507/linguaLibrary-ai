using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp = 100;

    public bool IsDead => currentHp <= 0;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp < 0)
            currentHp = 0;
    }

    public void HealFull()
    {
        currentHp = maxHp;
    }
}