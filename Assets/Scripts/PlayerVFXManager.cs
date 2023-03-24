using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    private AttackState attackState;
    [SerializeField] private ParticleSystem[] _blades;
    [SerializeField] private VisualEffect _footStep;
    private void Awake()
    {
        attackState = transform.Find("States").GetComponent<AttackState>();
        attackState.OnAttackStart += PlayBlade;
        attackState.onAttackEnd += StopBlade;
    }
    public void UpdateFootStep(bool state)
    {
        if(state)
        {
            _footStep.Play();
        }
        else
        {
            _footStep.Stop();
        }
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
