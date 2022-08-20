using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//게임 상황
public enum GameState
{
    menu,
    inGame,
    gameOver,
    gameClear
}

public class GameManager : MonoBehaviour
{

    // 싱글톤 선언
    public static GameManager instance;

    //현재 게임 상황 변수
    public GameState currentGameState = GameState.menu;

    //멈춰있는 상태
    bool isPause;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject gameMenuImg;
    [SerializeField]
    private GameObject gameEndingImg;
    [SerializeField]
    private GameObject gameClearImg;

    //스코어 컴포넌트
    [SerializeField]
    private TextMeshProUGUI clearScoreText;
    [SerializeField]
    private TextMeshProUGUI overScoreText;

    private void Awake()
    {
        instance = this;
        isPause = false;
        ChangeTimeScale(isPause);
    }

    //게임진행 멈춤 or 진행
    public void ChangeTimeScale(bool _isPause)
    {
        if (_isPause)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        isPause = _isPause;
    }

    //게임 시작 버튼 함수
    public void OnClickStartButton()
    {
        Debug.Log("게임시작");
        SetGameState(GameState.inGame);
    }

    //게임 메뉴로가는 버튼 함수
    public void OnClickGameMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    //나가기 버튼 함수
    public void OnClickExitButton()
    {
        Application.Quit();
    }

    //게임 상황 설정함수
    void SetGameState(GameState newGameState)
    {
        switch(newGameState)
        {
            case (GameState.menu):
                gameMenuImg.SetActive(true);
                gameEndingImg.SetActive(false);
                ChangeTimeScale(false);
                break;
            case (GameState.inGame):
                gameMenuImg.SetActive(false);
                gameEndingImg.SetActive(false);
                ChangeTimeScale(true);
                break;
            case (GameState.gameOver):
                gameEndingImg.SetActive(true);
                ChangeTimeScale(false);
                break;
            case (GameState.gameClear):
                gameClearImg.SetActive(true);
                ChangeTimeScale(false);
                break;
        }
        currentGameState = newGameState;
    }

    //외부에서 호출되는 게임 오버 함수
    public void GameOver(int score)
    {
        SetGameState(GameState.gameOver);
        overScoreText.text = "Kill : " + score.ToString();
    }


    //외부에서 호출되는 게임 클리어 함수
    public void GameClear(int score)
    {
        SetGameState(GameState.gameClear);
        clearScoreText.text = "Kill : " + score.ToString();
    }
}
