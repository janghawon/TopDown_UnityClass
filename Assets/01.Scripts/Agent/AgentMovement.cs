using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    private float _gravity = -9.8f;

    private CharacterController _charController;
    private AgentAnimator _agentAnimator;

    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;
    private Vector3 _inputVelocity;

    public bool IsActiveMove { get; set; }

    private AgentController _controller;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _controller = GetComponent<AgentController>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        _inputVelocity = value;
        _movementVelocity = value;
    }

    private void CalculatePlayerMovement()
    {
        //여기는 나중에
        _inputVelocity.Normalize();

        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _inputVelocity;
        //쿼터니언 곱셉 교환법칙 성립 안한다.

        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude); //이동속도 반영

        _movementVelocity *= _controller.CharData.MoveSpeed * Time.fixedDeltaTime;
        if(_movementVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
        
    }

    public void SetRotation(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _agentAnimator?.SetSpeed(0); //이동속도 반영
    }

    private void FixedUpdate()
    {
        if(IsActiveMove)
        {
            CalculatePlayerMovement();
        }
            
        if(_charController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _charController.Move(move);
        _agentAnimator?.SetAirbone(!_charController.isGrounded);
    }
}
