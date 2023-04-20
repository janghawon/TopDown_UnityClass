using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : CommonState
{
    //protected AgentMovement _agentMovement;

    //public override void SetUp(Transform agentRoot)
    //{
    //    base.SetUp(agentRoot);
    //    _agentMovement = agentRoot.GetComponent<AgentMovement>();
    //}

    public override void OnEnterState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress += OnMovementHandle; //���� �� Ű�Է� ����
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentInput.OnRollingKeyPress += OnRollingHandle;

        _agentMovement.IsActiveMove = true;
    }
       

    public override void OnExitState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress -= OnMovementHandle; //������ Ű�Է� ��������
        _agentInput.OnAttackKeyPress -= OnAttackHandle;
        _agentInput.OnRollingKeyPress -= OnRollingHandle;

        _agentMovement.IsActiveMove = false;
    }

    private void OnAttackHandle()
    {
        //����Ű�� ó�� ���� ���� ���ݻ��·� ��ȯ�Ǵµ� 
        _agentMovement.SetRotation(_agentInput.GetMouseWorldPosition());
        _agentController.ChangeState(StateType.Attack);
    }


    private void OnMovementHandle(Vector3 obj)
    {
        _agentMovement?.SetMovementVelocity(obj);
    }

    private void OnRollingHandle()
    {
        _agentController.ChangeState(StateType.Rolling);
    }

    public override void UpdateState()
    {
        
    }
}
