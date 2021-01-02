using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform mainMenuHolder;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;

    private void Awake() {
        mainMenuHolder.gameObject.SetActive(true);

        newGameButton.onClick.AddListener(delegate { GameManager.Instance.SetUpGame(); });
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += MainMenuUI_OnGameSetupEvent;
        GameManager.Instance.OnGameQuitEvent += MainMenuUI_OnGameQuitEvent;
    }

    private void MainMenuUI_OnGameQuitEvent(object sender, EventArgs e) {
        mainMenuHolder.gameObject.SetActive(true);
    }

    private void MainMenuUI_OnGameSetupEvent(object sender, EventArgs e) {
        mainMenuHolder.gameObject.SetActive(false);
    }
}
