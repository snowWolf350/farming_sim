using System;
using UnityEngine;

public class GameManager : MonoBehaviour,IHasProgress
{
    public static GameManager Instance { get; private set; }

    float globalTimer = 0;
    float countDownTimer = 3;
    float gameTimeMax = 240;

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
    }

    private void Start()
    {
        setGameState(gameState.start);
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

                onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                {
                    progressNormalized = 1
                });
                break;
            case gameState.lost:

                onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                {
                    progressNormalized = 1
                });
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
    public float GetCountDownTimer()
    {
        return countDownTimer;
    }
}
