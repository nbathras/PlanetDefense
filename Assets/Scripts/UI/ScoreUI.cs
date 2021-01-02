using UnityEngine;
using TMPro;
using System;

/* UI systems have the following structure
 * 
 * GameObject Structure 
 *
 * ParentUIObject (UI script component)
 * ==> HolderObject (used to activate and deactivate element without turning off script)
 * ====> All Remaining UI elements
 * 
 * Script Structure
 * 
 * Awake() {
 *   1. Set active or inactive at start of game
 *   2. Add event listeners for buttons
 * }
 * 
 * Start () {
 *   1. Add event handlers for unity events that rely on singeltons instances from controllers
 * }
 */
public class ScoreUI : MonoBehaviour {
    [Header("Transform Holder")]
    [SerializeField] private Transform scoreUIHolder;

    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake() {
        scoreUIHolder.gameObject.SetActive(false);
    }

    private void Start() {
        GameManager.Instance.OnScoreChangedEvent += ScoreUI_OnScoreChangedEvent;
        GameManager.Instance.OnGameSetupEvent += ScoreUI_OnGameSetupEvent;
        GameManager.Instance.OnGameQuitEvent += ScoreUI_OnGameQuitEvent;
    }

    private void ScoreUI_OnGameQuitEvent(object sender, EventArgs e) {
        scoreUIHolder.gameObject.SetActive(false);
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
