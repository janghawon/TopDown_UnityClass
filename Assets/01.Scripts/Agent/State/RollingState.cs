using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : CommonState
{
    [SerializeField]
    private float _rollingSpeed = 0.4f, _animationThreshold = 0.1f; //요건 니들이 값 잘 찾아야 한다.
    private float _timer = 0;

    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += RollingEndHandle;
        _agentMovement.IsActiveMove = false;
        //여기서 롤링을 마우스있는곳으로 롤링할지, 키보드를 누른 방향으로 롤링할지를 결정해야해.
         
        Vector3 dir = _agentInput.GetCurrentInputDirection();
        //키보드를 안눌렀다면 지금 현재 캐릭터가 바라보는 방향으로 롤링
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
        if (_timer < _animationThreshold) return; //이건 들어온지 0.1초도 안지난거니까 안나가.
        _agentMovement.StopImmediately();
        _agentController.ChangeState(StateType.Normal);
    }


    public override bool UpdateState()
    {
        _timer += Time.deltaTime;
        return false;
    }
}
