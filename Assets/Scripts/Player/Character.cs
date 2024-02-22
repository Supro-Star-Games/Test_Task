using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int _characterMaxHp;
    [SerializeField] private int _characterDamage;

    protected int maxHP => _characterMaxHp;
    protected int currentHP;

    public event Action<int, float> HpChanged;

    public event Action PlayerWon;

    public virtual void Initialize()
    {
        currentHP = maxHP;
        SaveManager.Instance.EnemyHp = currentHP;
        HpChange(currentHP, (float)currentHP / maxHP);
    }

    public virtual void LoadData(int value)
    {
        currentHP = value;
        HpChange(currentHP, (float)currentHP / maxHP);
    }

    protected void HpChange(int hpValue, float fillValue)
    {
        HpChanged?.Invoke(hpValue, fillValue);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
        {
            currentHP = 0;
            PlayerWon?.Invoke();
        }

        SaveManager.Instance.EnemyHp = currentHP;
        HpChange(currentHP, (float)currentHP / maxHP);
    }

    public int GetCharacterDamage()
    {
        return _characterDamage;
    }
}