using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Playing, GameOver }

    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PlayerController player;
    [SerializeField, Min(0.1f)] private float initialSpeed = 8f;
    [SerializeField, Min(0.1f)] private float speedIncreaseInterval = 10f;
    [SerializeField, Min(0f)] private float speedIncrease = 1f;
    [SerializeField, Min(0.1f)] private float maximumSpeed = 18f;

    public GameState State { get; private set; } = GameState.Ready;
    public bool IsPlaying => State == GameState.Playing;
    public int Score { get; private set; }
    public float CurrentSpeed { get; private set; }
    public PlayerController Player => player;
    public event Action Changed;
    private float elapsedTime;

    private void Start()
    {
        CurrentSpeed = initialSpeed;
        player.Initialize(this);
        obstacleSpawner.Initialize(this);
        Changed?.Invoke();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (!IsPlaying)
        {
            if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
                StartGame();
            return;
        }

        elapsedTime += Time.deltaTime;
        float previousSpeed = CurrentSpeed;
        CurrentSpeed = Mathf.Min(Mathf.Max(initialSpeed, maximumSpeed), initialSpeed +
            Mathf.Floor(elapsedTime / Mathf.Max(0.1f, speedIncreaseInterval)) * speedIncrease);
        if (CurrentSpeed != previousSpeed) Changed?.Invoke();
    }

    public void StartGame()
    {
        if (IsPlaying || Time.timeScale == 0f) return;
        obstacleSpawner.ResetObstacles();
        Score = 0;
        elapsedTime = 0f;
        CurrentSpeed = initialSpeed;
        player.ResetPlayer();
        State = GameState.Playing;
        obstacleSpawner.StartSpawning();
        Changed?.Invoke();
    }

    public void EndGame()
    {
        if (!IsPlaying) return;
        State = GameState.GameOver;
        obstacleSpawner.StopSpawning();
        player.StopPlayer();
        Changed?.Invoke();
    }

    public void AddScore()
    {
        if (!IsPlaying) return;
        Score++;
        Changed?.Invoke();
    }

}
