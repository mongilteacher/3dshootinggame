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
            
            // 서버와 통신해서 유저 데이터나 기획 데이터를 받아오면 된다.
            if (ao.progress <= 0.1)
            {
                ProgressText.text = $"({ao.progress * 100}) 작전 코드: 리드 데드 시작 중...";
            }
            else if (ao.progress <= 0.3)
            {
                ProgressText.text = $"({ao.progress * 100}) 해병, 총기 점검 완료!";
            }
            else if (ao.progress <= 0.5)
            {
                ProgressText.text = $"({ao.progress * 100}) 임무: 생존자 보호 및 좀비 섬멸";
            }
            else if (ao.progress <= 0.7)
            {
                ProgressText.text = $"({ao.progress * 100}) 사상자 수집 중... 너무 많다";
            }
            else if (ao.progress <= 0.9)
            {
                ProgressText.text = $"({ao.progress * 100}) 진입 포인트 확보 완료";
            }

            if (ao.progress >= 0.9f)
            {              
                ProgressText.text = $"({ao.progress * 100}) Go! Go! Go!";

                ao.allowSceneActivation = true;
            }
            
            //yield return new WaitForSeconds(1); // 1초 대기
            yield return null;                  // 1프레임 대기 
        }
    }
}
