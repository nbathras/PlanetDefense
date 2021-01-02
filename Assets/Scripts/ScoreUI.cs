using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        GameManager.Instance.OnScoreChangedEvent += ScoreUI_OnScoreChangedEvent;
        GameManager.Instance.OnGameSetupEvent += ScoreUI_OnGameSetupEvent;

        UpdateScoreText();
    }

    private void ScoreUI_OnScoreChangedEvent(object sender, System.EventArgs e) {
        UpdateScoreText();
    }

    private void ScoreUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        gameObject.SetActive(true);

        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }
}
