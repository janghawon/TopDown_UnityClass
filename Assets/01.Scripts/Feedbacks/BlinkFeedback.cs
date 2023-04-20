using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkFeedback : Feedback
{
    [SerializeField]
    private SkinnedMeshRenderer _meshRenderer;
    [SerializeField]
    private float _blinkTime = 0.2f;

    private MaterialPropertyBlock _matPropBlock;

    private readonly int _blinkHash = Shader.PropertyToID("_Blink");
    // readonly ��Ÿ��
    // const ������ Ÿ��

    private void Awake()
    {
        _matPropBlock = new MaterialPropertyBlock();
        _meshRenderer.GetPropertyBlock(_matPropBlock);
    }

    public override void CreateFeedback()
    {
        StartCoroutine(MaterialBlink());
    }

    private IEnumerator MaterialBlink()
    {
        _matPropBlock.SetFloat(_blinkHash, 0.7f);
        _meshRenderer.SetPropertyBlock(_matPropBlock);

        yield return new WaitForSeconds(_blinkTime);

        _matPropBlock.SetFloat(_blinkHash, 0);
        _meshRenderer.SetPropertyBlock(_matPropBlock);
    }

    public override void FinishFeedback()
    {
        StopAllCoroutines(); //��� �ڷ�ƾ ����
        _matPropBlock.SetFloat(_blinkHash, 0);
        _meshRenderer.SetPropertyBlock(_matPropBlock);
    }
}
