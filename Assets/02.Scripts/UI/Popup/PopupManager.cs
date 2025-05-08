using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum EPopupType
{
    UI_OptionPopup,
    UI_CreditPopup
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    

    [Header("팝업 UI 참조")]
    public List<UI_Popup> Popups; // 모든 팝업을 관리하는데

    private List<UI_Popup> _openedPopups = new List<UI_Popup>(); // null은 아니지만 비어있는 리스트
    
    private void Awake()
    {
        Instance = this;
    }

    public void Open(EPopupType type, Action closeCallback = null)
    {
        Open(type.ToString(),  closeCallback);
    }
    
    private void Open(string popupName, Action closeCallback)
    {
        foreach (UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                // 팝업을 열 때마다 담는다.
                _openedPopups.Add(popup);
                break;
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                Debug.Log("A");
                _openedPopups[_openedPopups.Count - 1].Close();
                _openedPopups.RemoveAt(_openedPopups.Count - 1);
            }
            else
            {
                Debug.Log("B");

                GameManager.Instance.Pause();
            }
        }
    }
}
