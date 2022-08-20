using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState = GameState.menu;

    //∏ÿ√Á¿÷¥¬ ªÛ≈¬
    bool isPause;
    [SerializeField]
    private GameObject gameMenuImg;
    [SerializeField]
    private GameObject gameEndingImg;

    private void Awake()
    {
        instance = this;
        isPause = false;
        ChangeTimeScale(isPause);
    }

    //∞‘¿”¡¯«‡ ∏ÿ√„ or ¡¯«‡
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
        SetGameState(GameState.inGame);
    }

    public void OnClickGameMenuButton()
    {
        SceneManager.LoadScene(0);
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
        }
        currentGameState = newGameState;
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {

    }
}
