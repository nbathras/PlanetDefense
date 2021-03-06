﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheatConsoleUI : MonoBehaviour {
    [SerializeField] private Transform cheatConsoleUIHolder;

    [SerializeField] private TMP_InputField cheatConsoleInputField;
    [SerializeField] private Button cheatConsoleButton;

    private void Awake() {
        cheatConsoleUIHolder.gameObject.SetActive(false);

        cheatConsoleButton.onClick.AddListener(delegate { EnterCheatCode(); });
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            if (cheatConsoleUIHolder.gameObject.activeSelf) {
                Disable();
            } else {
                Enable();
            }
        }
    }

    private void Disable() {
        cheatConsoleUIHolder.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Enable() {
        cheatConsoleUIHolder.gameObject.SetActive(true);
        cheatConsoleInputField.Select();
    }

    private void EnterCheatCode() {
        if (cheatConsoleButton.gameObject.activeSelf) {
            string cheatCode = cheatConsoleInputField.text;

            if (cheatCode == "c") {
                List<City> cityList = CityController.Instance.GetCities();
                while (cityList.Count > 0) {
                    Destroy(cityList[0].gameObject);
                }
            }

            if (cheatCode == "a1") {
                EnemySpawnerController.Instance.SpawnAsteroid();
            }

            if (cheatCode == "a2") {
                EnemySpawnerController.Instance.SpawnAlien();
            }

            if (cheatCode == "a3") {
                EnemySpawnerController.Instance.SpawnSpliner();
            }

            if (cheatCode == "el") {
                GameManager.Instance.EndLevel();
            }
        }
    }
}
