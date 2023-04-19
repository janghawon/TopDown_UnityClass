using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySlashEffectPrefab : FeedBack
{
    [SerializeField] private PoolableMono _slashEffect;
    [SerializeField] private float _effectPlayTime;

    private AIActionData _aIActionData;

    private void Awake()
    {
        _aIActionData = transform.parent.Find("AI").GetComponent<AIActionData>();
    }

    public override void CreateFeddBack()
    {
        EffectPlayer effect = PoolManager.Instance.Pop(_slashEffect.name) as EffectPlayer;
        effect.transform.position = _aIActionData.HitPoint;
        effect.StartPlay(_effectPlayTime);
    }

    public override void FinishFeedBack()
    {

    }
}
