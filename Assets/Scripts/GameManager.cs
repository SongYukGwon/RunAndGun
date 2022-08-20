using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//���� ��Ȳ
public enum GameState
{
    menu,
    inGame,
    gameOver,
    gameClear
}

public class GameManager : MonoBehaviour
{

    // �̱��� ����
    public static GameManager instance;

    //���� ���� ��Ȳ ����
    public GameState currentGameState = GameState.menu;

    //�����ִ� ����
    bool isPause;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GameObject gameMenuImg;
    [SerializeField]
    private GameObject gameEndingImg;
    [SerializeField]
    private GameObject gameClearImg;

    //���ھ� ������Ʈ
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

    //�������� ���� or ����
    public void ChangeTimeScale(bool _isPause)
    {
        if (_isPause)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
        isPause = _isPause;
    }

    //���� ���� ��ư �Լ�
    public void OnClickStartButton()
    {
        Debug.Log("���ӽ���");
        SetGameState(GameState.inGame);
    }

    //���� �޴��ΰ��� ��ư �Լ�
    public void OnClickGameMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    //������ ��ư �Լ�
    public void OnClickExitButton()
    {
        Application.Quit();
    }

    //���� ��Ȳ �����Լ�
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

    //�ܺο��� ȣ��Ǵ� ���� ���� �Լ�
    public void GameOver(int score)
    {
        SetGameState(GameState.gameOver);
        overScoreText.text = "Kill : " + score.ToString();
    }


    //�ܺο��� ȣ��Ǵ� ���� Ŭ���� �Լ�
    public void GameClear(int score)
    {
        SetGameState(GameState.gameClear);
        clearScoreText.text = "Kill : " + score.ToString();
    }
}
