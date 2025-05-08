using UnityEngine;
using UnityEngine.SceneManagement;


public enum EGameState
{
    Ready,
    Run,
    Pause,
    Over
}


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private EGameState _gameState = EGameState.Run;
    public EGameState GameState => _gameState;
    
    
    private void Awake()
    {
        // 게임 오브젝트가 삭제될 경우 게임 오브젝트의 참조는 잃지만
        // 스태틱 변수가 남아 있어서 오류가 생기는 경우가 있다.
        // 이럴 경우에는 게임 오브젝트가 삭제되지 않도록
        if (_instance != null)
        {
            //Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(this.gameObject); // -> 씬이 바뀌어도 '그 게임 오브젝트는 삭제하지 않겠다'라는 의미

      
        
        
        _instance = this;
    }
    
    public void Pause()
    {
        _gameState = EGameState.Pause;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        
        PopupManager.Instance.Open(EPopupType.UI_OptionPopup, closeCallback: Continue);
    }

    public void Continue()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        _gameState = EGameState.Run;
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        
        
        // 다시시작을 했더니 게임이 망가지는 경우가 있다...
        // 싱글톤 처리를 잘못했을경우 망가진다.
    }
    
}
