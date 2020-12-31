using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start() {
        GameManager.Instance.OnScoreChangedEvent += ScoreUI_OnScoreChangedEvent;

        UpdateScoreText();
    }

    private void ScoreUI_OnScoreChangedEvent(object sender, System.EventArgs e) {
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }
}
