using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriterTextEffect : MonoBehaviour
{
    [SerializeField]
    private float _typeTime = 0.1f;

    [SerializeField]
    private Color _startColor, _endColor;

    private TMP_Text _tmpText;
    private int _tIndex = 0;
    private bool _isTyping = false;
    [SerializeField] private ParticleSystem _aprticlePrefab;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && _isTyping == false)
        {
            _isTyping = true;
            StartEffect("Hi! this is GGM! �ȳ��ϼ���");
        }
        else if (Input.GetKeyDown(KeyCode.A) && _isTyping == true)
        {
            StopEffect();
        }
    }

    private void StopEffect()
    {
        StopAllCoroutines();
        TMP_TextInfo textInfo = _tmpText.textInfo;
        _tmpText.maxVisibleCharacters = textInfo.characterCount;
        _tmpText.ForceMeshUpdate();
        for (int i = _tIndex; i < textInfo.characterCount; i++)
        {
            StartCoroutine(TypeOneChar(textInfo, i));
        }
        _isTyping = false;
    }

    private void StartEffect(string text)
    {
        _tmpText.SetText(text);
        _tmpText.maxVisibleCharacters = 0;
        _tIndex = 0;
        _tmpText.color = _endColor;
        _tmpText.ForceMeshUpdate();

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        TMP_TextInfo textInfo = _tmpText.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            yield return StartCoroutine(TypeOneChar(textInfo));
        }
        _isTyping = false;
    }

    private IEnumerator TypeOneChar(TMP_TextInfo textInfo, int idx = -1)
    {
        if (idx < 0)
        {
            _tmpText.maxVisibleCharacters = _tIndex + 1;
            _tmpText.ForceMeshUpdate();
        }

        TMP_CharacterInfo charInfo = textInfo.characterInfo[idx < 0 ? _tIndex : idx];

        if (charInfo.isVisible == false)
        {
            yield return new WaitForSeconds(_typeTime); //�ѱ��� Ÿ���� �ð���ŭ ��ٷ��ֱ⸸ �ϰ� ��
        }
        else
        {
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            Color32[] colors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            int vIndex0 = charInfo.vertexIndex;
            int vIndex1 = vIndex0 + 1;
            int vIndex2 = vIndex0 + 2;
            int vIndex3 = vIndex0 + 3; //���߿� ������

            Vector3 v1Origin = vertices[vIndex1];
            Vector3 v2Origin = vertices[vIndex2];

            float currentTime = 0;
            float percent = 0;
            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / _typeTime;

                float yDelta = Mathf.Lerp(2f, 0, percent);

                vertices[vIndex1] = v1Origin + new Vector3(0, yDelta, 0);
                vertices[vIndex2] = v2Origin + new Vector3(0, yDelta, 0);

                //�÷��� ���߿� ��ġ��
                for (int i = 0; i < 4; i++)
                {
                    colors[vIndex0 + i] = Color.Lerp(_startColor, _endColor, percent);
                }

                _tmpText.UpdateVertexData(
                    TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32);
                yield return null;
            }

            Vector3 worldParticlePosition = transform.TransformPoint(vertices[vIndex3]);
            GameObject.Instantiate(_aprticlePrefab, null).transform.position = worldParticlePosition;
        }

        _tIndex++;
    }
}

