using System.Collections.Generic;
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
    
    private void Awake()
    {
        Instance = this;
    }

    public void Open(EPopupType type)
    {
        Open(type.ToString());
    }
    
    private void Open(string popupName)
    {
        foreach (UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open();
                break;
            }
        }
    }
}
