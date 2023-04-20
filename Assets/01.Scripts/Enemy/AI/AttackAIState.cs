using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAIState : CommonAIState
{
    protected float _rotateSpeed = 360f;
    protected Vector3 _targetVector;

    protected bool _isActive = false;

    protected int _atkDamage = 1;
    protected float _atkCooltime = 0.2f;

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        _atkDamage = _enemyController.EnemyData.AtkDamage;
        _atkCooltime = _enemyController.EnemyData.AtkCoolTime;
        _rotateSpeed = _enemyController.EnemyData.RotateSpeed;
    }

    public override void OnEnterState()
    {
        _enemyController.NavMovement.StopImmediately();
        _enemyController.AgentAnimator.OnAnimationEndTrigger += AttackAnimationEndHandle;
        _enemyController.AgentAnimator.OnAnimationEventTrigger += AttackCollisionHandle;
        _isActive = true;
    }


    public override void OnExitState()
    {
        _enemyController.AgentAnimator.OnAnimationEndTrigger -= AttackAnimationEndHandle;
        _enemyController.AgentAnimator.OnAnimationEventTrigger -= AttackCollisionHandle;

        _enemyController.AgentAnimator.SetAttackState(false);
        _enemyController.AgentAnimator.SetAttackTrigger(false);
        _isActive = false;
    }

    private void AttackAnimationEndHandle()
    {
        //�ִϸ��̼��� ������ ���� ���� ��
        _enemyController.AgentAnimator.SetAttackState(false);
        StartCoroutine(DelayCoroutine(() => _aiActionData.IsAttacking = false, _atkCooltime));
    }

    private IEnumerator DelayCoroutine(Action Callback, float time)
    {
        yield return new WaitForSeconds(time);
        Callback?.Invoke();
    }

    private void AttackCollisionHandle()
    {
        //���� �������� �� �ֺ��� ĳ�����ؼ� �������� �ִ� ���� �־�� �Ѵ�.
        //���߿� ������Ʈȭ �����̰�
    }

    private void SetTarget()
    {
        _targetVector = _enemyController.TargetTrm.position - transform.position;
        _targetVector.y = 0; //Ÿ���� �ٶ󺸴� ������ �����ִ� �ż���
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(_aiActionData.IsAttacking == false && _isActive)  //��Ƽ��
        {
            SetTarget(); //���� ��ġ�� �����ؼ� targetVector�� ������ְ� 

            //_enemyController.transform.rotation = Quaternion.LookRotation(_targetVector);
            Vector3 currentFrontVector = transform.forward;
            float angle = Vector3.Angle(currentFrontVector, _targetVector);

            if(angle >= 10f)
            {
                //�������ϰ�
                Vector3 result = Vector3.Cross(currentFrontVector, _targetVector);

                float sign = result.y > 0 ? 1 : -1;
                _enemyController.transform.rotation
                    = Quaternion.Euler(0, sign * _rotateSpeed * Time.deltaTime, 0) 
                        * _enemyController.transform.rotation;
            }else
            {
                _aiActionData.IsAttacking = true;
                _enemyController.AgentAnimator.SetAttackState(true);
                _enemyController.AgentAnimator.SetAttackTrigger(true);
            }            
        }
    }

}
