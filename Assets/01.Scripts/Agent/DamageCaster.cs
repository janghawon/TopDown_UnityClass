using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 3f)]
    private float _casterRadius = 1f;

    [SerializeField]
    private float _casterInterpolation = 0.5f;

    [SerializeField]
    private LayerMask _targetLayer;

    private AgentController _controller;

    private void Awake()
    {
        _controller = transform.parent.GetComponent<AgentController>();
    }

    public void CastDamage()
    {
        Vector3 startPos = transform.position - transform.forward * _casterRadius;

        RaycastHit[] hits = Physics.SphereCastAll(startPos, _casterRadius, transform.forward, 
                                 _casterRadius + _casterInterpolation, _targetLayer);

        for(int i = 0; i < hits.Length; i++)
        {
            //Debug.Log($"맞았습니다.{hit.collider.name}");
            if (hits[i].collider.TryGetComponent<IDamageable>(out IDamageable health)) 
            {
                if(hits[i].point.sqrMagnitude == 0)
                {
                    continue;
                }

                float dice = Random.value;
                int damage = _controller.CharacterData.BaseDamage;
                int fontsize = 10;
                Color fontColot = Color.white;
                if(dice < _controller.CharacterData.BaseCritical)
                {
                    damage = Mathf.CeilToInt(damage * _controller.CharacterData.BaseCriticalDamage);
                    fontsize = 15;
                    fontColot = Color.red;
                }

                health.OnDamage(damage, hits[i].point, hits[i].normal);
                PopupText text = PoolManager.Instance.Pop("PopupText") as PopupText;
                text.Startpopup(damage.ToString(), hits[i].point + new Vector3(0, 0.5f), fontsize, fontColot);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(UnityEditor.Selection.activeGameObject == gameObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _casterRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}
