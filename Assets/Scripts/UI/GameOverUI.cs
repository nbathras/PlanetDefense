using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform gameOverUIHolder;

    [Header("Buttons")]
    [SerializeField] private Button saveButton;

    [Header("Dynamic Text")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField playerInitialInputField;

    [Header("Score Text Fields")]
    [SerializeField] private TextMeshProUGUI rankedListText;
    [SerializeField] private TextMeshProUGUI scoreListText;
    [SerializeField] private TextMeshProUGUI nameListText;

    private void Awake() {
        gameOverUIHolder.gameObject.SetActive(false);

        saveButton.onClick.AddListener(delegate { SaveAndQuit(); });
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += GameOverUI_OnGameSetupEvent;
        GameManager.Instance.OnGameOverEvent += GameOverUI_OnGameOverEvent;
        GameManager.Instance.OnGameCleanupEvent += GameOverUI_OnGameCleanupEvent;
    }

    private void GameOverUI_OnGameCleanupEvent(object sender, EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(false);
    }

    private void GameOverUI_OnGameOverEvent(object sender, EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(true);

        int playerScore = ScoreController.Instance.GetTotalScore();

        scoreText.SetText("Score: " + playerScore.ToString());

        PlayFabController.Instance.UpdateScoreTextFieldsNewScore(playerScore, rankedListText, scoreListText, nameListText);
    }

    private void GameOverUI_OnGameSetupEvent(object sender, EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(false);
    }

    private void SaveAndQuit() {
        int playerScore = ScoreController.Instance.GetTotalScore();
        string playerInitial = playerInitialInputField.text;
        if (playerInitial.Length > 3) {
            playerInitial = playerInitial.Substring(0, 3);
        }

        PlayFabController.Instance.SaveHighScore(playerScore, playerInitial);

        GameManager.Instance.CleanupGame();
    }
}
