using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamAttack : EnemyAttack
{
    [SerializeField]
    private Beam _beamPrefab;
    [SerializeField]
    private Transform _atkPosTrm;

    private Beam _currentBeam; //내가 지금 쏘고 있는 빔 객체

    [SerializeField]
    private LayerMask _whatIsEnemy;

    public override void Attack(int damage, Vector3 targetDir)
    {
        if (_currentBeam == null) return; //이런일은 일어나지 않겠지만..그래도.
        _currentBeam.FireBeam(damage, targetDir);
        _currentBeam = null;
    }

    public override void CancelAttack()
    {
        if (_currentBeam != null)  //여기는 ? 연산자 쓰면 안돼
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
        _currentBeam.WhatIsEnemy = _whatIsEnemy; //타겟을 설정
        _currentBeam.transform.position = _atkPosTrm.position;
        _currentBeam.PreCharging(); //프리차징 시작
    }
}
