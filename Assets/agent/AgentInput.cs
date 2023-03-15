using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentInput : MonoBehaviour
{
    public event Action<Vector3> OnMovementKeyPress = null;
    public event Action OnAttackKeyPress = null;
    private void Update()
    {
        UpdateMoveInput();
        UpdateAttackInput();
    }

    private void UpdateAttackInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnAttackKeyPress?.Invoke();
        }
    }

    private void UpdateMoveInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        OnMovementKeyPress?.Invoke(new Vector3(h, 0, v));
    }
}
