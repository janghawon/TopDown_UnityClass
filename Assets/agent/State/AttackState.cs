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
    private float _keyDelay = 0.5f; //0.5�ʳ��� ���콺�� �ѹ� ������� ����

    private int _currentCombo = 0; //���� �޺� ��ġ
    private bool _canAttack = true; //���� ������ �����Ѱ�?
    private float _keyTimer = 0;

    private float _attackStartTime; //������ ���۵� �ð�
    [SerializeField]
    private float _attackSlideDuration = 0.2f, _attackSlideSpeed = 0.1f;

    private DamageCaster _damageCaster;

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        _damageCaster = agentRoot.Find("DamageCaster").GetComponent<DamageCaster>();
    }

    private void OnDamageCastHandle()
    {
        _damageCaster.CastDamage();
    }

    public override void OnEnterState()
    {
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentAnimator.OnAnimationEndTrigger += OnAnimationEndHandle;
        _agentInput.OnRollingKeyPress += OnRollingHandle;
        _agentAnimator.OnAnimationEventTrigger += OnDamageCastHandle;
        _currentCombo = 0;
        _canAttack = true;
        _agentAnimator.SetAttackState(true); //���ݻ��·� ��ȯ
        OnAttackHandle(); //�������� ���� ����
    }

    public override void OnExitState()
    {
        _agentInput.OnAttackKeyPress -= OnAttackHandle;
        _agentAnimator.OnAnimationEndTrigger -= OnAnimationEndHandle;
        _agentInput.OnRollingKeyPress -= OnRollingHandle;
        _agentAnimator.OnAnimationEventTrigger -= OnDamageCastHandle;
        _agentAnimator.SetAttackState(false); //���ݻ��·� ��ȯ
        _agentAnimator.SetAttackTrigger(false);
    }

    private void OnAnimationEndHandle()
    {
        _canAttack = true;
        _keyTimer = _keyDelay; //0.5�ʽð� �־�ΰ� �������� ī��Ʈ ����
    }

    private void OnAttackHandle()
    {
        if (_canAttack && _currentCombo < 3)
        {
            _attackStartTime = Time.time; //���� ���� �ð��� ����Ѵ�.
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

        //�����̵��� �Ǿ�� �ϴ� �ð��̶�� ��
        if (Time.time < _attackStartTime + _attackSlideDuration)
        {
            float timePassed = Time.time - _attackStartTime; //���ݽ������� ���ʵƴ�?
            float lerpTime = timePassed / _attackSlideDuration; //0~1������ ��ȯ

            _agentMovement.SetMovementVelocity(
                Vector3.Lerp(_agentMovement.transform.forward * _attackSlideSpeed,
                Vector3.zero,
                lerpTime)
            );

        }
    }
}