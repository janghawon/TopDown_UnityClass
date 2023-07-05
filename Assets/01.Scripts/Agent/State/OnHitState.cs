using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitState : CommonState
{
    [SerializeField]
    private float _recoverTime = 0.3f;

    private float _hitTimer;

    public override void OnEnterState()
    {
        _agentController.AgentHealthCompo.OnHitTrigger.AddListener( HandleHit );
    }

    public override void OnExitState()
    {
        _agentController.AgentHealthCompo.OnHitTrigger.RemoveListener(HandleHit);
        _agentAnimator.SetIsHit(false);
        _agentAnimator.SetHurtTrigger(false);
    }

    private void HandleHit(int damage, Vector3 point, Vector3 normal)
    {
        normal.y = 0;
        _hitTimer = 0;
        _agentAnimator.SetIsHit(true);
        _agentAnimator.SetHurtTrigger(true);
        _agentController.transform.rotation = Quaternion.LookRotation(normal);
    }

    public override bool UpdateState()
    {
        _hitTimer += Time.deltaTime;
        //���� �˹��� �����Ҳ��� ���⼭ AgentMovement�� active��带 ���ְ�
        // �˹��Ű�ٰ� ������ ���ָ� �ȴ�.
        if(_hitTimer >= _recoverTime)
        {
            _agentController.ChangeState(Core.StateType.Normal); //�� �¾����� �븻�� ��Ŀ��
        }
        return false;
    }
}
