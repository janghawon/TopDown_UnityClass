using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestBezier : MonoBehaviour
{

    [SerializeField]
    private Transform _startTrm, _startCtrlTrm, _endTrm, _endCtrlTrm;

    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    private Vector3[] _points;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            _points = DOCurve.CubicBezier.GetSegmentPointCloud(_startTrm.position, 
                _startCtrlTrm.position, 
                _endTrm.position, 
                _endCtrlTrm.position, 70);

            _line.positionCount = _points.Length;
            _line.SetPositions(_points);

            StartCoroutine(MoveCube());
        }
    }

    private IEnumerator MoveCube()
    {
        //���⸦ �ۼ��Ͽ��� 2�ʵ��� ������ �̵��ϵ��� ��������.
        float time = 2.0f / _points.Length;
        for(int i = 0; i < _points.Length; i++)
        {
            yield return new WaitForSeconds(time);
            transform.position = _points[i];
        }
    }
}
