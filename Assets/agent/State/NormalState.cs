using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class NormalState : CommonState
{
    protected AgentMovement _agentMovement;
    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        _agentMovement = agentRoot.GetComponent<AgentMovement>();
    }

    public override void OnEnterState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress += OnMovementHandle; //들어올 때 키입력 구독
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentInput.OnAttackKeyPress -= OnAttackHandle;
    }

    private void OnAttackHandle()
    {
        _agentController.ChangeState(StateType.Attack);
    }

    public override void OnExitState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress -= OnMovementHandle; //나갈때 키입력 구독해제
    }

    private void OnMovementHandle(Vector3 obj)
    {
        _agentMovement?.SetMovementVelocity(obj);
    }

    public override void UpdateState()
    {

    }
}