using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Core;

public class PopupText : PoolableMono
{
    private TextMeshPro _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TextMeshPro>();
    }

    public void Startpopup(string text, Vector3 pos, int fontsize, Color color, float time = 1f, float yDelta = 2f)
    {
        _tmpText.SetText(text);
        _tmpText.color = color;
        _tmpText.fontSize = fontsize;
        transform.position = pos;
        StartCoroutine(ShowRoutine(time, yDelta));
    }

    IEnumerator ShowRoutine(float time, float yDelta)
    {
        float currentTime = 0;
        Vector3 firstPos = transform.position;
        float percent = 0;

        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;
            float nextY = yDelta * (float)EaseOutCubic(percent);
            float nextOpacity = Mathf.Lerp(1, 0, percent);

            transform.position = firstPos + new Vector3(0, nextY, 0);
            _tmpText.alpha = nextOpacity;

            Vector3 camDir = (transform.position - Define.MainCam.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(camDir);
            yield return null;
        }
    }

    public override void Init()
    {
        _tmpText.alpha = 1;
    }

    private float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    private double EaseOutCubic(float x)
    {
        return 1 - Math.Pow(1 - x, 3);
    }
}
