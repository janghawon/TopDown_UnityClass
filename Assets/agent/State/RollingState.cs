using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class RollingState : CommonState
{
    [SerializeField] private float _rollingSpeed = 0.4f;
    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += RollingEndHandle;
        _agentMovement.isActiveMove = false;
        //�Ѹ��� ���콺�� ���� Ű����� ���� �����

        _agentMovement.StopImmediately();
        _agentMovement.SetMovementVelocity(transform.forward * _rollingSpeed);
        _agentAnimator.SetRollingState(true);
    }

    public override void OnExitState()
    {
        _agentAnimator.OnAnimationEndTrigger -= RollingEndHandle;
        _agentAnimator.SetRollingState(false);
        _agentMovement.isActiveMove = true;
    }
    private void RollingEndHandle()
    {
        _agentMovement.StopImmediately();
        _agentController.ChangeState(StateType.Normal);
    }
    public override void UpdateState()
    {

    }
}
