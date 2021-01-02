using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Transform gameOverUIHolder;

    [SerializeField] private Button restartButton;

    private void Awake() {
        gameOverUIHolder.gameObject.SetActive(false);
    }

    private void Start() {
        GameManager.Instance.OnGameOverEvent += GameOverUI_OnGameOverEvent;
        GameManager.Instance.OnGameSetupEvent += GameOverUI_OnGameSetupEvent;

        restartButton.onClick.AddListener(delegate { GameManager.Instance.SetUpGame(); });
    }

    private void GameOverUI_OnGameOverEvent(object sender, System.EventArgs e) {
        Debug.Log("test1");

        gameOverUIHolder.gameObject.SetActive(true);

        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }

    private void GameOverUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(false);
    }
}
