using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySlashEffectFeedback : Feedback
{
    [SerializeField]
    private EffectPlayer _slashEffect;
    [SerializeField]
    private float _effectPlayTime;

    private AIActionData _aiActionData;
    private void Awake()
    {
        _aiActionData = transform.parent.Find("AI").GetComponent<AIActionData>();
    }

    public override void CreateFeedback()
    {
        EffectPlayer effect = PoolManager.Instance.Pop(_slashEffect.name) as EffectPlayer;
        effect.transform.position = _aiActionData.HitPoint;
        effect.StartPlay(_effectPlayTime);
    }

    public override void FinishFeedback()
    {
        //do nothing;
    }
}
