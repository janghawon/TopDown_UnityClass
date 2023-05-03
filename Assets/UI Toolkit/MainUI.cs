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
        Button popBtn = root.Q<Button>("MyPopupBtn");

        VisualElement popup = root.Q<VisualElement>("popupWindow");

        popBtn.RegisterCallback<ClickEvent>(e =>
        {
            Time.timeScale = 0;
            popup.AddToClassList("on");
        });

        Button popexitBtn = root.Q<Button>("popupexitBtn");

        popexitBtn.RegisterCallback<ClickEvent>(e =>
        {
            Time.timeScale = 1;
            popup.RemoveFromClassList("on");
        });
    }
}
