using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentInput : MonoBehaviour
{
    public event Action<Vector3> OnMovementKeyPress = null;

    private void Update()
    {
        UpdateMoveInput();
    }

    private void UpdateMoveInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        OnMovementKeyPress?.Invoke(new Vector3(h, 0, v));
    }
}
