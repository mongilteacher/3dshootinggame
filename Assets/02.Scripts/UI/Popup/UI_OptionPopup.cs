using UnityEngine;

public class UI_OptionPopup : MonoBehaviour
{
    public UI_CreditPopup CreditPopup;
    
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void OnClickContinueButton()
    {       
        GameManager.Instance.Continue();

        gameObject.SetActive(false);
    }

    public void OnClickRetryButton()
    {
        GameManager.Instance.Restart();
    }

    public void OnClickQuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnClickCreditButton()
    {
        CreditPopup.Open();
    }
}
