using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _blades;

    [SerializeField]
    private VisualEffect _footstep;

    [SerializeField]
    private VisualEffect _healEffect;

    private AttackState _atkState;

    private void Awake()
    {
        _atkState = transform.Find("States").GetComponent<AttackState>();
        _atkState.OnAttackStart += PlayBlade;
        _atkState.OnAttackStateEnd += StopBlade;
    }

    private void PlayBlade(int combo)
    {
        _blades[combo - 1].Play();
    }

    private void StopBlade()
    {
        foreach(ParticleSystem p in _blades)
        {
            p.Simulate(0);
            p.Stop();
        }
    }

    public void UpdateFootStep(bool value)
    {
        if (value)
            _footstep.Play();
        else
            _footstep.Stop();
    }

    public void PlayHealEffect()
    {
        _healEffect.Play();
    }

}
