using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Transform gameOverUIHolder;

    private void Awake() {
        gameOverUIHolder.gameObject.SetActive(false);
    }

    private void Start() {
        GameManager.Instance.OnGameOverEvent += GameOverUI_OnGameOverEvent;
    }

    private void GameOverUI_OnGameOverEvent(object sender, System.EventArgs e) {
        gameOverUIHolder.gameObject.SetActive(true);

        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }
}
