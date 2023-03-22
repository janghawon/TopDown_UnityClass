using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class RollingState : CommonState
{
    [SerializeField] private float _rollingSpeed = 0.4f ,_animationThreshold = 0.1f;
    private float _timer = 0;
    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += RollingEndHandle;
        _agentMovement.isActiveMove = false;
        //롤링을 마우스로 할지 키보드로 할지 만들기

        Vector3 dir = _agentInput.GetCurrentInputDirection();

        if(dir.magnitude < 0.1f)
        {
            dir = _agentController.transform.forward;
        }
        _agentMovement.SetRotation(dir + _agentController.transform.position);

        _agentMovement.StopImmediately();
        _agentMovement.SetMovementVelocity(transform.forward * _rollingSpeed);
        _agentAnimator.SetRollingState(true);
        _timer = 0;
    }

    public override void OnExitState()
    {
        _agentAnimator.OnAnimationEndTrigger -= RollingEndHandle;
        _agentAnimator.SetRollingState(false);
        _agentMovement.isActiveMove = true;
    }
    private void RollingEndHandle()
    {
        if (_timer < _animationThreshold) return;
        _agentMovement.StopImmediately();
        _agentController.ChangeState(StateType.Normal);
    }
    public override void UpdateState()
    {
        _timer += Time.deltaTime;
    }
}
