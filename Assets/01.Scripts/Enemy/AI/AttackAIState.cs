using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAIState : CommonAIState
{
    protected Vector3 _targetVector;
    protected bool _isActive = false;

    private float _lastAtkTime;

    private EnemyDataSO _dataSO;

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        _dataSO = _enemyController.EnemyData;
    }

    public override void OnEnterState()
    {
        _enemyController.NavMovement.StopImmediately();
        _enemyController.AgentAnimator.OnAnimationEndTrigger += AttackAnimationEndHandle;
        _enemyController.AgentAnimator.OnAnimationEventTrigger += AttackCollisionHandle;
        _enemyController.AgentAnimator.OnPreAnimationEventTrigger += PreAttackHandle;
        _isActive = true;
    }


    public override void OnExitState()
    {
        _enemyController.AgentAnimator.OnAnimationEndTrigger -= AttackAnimationEndHandle;
        _enemyController.AgentAnimator.OnAnimationEventTrigger -= AttackCollisionHandle;
        _enemyController.AgentAnimator.OnPreAnimationEventTrigger -= PreAttackHandle;

        _enemyController.AgentAnimator.SetAttackState(false);
        _enemyController.AgentAnimator.SetAttackTrigger(false);
        _enemyController.EnemyAttackCompo.CancelAttack();  //������ �����ϴ��� ��� ��Ű��
        
        _aiActionData.IsAttacking = false;
        _isActive = false;
    }

    private void PreAttackHandle()
    {
        _enemyController.EnemyAttackCompo.PreAttack();
    }

    private void AttackAnimationEndHandle()
    {
        //�ִϸ��̼��� ������ ���� ���� ��
        _enemyController.AgentAnimator.SetAttackState(false);
        _lastAtkTime = Time.time; //�������� 1�� ��ٷȴٰ� �ٽ� ������ �߻�
        StartCoroutine(DelayCoroutine(() => _aiActionData.IsAttacking = false, _dataSO.MotionDelay));
    }

    private IEnumerator DelayCoroutine(Action Callback, float time)
    {
        yield return new WaitForSeconds(time);
        Callback?.Invoke();
    }

    private void AttackCollisionHandle()
    {
        _enemyController.EnemyAttackCompo.Attack(_dataSO.AtkDamage, _targetVector);
    }

    private void SetTarget()
    {
        _targetVector = _enemyController.TargetTrm.position - transform.position;
        _targetVector.y = 0; //Ÿ���� �ٶ󺸴� ������ �����ִ� �ż���
    }

    public override bool UpdateState()
    {
        if(base.UpdateState())
        {
            return true;
        }

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
                    = Quaternion.Euler(0, sign * _dataSO.RotateSpeed * Time.deltaTime, 0) 
                        * _enemyController.transform.rotation;
            }else if(_lastAtkTime + _dataSO.AtkCoolTime < Time.time) //��Ÿ�ӵ� á�� ������ 10���� ���Դٸ�
            {
                _aiActionData.IsAttacking = true;
                _enemyController.AgentAnimator.SetAttackState(true);
                _enemyController.AgentAnimator.SetAttackTrigger(true);
            }            
        }

        return false;
    }

}
