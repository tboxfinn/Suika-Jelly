using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    public int CurrentScore;
    public int FinalScore;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Image FadePanel;
    [SerializeField] private float _fadeTime = 2f;

    public float TimeTillGameOver = 1.5f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeGame;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FadeGame;
    }

    private void OnDestroy()
    {
        OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState state)
    {
        Debug.Log("Game State Changed to: " + state);
    }

    private void Update()
    {
        if (State == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UpdateGameState(GameState.Paused);
            }

            _gameOverPanel.SetActive(false);
        }

        if (State == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UpdateGameState(GameState.Playing);
            }
        }

        if (State == GameState.GameOver)
        {
            _gameOverPanel.SetActive(true);
        }
        
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _gameOverPanel.SetActive(false);
        _scoreText.text = CurrentScore.ToString("0");
        OnGameStateChanged += GameManagerOnStateChanged;
       
    }

    private void Start()
    {
        UpdateGameState(GameState.Playing);
        CurrentScore = 0;
        FinalScore = 0;
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Playing:
                break;
            case GameState.GameOver:
                break;
            case GameState.Paused:
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }

        OnGameStateChanged?.Invoke(newState);
    }

#region Recompensas
        public void DuplicarScoreDespuesDeAnuncio()
        {
            FinalScore = CurrentScore * 2;
            _finalScoreText.text = FinalScore.ToString("0");
        }
#endregion

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;
        FinalScore = CurrentScore;
        _scoreText.text = CurrentScore.ToString("0");
        _finalScoreText.text = FinalScore.ToString("0");
    }

    public void GameOver()
    {
        UpdateGameState(GameState.GameOver);
    }

    public void PauseGame()
    {
        UpdateGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        UpdateGameState(GameState.Playing);
    }

    public void ResetGame()
    {
        StartCoroutine(ResetGameCoroutine());
    }

    public void EliminarMitadDeFrutas()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruta");
        int half = fruits.Length / 2;
        for (int i = 0; i < half; i++)
        {
            Destroy(fruits[i]);
        }

        UpdateGameState(GameState.Playing);
    }

    private IEnumerator ResetGameCoroutine()
    {
        FadePanel.gameObject.SetActive(true);

        Color startColor = FadePanel.color;
        startColor.a = 0f;
        FadePanel.color = startColor;

        float elapsedTime = 0f;
        while(elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(0f, 1f, (elapsedTime / _fadeTime));
            startColor.a = newAlpha;
            FadePanel.color = startColor;

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FadeGame(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeGameIn());
    }

    private IEnumerator FadeGameIn()
    {
        FadePanel.gameObject.SetActive(true);
        Color startColor = FadePanel.color;
        startColor.a = 1f;
        FadePanel.color = startColor;

        float elapsedTime = 0f;
        while(elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(1f, 0f, (elapsedTime / _fadeTime));
            startColor.a = newAlpha;
            FadePanel.color = startColor;

            yield return null;
        }

        FadePanel.gameObject.SetActive(false);
    }
}

public enum GameState
{
    Playing,
    GameOver,
    Paused,
}