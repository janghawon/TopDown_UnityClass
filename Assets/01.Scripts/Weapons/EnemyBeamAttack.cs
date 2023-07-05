using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamAttack : EnemyAttack
{
    [SerializeField]
    private Beam _beamPrefab;
    [SerializeField]
    private Transform _atkPosTrm;

    private Beam _currentBeam; //���� ���� ��� �ִ� �� ��ü

    [SerializeField]
    private LayerMask _whatIsEnemy;

    public override void Attack(int damage, Vector3 targetDir)
    {
        if (_currentBeam == null) return; //�̷����� �Ͼ�� �ʰ�����..�׷���.
        _currentBeam.FireBeam(damage, targetDir);
        _currentBeam = null;
    }

    public override void CancelAttack()
    {
        if (_currentBeam != null)  //����� ? ������ ���� �ȵ�
            _currentBeam.StopBeam();
        _currentBeam = null;
    }

    private void OnDisable()
    {
        CancelAttack();
    }

    public override void PreAttack()
    {
        _currentBeam = PoolManager.Instance.Pop(_beamPrefab.gameObject.name) as Beam;
        _currentBeam.WhatIsEnemy = _whatIsEnemy; //Ÿ���� ����
        _currentBeam.transform.position = _atkPosTrm.position;
        _currentBeam.PreCharging(); //������¡ ����
    }
}
