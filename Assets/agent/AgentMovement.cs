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

    public bool isActiveMove { get; set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        _movementVelocity = value;
    }

    public void SetRotation(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity.Normalize();

        _movementVelocity = Quaternion.Euler(0, -45, 0) * _movementVelocity;

        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude); // 이동속도 반영

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
        if(isActiveMove)
        {
            CalculatePlayerMovement();
        }
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
