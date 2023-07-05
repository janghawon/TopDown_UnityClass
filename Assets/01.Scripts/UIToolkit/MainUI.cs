using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDocument.rootVisualElement;
        Button popupBtn = root.Q<Button>("myPopupBtn");

        VisualElement popup = root.Q<VisualElement>("popupWindow");

        popupBtn.RegisterCallback<ClickEvent>(e =>
        {
            Time.timeScale = 0;
            popup.AddToClassList("on"); //Ŭ������ on�� �ٿ��ش�.
        });

        Button closeBtn = root.Q<Button>("closeBtn");
        closeBtn.RegisterCallback<ClickEvent>(e =>
        {
            Time.timeScale = 1;
            popup.RemoveFromClassList("on");
        });
    }
}
