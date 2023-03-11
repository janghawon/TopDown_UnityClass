using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private Dictionary<Core.StateType, IState> _stateDictionary = null;
    private IState _currentState;

    private void Awake()
    {
        _stateDictionary = new Dictionary<Core.StateType, IState>();

        Transform stateTrm = transform.Find("State");

        foreach(Core.StateType state in )
    }
}
