using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 목표: 다음 씬을 '비동기 방식'으로 로드하고 싶다.
    //      또한 로딩 진행률을 시각적으로 표현하고 싶다.
    //                        ㄴ % 프로그레스 바와 %별 텍스트
    
    // 속성:
    // - 다음 씬 번호(인덱스)
    public int NextSceneIndex = 2;

    // - 프로그레스 슬라이더바
    public Slider ProgressSlider;

    // - 프로그레스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScent_Coroutine());
    }

    private IEnumerator LoadNextScent_Coroutine()
    {
        // 지정된 씬을 비동기로 로드한다.
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속해서 반복문
        while (ao.isDone == false)
        {
            // 비동기로 실행할 코드를
            Debug.Log(ao.progress); // 0~1 
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100f}%";
            
            // 서버와 통신해서 유저 데이터나 기획 데이터를 받아오면 된다.
            

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            
            //yield return new WaitForSeconds(1); // 1초 대기
            yield return null;                  // 1프레임 대기 
        }
    }
}
