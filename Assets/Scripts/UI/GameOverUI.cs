using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform gameOverUIHolder;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;

    [Header("Dynamic Text")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake() {
        gameOverUIHolder.gameObject.SetActive(false);

        restartButton.onClick.AddListener(delegate { GameManager.Instance.SetUpGame(); });
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

        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }

    private void GameOverUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(false);
    }
}
