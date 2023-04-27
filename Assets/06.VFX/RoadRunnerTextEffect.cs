using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeChar
{
    public bool IsComplete = false;
    private Vector3[] _originPosition; //����� ���� ����Ʈ�� ������.
    private Vector3[] _startPosition; //���⼭ �����ؼ�
    private float _currentTime = 0; //���� �ð�
    private float _delayTime = 0; //��ٸ��� ��
    private float _effectTime = 0; //����Ʈ ���� ��
    private int _vertexIndex = 0; //ó�� ���� ���ؽ� ��

    private Color _startColor;
    private Color _endColor;

    public TypeChar(int vertexIndex, Vector3[] vertices, Vector3[] startPos, Color32[] colors,
            Color startColor, Color endColor, float delayTime, float effectTime)
    {
        _vertexIndex = vertexIndex;
        _originPosition = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            Vector3 point = vertices[_vertexIndex + i];
            _originPosition[i] = point;

            colors[_vertexIndex + i].a = 0; //������·� ���ְ�
        }
        _startPosition = startPos; //������ġ�� �־��ְ�

        _startColor = startColor;
        _endColor = endColor;
        _delayTime = delayTime;
        _effectTime = effectTime;
    }

    public void UpdateChar(Vector3[] vertices, Color32[] colors)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _delayTime || IsComplete) return; //���� ���������� �ִϸ��̼��̰ų�, �Ϸ�Ǿ��ٸ� ����

        float time = _currentTime - _delayTime;
        float percent = time / _effectTime; //���� �귯�� 0~1���� ���ϰ�

        for (int i = 0; i < 4; i++)
        {
            vertices[_vertexIndex + i] = Vector3.Lerp(_startPosition[i], _originPosition[i], percent);
            colors[_vertexIndex + i] = Color.Lerp(_startColor, _endColor, percent);
        }

        if (percent >= 1)
        {
            IsComplete = true;
        }
    }

}

public class RoadRunnerTextEffect : MonoBehaviour
{
    [SerializeField]
    private float _typeTime = 0.2f;
    [SerializeField]
    private Color _startColor, _endColor;

    private bool _isTyping = false;
    private TMP_Text _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && _isTyping == false)
        {
            StartEffect("Hello world! �ȳ��ϼ���");
        }
    }

    private void StartEffect(string text)
    {
        _tmpText.SetText(text);
        _tmpText.color = _endColor;
        _tmpText.ForceMeshUpdate();
        _isTyping = true;
        StartCoroutine(TypeText());
    }

    [SerializeField]
    private float _ruuningDistance = 15f;

    private IEnumerator TypeText()
    {
        List<TypeChar> charList = new List<TypeChar>();

        TMP_TextInfo textInfo = _tmpText.textInfo;
        Vector3[] vertices = textInfo.meshInfo[0].vertices;
        Color32[] colors = textInfo.meshInfo[0].colors32;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (charInfo.isVisible == false) continue;

            Vector3[] startPos = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                if (j == 0 || j == 3)
                    startPos[j] = vertices[charInfo.vertexIndex + j] + new Vector3(_ruuningDistance, 0, 0);
                else
                    startPos[j] = vertices[charInfo.vertexIndex + j] + new Vector3(_ruuningDistance + 0.5f, 0, 0);
            }
            TypeChar t = new TypeChar(charInfo.vertexIndex, vertices, startPos, colors,
                                        _startColor, _endColor, i * _typeTime, _typeTime);
            charList.Add(t);
        }

        _tmpText.UpdateVertexData(); //��ü ����

        bool isComplete = false;

        while (isComplete == false)
        {
            foreach (TypeChar t in charList)
            {
                t.UpdateChar(vertices, colors);
                isComplete = t.IsComplete;
            }
            //���� �Դµ� isComplete�� true��°� ��� ���ڰ� ���������� �����Ѱ���
            _tmpText.UpdateVertexData();
            yield return null;
        }
        //������� ������ Ÿ���� ��� �����Ŵ�.
        _isTyping = false;
    }
}
