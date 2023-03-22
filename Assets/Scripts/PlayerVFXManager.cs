using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
{
    private AttackState attackState;
    [SerializeField] private ParticleSystem[] _blades;
    private void Awake()
    {
        attackState = transform.Find("States").GetComponent<AttackState>();
        attackState.OnAttackStart += PlayBlade;
        attackState.onAttackEnd += StopBlade;
    }

    private void StopBlade()
    {
        foreach(ParticleSystem p in _blades)
        {
            p.Simulate(0);
            p.Stop();
        }
    }

    private void PlayBlade(int combo)
    {
        _blades[combo - 1].Play();
    }
}
