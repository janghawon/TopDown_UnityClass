using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitTriggered = null;
    private AIActionData _aIActionData;

    private void Awake()
    {
        _aIActionData = transform.Find("AI").GetComponent<AIActionData>();
    }

    public void OnDamage(int damage, Vector3 point, Vector3 normal)
    {
        _aIActionData.HitPoint = point;
        _aIActionData.HitNormal = normal;



        OnHitTriggered?.Invoke();
    }

    
}
