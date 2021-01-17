using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform mainMenuHolder;
    [SerializeField] private Transform page1Holder;
    [SerializeField] private Transform page2Holder;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button scoreButton;
    [SerializeField] private Button backButton;

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI rankedListText;
    [SerializeField] private TextMeshProUGUI scoreListText;
    [SerializeField] private TextMeshProUGUI nameListText;

    private void Awake() {
        Enable();
        DisplayManMenu();

        newGameButton.onClick.AddListener(delegate { GameManager.Instance.SetupGame(); });
        exitButton.onClick.AddListener(delegate { Application.Quit(); });
        scoreButton.onClick.AddListener(delegate { DisplayScores(); });
        backButton.onClick.AddListener(delegate { DisplayManMenu(); });
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += MainMenuUI_OnGameSetupEvent;
        GameManager.Instance.OnGameCleanupEvent += MainMenuUI_OnGameCleanupEvent;
    }

    private void MainMenuUI_OnGameCleanupEvent(object sender, EventArgs e) {
        Enable();
    }

    private void MainMenuUI_OnGameSetupEvent(object sender, EventArgs e) {
        Disable();
    }

    private void DisplayScores() {
        page1Holder.gameObject.SetActive(false);
        page2Holder.gameObject.SetActive(true);

        PlayFabController.Instance.UpdateScoreTextFields(rankedListText, scoreListText, nameListText);
    }

    private void DisplayManMenu() {
        page1Holder.gameObject.SetActive(true);
        page2Holder.gameObject.SetActive(false);
    }

    private void Disable() {
        mainMenuHolder.gameObject.SetActive(false);
    }

    private void Enable() {
        mainMenuHolder.gameObject.SetActive(true);
    }
}
