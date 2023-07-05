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

        
        RaycastHit[] hits = Physics.SphereCastAll (startPos, _casterRadius, transform.forward, 
                                 _casterRadius + _casterInterpolation, _targetLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                if (hit.point.sqrMagnitude == 0)
                {
                    continue;
                }

                float dice = Random.value; // 0 ~ 1까지의 값
                int damage = _controller.CharData.BaseDamage;
                int fontSize = 10;
                Color fontColor = Color.white;
                if (dice < _controller.CharData.BaseCritical)
                {
                    damage = Mathf.CeilToInt(damage * _controller.CharData.BaseCriticalDamage);
                    fontSize = 15;
                    fontColor = Color.red;
                }

                health.OnDamage(damage, hit.point, hit.normal);

                //크리티컬 계산해야하지만 일단은 그냥 고

                PopupText text = PoolManager.Instance.Pop("PopupText") as PopupText;
                text.StartPopup(text: damage.ToString(), pos: hit.point + new Vector3(0, 0.5f),
                                fontSize: fontSize, color: fontColor);
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
