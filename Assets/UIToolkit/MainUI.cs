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
        VisualElement root = _uiDocument.rootVisualElement;

        Button popupBtn = root.Q<Button>("MyPopUpBtn");

        popupBtn.RegisterCallback<ClickEvent>(e =>
        {
            Debug.Log("버튼 클릭ㅋ");
        });
    }

}
