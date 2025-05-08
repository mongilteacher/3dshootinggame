using UnityEngine;


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

    public UI_OptionPopup OptionPopup;
    
    
    private void Awake()
    {
        _instance = this;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        _gameState = EGameState.Pause;
        
        // Todo:
        // 옵션 팝업을 활성화한다.
        OptionPopup.Open();
    }
    
    
}
