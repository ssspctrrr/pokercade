using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public int baseTargetScore = 300;
    public float levelScaling = 1.5f; // Score requirement * 1.5 every level
    public int maxHands = 4;
    public int maxDiscards = 3;

    [Header("Current State")]
    public int currentLevel = 1;
    public int currentScore = 0;
    public int targetScore;
    public int handsRemaining;
    public int discardsRemaining;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI handsText;
    public TextMeshProUGUI discardsText;
    public TextMeshProUGUI levelText;
    
    [Header("Panels")]
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;
    public Button nextLevelButton;
    public Button restartButton;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [System.Obsolete]
    void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
{
    currentLevel = 1;
    targetScore = baseTargetScore;
    
    // Reset variables
    currentScore = 0;
    handsRemaining = maxHands;
    discardsRemaining = maxDiscards;
    UpdateUI();

    levelCompletePanel.SetActive(false);
    gameOverPanel.SetActive(false);

    // Use the same robust sequence for a fresh game
    FindFirstObjectByType<SpawnManager>().TriggerNextLevel();
}

    [System.Obsolete]
    public void ResetRound()
    {
        currentScore = 0;
        handsRemaining = maxHands;
        discardsRemaining = maxDiscards;
        UpdateUI();
        
        // Ensure SpawnManager deals a fresh hand if needed
        FindObjectOfType<SpawnManager>().RefillHand();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();
        CheckWinCondition();
    }

    public bool TryUseDiscard()
    {
        if (discardsRemaining > 0)
        {
            discardsRemaining--;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool TryUseHand()
    {
        if (handsRemaining > 0)
        {
            handsRemaining--;
            UpdateUI();
            return true;
        }
        return false;
    }

    void CheckWinCondition()
    {
        if (currentScore >= targetScore)
        {
            Debug.Log("Level Complete!");
            levelCompletePanel.SetActive(true);
            
        
        }
        else if (handsRemaining <= 0)
        {
            Debug.Log("Game Over");
            gameOverPanel.SetActive(true);
        }
    }


    // Inside GameManager.cs

public void OnNextLevelButton()
{
    // 1. Increase difficulty
    currentLevel++;
    targetScore = Mathf.RoundToInt(targetScore * levelScaling);

    // 2. Reset the Game Variables (Score, Hands, Discards)
    currentScore = 0;
    handsRemaining = maxHands;
    discardsRemaining = maxDiscards;
    UpdateUI();
    
    // 3. Hide the Panel
    levelCompletePanel.SetActive(false);

    // 4. Trigger the new Card Sequence
    // We use FindFirstObjectByType to call the Coroutine we wrote above
    FindFirstObjectByType<SpawnManager>().TriggerNextLevel();
}

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateUI()
    {
        if(scoreText) scoreText.text = $"{currentScore}";
        if(targetText) targetText.text = $"/ {targetScore}";
        if(handsText) handsText.text = $"Hands: {handsRemaining}";
        if(discardsText) discardsText.text = $"Discards: {discardsRemaining}";
        if(levelText) levelText.text = $"Level: {currentLevel}";
    }
}