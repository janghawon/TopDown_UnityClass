using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : CommonState
{
    public Action<int> OnAttackStart = null;
    public Action onAttackEnd = null;

    [SerializeField]
    private float _keyDelay = 0.5f; //0.5초내에 마우스를 한번 눌러줘야 실행

    private int _currentCombo = 0; //현재 콤보 수치
    private bool _canAttack = true; //현재 공격이 가능한가?
    private float _keyTimer = 0;

    private float _attackStartTime; //공격이 시작된 시간
    [SerializeField]
    private float _attackSlideDuration = 0.2f, _attackSlideSpeed = 0.1f;

    public override void OnEnterState()
    {
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentAnimator.OnAnimationEndTrigger += OnAnimationEndHandle;
        _agentInput.OnRollingKeyPress += OnRollingHandle;
        _currentCombo = 0;
        _canAttack = true;
        _agentAnimator.SetAttackState(true); //공격상태로 전환
        OnAttackHandle(); //수동으로 공격 시작
    }

    public override void OnExitState()
    {
        _agentInput.OnAttackKeyPress -= OnAttackHandle;
        _agentAnimator.OnAnimationEndTrigger -= OnAnimationEndHandle;
        _agentInput.OnRollingKeyPress -= OnRollingHandle;
        _agentAnimator.SetAttackState(false); //공격상태로 전환
        _agentAnimator.SetAttackTrigger(false);
    }

    private void OnAnimationEndHandle()
    {
        _canAttack = true;
        _keyTimer = _keyDelay; //0.5초시간 넣어두고 이제부터 카운트 시작
    }

    private void OnAttackHandle()
    {
        if (_canAttack && _currentCombo < 3)
        {
            _attackStartTime = Time.time; //공격 시작 시간을 기록한다.
            _canAttack = false;
            _agentAnimator.SetAttackTrigger(true);
            _currentCombo++;
        }
    }

    private void OnRollingHandle()
    {
        _agentController.ChangeState(StateType.Rolling);
    }

    public override void UpdateState()
    {
        if (_canAttack && _keyTimer > 0)
        {
            _keyTimer -= Time.deltaTime;
            if (_keyTimer <= 0)
            {
                _agentController.ChangeState(StateType.Normal);
            }
        }

        //슬라이딩이 되어야 하는 시간이라는 뜻
        if (Time.time < _attackStartTime + _attackSlideDuration)
        {
            float timePassed = Time.time - _attackStartTime; //공격시작한지 몇초됐니?
            float lerpTime = timePassed / _attackSlideDuration; //0~1값으로 변환

            _agentMovement.SetMovementVelocity(
                Vector3.Lerp(_agentMovement.transform.forward * _attackSlideSpeed,
                Vector3.zero,
                lerpTime)
            );

        }
    }
}