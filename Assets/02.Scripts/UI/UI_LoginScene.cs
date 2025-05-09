using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;  // 결과 텍스트
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordComfirmInputField;
    public Button ConfirmButton;   // 로그인 or 회원가입 버튼
}

public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject ResisterPanel;

    [Header("로그인")] 
    public UI_InputFields LoginInputFields;
    
    [Header("회원가입")] 
    public UI_InputFields RegisterInputFields;

    private const string SALT = "10043420";
    
    

    // 게임 시작하면 로그인 켜주고 회원가입은 꺼주고..
    private void Start()
    {
        LoginPanel.SetActive(true);
        ResisterPanel.SetActive(false);
        
        LoginInputFields.ResultText.text    = string.Empty;
        RegisterInputFields.ResultText.text = string.Empty;
    }

    // 회원가입하기 버튼 클릭
    public void OnClickGoToResisterButton()
    {
        LoginPanel.SetActive(false);
        ResisterPanel.SetActive(true);
    }
    
    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        ResisterPanel.SetActive(false);
    }


    // 회원가입
    public void Resister()
    {
        // 1. 아이디 입력을 확인한다.
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            RegisterInputFields.ResultText.text = "아이디를 입력해주세요.";
            return;
        }
        
        // 2. 1차 비밀번호 입력을 확인한다.
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            RegisterInputFields.ResultText.text = "비밀번호를 입력해주세요.";
            return;
        }
        
        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string password2 = RegisterInputFields.PasswordComfirmInputField.text;
        if (string.IsNullOrEmpty(password2))
        {
            RegisterInputFields.ResultText.text = "비밀번호를 입력해주세요.";
            return;
        }

        if (password != password2)
        {
            RegisterInputFields.ResultText.text = "비밀번혹가 다릅니다.";
            return;
        }
        
        // 4. PlayerPrefs를 이용해서 아이디와 비밀번호를 저장한다.
        // (비밀번호를 암호화 해서 저장하세요.)
        PlayerPrefs.SetString(id, Encryption(password + SALT));
        
        // 5. 로그인 창으로 돌아간다.
        // (이때 아이디는 자동 입력되어 있다.)
        OnClickGoToLoginButton();
    }

    public string Encryption(string text)
    {
        // 해시 암호화 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();
        
        // 운영체제 혹은 프로그래밍 언어별로 string 표현하는 방식이 다 다르므로
        // UTF8 버전 바이트로 배열로 바꿔야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);
        
        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            // byte를 다시 string으로 바꿔서 이어붙이기
            resultText += b.ToString("X2");
        }

        return resultText;
    }
    
}
