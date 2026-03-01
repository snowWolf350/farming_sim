using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour,IHasProgress
{
    public static GameManager Instance { get; private set; }


    [Header("Buttons")]
    [SerializeField] Button[] MainMenuButtonList;
    [SerializeField] Button[] RetryButtonList;
    [SerializeField] Button ResumeButton;
    [SerializeField] Button PauseButton;
    [Header("EndScreen")]
    [SerializeField] GameObject EndScreen;
    [SerializeField] TextMeshProUGUI endText;
    [Header("PauseScreenScreen")]
    [SerializeField] GameObject PauseScreen;

    float globalTimer = 0;
    float countDownTimer = 3;
    float gameTimeMax = 120;

    enum gameState
    {
        start,
        game,
        pause,
        won,
        lost
    }

    gameState currentGameState;

    public event EventHandler<IHasProgress.onProgressChangedEventArgs> onProgressChanged;

    public event EventHandler OnGameStaateChanged;

    private void Awake()
    {
        Instance = this;

        foreach (Button button in MainMenuButtonList)
        {
            button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
        }
        foreach (Button button in RetryButtonList)
        {
            button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(1);
            });
        }

        PauseButton.onClick.AddListener(() => {
            Time.timeScale = 0;
            PauseScreen.SetActive(true);
            setGameState(gameState.pause);
        });
        ResumeButton.onClick.AddListener(() => {
            Time.timeScale = 1;
            PauseScreen.SetActive(false);
            setGameState(gameState.game);
        });
    }

    private void Start()
    {
        Time.timeScale = 1;
        setGameState(gameState.start);
        PauseScreen.SetActive(false);
        EndScreen.SetActive(false);
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case gameState.start:
                countDownTimer -= Time.deltaTime;

                onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                {
                    progressNormalized = 1
                });

                if (countDownTimer < 0)
                {
                    setGameState(gameState.game);  
                }
                break;
            case gameState.game:
                globalTimer += Time.deltaTime;
                onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                {
                    progressNormalized = 1 - globalTimer / gameTimeMax
                });

                if (globalTimer > gameTimeMax)
                {
                    if (DeliveryManager.Instance.CheckRequiredLifeAmount())
                    {
                        setGameState(gameState.won);
                    }
                    else
                    {
                        setGameState(gameState.lost);
                    }
                    globalTimer = 0;
                }
                break;
            case gameState.won:
                EndScreen.SetActive(true);
                endText.text = "Congrat's you passed the quota";
                onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                {
                    progressNormalized = 1
                });
                break;
            case gameState.lost:
                EndScreen.SetActive(true);
                endText.text = "Too bad you didn't meet the quota";
                onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                {
                    progressNormalized = 1
                });
                break;
            case gameState.pause:
                break;
        }
    }
    void setGameState(gameState gameState)
    {
        currentGameState = gameState;
        globalTimer = 0;
        OnGameStaateChanged.Invoke(this, EventArgs.Empty);
    }

    public bool CountDownIsActive()
    {
        return currentGameState == gameState.start;
    }
    public bool IsInMenu()
    {
        if(currentGameState == gameState.start|| currentGameState == gameState.game)
        return false;

        return true;
    }
    public float GetCountDownTimer()
    {
        return countDownTimer;
    }
}
