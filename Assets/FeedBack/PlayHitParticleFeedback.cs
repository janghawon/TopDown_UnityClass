using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHitParticleFeedback : FeedBack
{
    [SerializeField] private PoolableMono _hitParticle;
    [SerializeField] private float _effectPlayTime;

    private AIActionData _aIActionData;

    private void Awake()
    {
        _aIActionData = transform.parent.Find("AI").GetComponent<AIActionData>();
    }

    public override void CreateFeddBack()
    {
        EffectPlayer effect = PoolManager.Instance.Pop(_hitParticle.name) as EffectPlayer;
        effect.transform.position = _aIActionData.HitPoint;
        effect.transform.rotation = Quaternion.LookRotation(_aIActionData.HitNormal * -1);
        effect.StartPlay(_effectPlayTime);
    }

    public override void FinishFeedBack()
    {

    }
}
