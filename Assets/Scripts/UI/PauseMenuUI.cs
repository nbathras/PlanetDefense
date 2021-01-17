﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour {
    [Header("Transform Holder")]
    [SerializeField] private Transform pauseMenuUIHolder;

    [Header("Buttons")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;

    private void Awake() {
        pauseMenuUIHolder.gameObject.SetActive(false);

        exitButton.onClick.AddListener(delegate { GameManager.Instance.CleanupGame(); });
        restartButton.onClick.AddListener(delegate { GameManager.Instance.SetupGame(); });
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += PauseMenuUI_OnGameSetupEvent;
        GameManager.Instance.OnGameCleanupEvent += PauseMenuUI_OnGameCleanupEvent;
    }

    private void PauseMenuUI_OnGameCleanupEvent(object sender, EventArgs e) {
        pauseMenuUIHolder.gameObject.SetActive(false);
    }

    private void PauseMenuUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        pauseMenuUIHolder.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            bool isPaused = !GameManager.Instance.IsPaused();
            pauseMenuUIHolder.gameObject.SetActive(isPaused);
            GameManager.Instance.Pause(isPaused);
        }
    }
}
