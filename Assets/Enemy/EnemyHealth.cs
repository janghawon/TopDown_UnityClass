using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitTriggered = null;
    private AIActionData _aIActionData;
    public UnityEvent OnDeadTriggered = null;

    public Action<int, int> OnHealthChanged = null;
    [SerializeField] private bool _isDead = false;

    public bool IsDead { get; set; }

    private int _maxHP;
    private int _currentHP;

    private void Awake()
    {
        _aIActionData = transform.Find("AI").GetComponent<AIActionData>();
    }

    public void SetMaxHP(int value)
    {
        _currentHP = _maxHP = value;
        IsDead = false;
    }

    public void OnDamage(int damage, Vector3 point, Vector3 normal)
    {
        if (IsDead) return;

        _aIActionData.HitPoint = point;
        _aIActionData.HitNormal = normal;

        _currentHP -= damage;
        if(_currentHP <= 0)
        {
            IsDead = true;
            OnDeadTriggered?.Invoke();
        }

        OnHitTriggered?.Invoke();
        OnHealthChanged?.Invoke(_currentHP, _maxHP);
    }

    
}
