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
        GameManager.Instance.OnGameQuitEvent += GameOverUI_OnGameQuitEvent;
    }

    private void GameOverUI_OnGameQuitEvent(object sender, EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(false);
    }

    private void GameOverUI_OnGameOverEvent(object sender, System.EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(true);

        int playerScore = GameManager.Instance.GetScore();

        scoreText.SetText("Score: " + playerScore.ToString());

        PlayFabController.Instance.UpdateScoreTextFieldsNewScore(playerScore, rankedListText, scoreListText, nameListText);
    }

    private void GameOverUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(false);
    }

    private void SaveAndQuit() {
        int playerScore = GameManager.Instance.GetScore();
        string playerInitial = playerInitialInputField.text;
        if (playerInitial.Length > 3) {
            playerInitial = playerInitial.Substring(0, 3);
        }

        PlayFabController.Instance.SaveHighScore(playerScore, playerInitial);

        GameManager.Instance.QuitGame();
    }
}
