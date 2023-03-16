using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentAnimator : MonoBehaviour
{
    private readonly int _speedHash = Animator.StringToHash("speed");
    private readonly int _isAirboneHash = Animator.StringToHash("is_airbone");

    private readonly int _attackHash = Animator.StringToHash("attack");
    private readonly int _isAttackhash = Animator.StringToHash("is_attack");

    private readonly int _isRollingHash = Animator.StringToHash("is_rolling");

    public event Action OnAnimationEndTrigger = null;

    private Animator _animator;
    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetRollingState(bool stae)
    {
        _animator.SetBool(_isRollingHash, stae);
    }
    public void SetAttackState(bool state)
    {
        _animator.SetBool(_isAttackhash, state);
    }
    public void SetAttackTrigger(bool value)
    {
        if(value)
        {
            _animator.SetTrigger(_attackHash);
        }
        else
        {
            _animator.ResetTrigger(_attackHash);
        }
    }
    public void SetSpeed(float value)
    {
        _animator.SetFloat(_speedHash, value);
    }

    public void SetAirbone(bool value)
    {
        _animator.SetBool(_isAirboneHash, value);
    }

    public void OnAnimationEnd()
    {
        OnAnimationEndTrigger?.Invoke();
    }
}
