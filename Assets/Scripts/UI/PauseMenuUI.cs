using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour {

    public static bool IsGamePaused;
    public static bool IsPauseable;

    [Header("Transform Holder")]
    [SerializeField] private Transform pauseMenuUIHolder;

    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;

    private void Awake() {
        IsGamePaused = true;
        IsPauseable = false;

        pauseMenuUIHolder.gameObject.SetActive(false);

        exitButton.onClick.AddListener(delegate { GameManager.Instance.CleanupGame(); });
        restartButton.onClick.AddListener(delegate { GameManager.Instance.SetupGame(); });
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += PauseMenuUI_OnGameSetupEvent;
        GameManager.Instance.OnGameCleanupEvent += PauseMenuUI_OnGameCleanupEvent;
        GameManager.Instance.OnLevelStartEvent += PauseMenuUI_OnLevelStartEvent;
        GameManager.Instance.OnLevelEndEvent += PauseMenuUI_OnLevelEndEvent;
    }

    private void PauseMenuUI_OnLevelEndEvent(object sender, EventArgs e) {
        IsPauseable = false;
    }

    private void PauseMenuUI_OnLevelStartEvent(object sender, EventArgs e) {
        IsPauseable = true;
    }

    private void PauseMenuUI_OnGameCleanupEvent(object sender, EventArgs e) {
        pauseMenuUIHolder.gameObject.SetActive(false);
        IsGamePaused = true;
        IsPauseable = false;
    }

    private void PauseMenuUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        pauseMenuUIHolder.gameObject.SetActive(false);
        IsGamePaused = true;
        IsPauseable = false;
    }

    private void Update() {
        if (IsPauseable) {
            if (Input.GetKeyDown(KeyCode.P)) {
                IsGamePaused = !IsGamePaused;
                // bool isPaused = !GameManager.Instance.IsPaused();
                pauseMenuUIHolder.gameObject.SetActive(IsGamePaused);
                // GameManager.Instance.Pause(isPaused);
            }
        }
    }

    public static void PauseGame() {
        IsGamePaused = true;
    }

    public static void UnPauseGame() {
        IsGamePaused = false;
    }
}
