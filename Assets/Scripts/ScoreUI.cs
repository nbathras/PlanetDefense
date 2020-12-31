using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake() {
        GameManager.Instance.OnScoreChanged += ScoreUI_OnScoreChanged;
    }

    private void Start() {
        UpdateScoreText();
    }

    private void ScoreUI_OnScoreChanged(object sender, System.EventArgs e) {
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }
}
