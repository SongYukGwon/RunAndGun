using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    menu,
    inGame,
    gameOver,
    gameClear
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState = GameState.menu;

    //멈춰있는 상태
    bool isPause;
    [SerializeField]
    private GameObject gameMenuImg;
    [SerializeField]
    private GameObject gameEndingImg;
    [SerializeField]
    private GameObject gameClearImg;


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

    public void OnClickStartButton()
    {
        Debug.Log("게임시작");
        SetGameState(GameState.inGame);
    }

    public void OnClickGameMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickExitButton()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

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

    public void GameOver(int score)
    {
        SetGameState(GameState.gameOver);
        overScoreText.text = "Kill : " + score.ToString();
    }

    public void GameClear(int score)
    {
        SetGameState(GameState.gameClear);
        clearScoreText.text = "Kill : " + score.ToString();
    }
}
