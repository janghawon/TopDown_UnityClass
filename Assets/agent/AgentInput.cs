using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Core.Define;

public class AgentInput : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround;
    public event Action<Vector3> OnMovementKeyPress = null;
    public event Action OnAttackKeyPress = null;
    public event Action OnRollingKeyPress = null;
    private void Update()
    {
        UpdateMoveInput();
        UpdateAttackInput();
        UpdateRollingInput();
    }

    private void UpdateRollingInput()
    {
        if(Input.GetButtonDown("Jump"))
        {
            OnRollingKeyPress?.Invoke();
        }
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

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        bool result = Physics.Raycast(ray, out hit, MainCam.farClipPlane, _whatIsGround);

        if(result)
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
