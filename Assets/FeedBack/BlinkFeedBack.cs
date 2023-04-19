using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkFeedBack : FeedBack
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private float _blinkTime = 0.2f;

    private MaterialPropertyBlock _matPropBlock;

    private readonly int _blinkHash = Shader.PropertyToID("_Blink");

    private void Awake()
    {
        _matPropBlock = new MaterialPropertyBlock();
        _meshRenderer.GetPropertyBlock(_matPropBlock);
    }

    public override void CreateFeddBack()
    {
        StartCoroutine(MaterialBlock());
    }

    IEnumerator MaterialBlock()
    {
        _matPropBlock.SetFloat(_blinkHash, 0.7f);
        _meshRenderer.SetPropertyBlock(_matPropBlock);

        yield return new WaitForSeconds(_blinkTime);

        _matPropBlock.SetFloat(_blinkHash, 0);
        _meshRenderer.SetPropertyBlock(_matPropBlock);
    }

    public override void FinishFeedBack()
    {
        StopAllCoroutines(); // 모든 코루틴 중지
        _matPropBlock.SetFloat(_blinkHash, 0);
        _meshRenderer.SetPropertyBlock(_matPropBlock);
    }
}
