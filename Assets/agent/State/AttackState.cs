using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : CommonState, IState
{
    [SerializeField] private float _keyDelay = 0.5f;
    private int _currentCombo = 0;
    private bool _canAttack = true;
    private float _keyTimer = 0;

    public override void OnEnterState()
    {
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentAnimator.OnAnimatonEndTrigger += OnAnimationEndHanddle;
        _currentCombo = 0;
        _canAttack = true;
        _agentAnimator.SetAttackState(true);
        OnAttackHandle();
    }

    

    public override void OnExitState()
    {
        _agentInput.OnAttackKeyPress -= OnAttackHandle;
        _agentAnimator.OnAnimatonEndTrigger -= OnAnimationEndHanddle;
        _agentAnimator.SetAttackState(false);
        _agentAnimator.SetAttackTrigger(false);
    }
    private void OnAnimationEndHanddle()
    {
        _canAttack = true;
    }
    private void OnAttackHandle()
    {
        if(_canAttack && _currentCombo < 3)
        {
            _agentAnimator.SetAttackTrigger(true);
            _currentCombo++;
        }
    }
    public override void UpdateState()
    {
        if(_canAttack && _keyTimer > 0)
        {
            _keyTimer -= Time.deltaTime;
            if(_keyTimer <= 0)
            {
                _agentController.ChangeState(Core.StateType.Normal);
            }
        }
    }
}
