using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamAttack : EnemyAttack
{
    [SerializeField] private Beam _beamPrefab;
    [SerializeField] private Transform _atkPosTrm;

    private Beam _currentBeam;

    public override void Attack(int damage, Vector3 targetDir)
    {
        if (_currentBeam == null) return;
        _currentBeam.FireBeam(damage, targetDir);
        _currentBeam = null;
    }

    public override void CancleAttack()
    {
        if(_currentBeam != null)
        {
            _currentBeam.StopBeam();
        }
        _currentBeam = null;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public override void PreAttack()
    {
        _currentBeam = PoolManager.Instance.Pop(_beamPrefab.gameObject.name) as Beam;
        _currentBeam.transform.position = _atkPosTrm.position;
        _currentBeam.PreCharging();
    }
}
