using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : CommonState
{
    [SerializeField]
    private float _rollingSpeed = 0.4f, _animationThreshold = 0.1f; //��� �ϵ��� �� �� ã�ƾ� �Ѵ�.
    private float _timer = 0;

    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += RollingEndHandle;
        _agentMovement.IsActiveMove = false;
        //���⼭ �Ѹ��� ���콺�ִ°����� �Ѹ�����, Ű���带 ���� �������� �Ѹ������� �����ؾ���.
         
        Vector3 dir = _agentInput.GetCurrentInputDirection();
        //Ű���带 �ȴ����ٸ� ���� ���� ĳ���Ͱ� �ٶ󺸴� �������� �Ѹ�
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
        _agentMovement.IsActiveMove = true;
    }

    private void RollingEndHandle()
    {
        if (_timer < _animationThreshold) return; //�̰� ������ 0.1�ʵ� �������Ŵϱ� �ȳ���.
        _agentMovement.StopImmediately();
        _agentController.ChangeState(StateType.Normal);
    }


    public override void UpdateState()
    {
        _timer += Time.deltaTime;
    }
}
