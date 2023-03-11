using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 8f, _gravity = -9.8f;
    private CharacterController _characterController;
    private AgentAnimator _agentAnimator;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        _movementVelocity = value;
    }
    private void CalculatePlayerMovement()
    {
        _movementVelocity.Normalize();

        _movementVelocity = Quaternion.Euler(0, -45, 0) * _movementVelocity;

        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude); // �̵��ӵ� �ݿ�

        _movementVelocity *= _moveSpeed * Time.fixedDeltaTime;
        if (_movementVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
    }
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();
        if(_characterController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }
        else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _characterController.Move(move);
        _agentAnimator?.SetAirbone(!_characterController.isGrounded);
    }
}