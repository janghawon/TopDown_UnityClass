using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonState : MonoBehaviour, IState
{
    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract void UpdateState();

    protected AgentAnimator _agentAnimator;
    protected AgentInput _agentInput;
    protected AgentController _agentController;
    public virtual void SetUp(Transform agentRoot)
    {
        Debug.Log(agentRoot);
        _agentAnimator = agentRoot.Find("Visual").GetComponent<AgentAnimator>();
        _agentInput = agentRoot.GetComponent<AgentInput>();
        Debug.Log(_agentInput.gameObject);
        _agentController = GetComponent<AgentController>();
    }
    public void OnHitHandle(Vector3 position, Vector3 normal)
    {

    }
}
