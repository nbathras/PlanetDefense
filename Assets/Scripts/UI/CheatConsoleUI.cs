using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
            cheatConsoleUIHolder.gameObject.SetActive(!cheatConsoleUIHolder.gameObject.activeSelf);
        }
    }

    private void EnterCheatCode() {
        if (cheatConsoleButton.gameObject.activeSelf) {
            string cheatCode = cheatConsoleInputField.text;

            if (cheatCode == "KILL_ALL") {
                List<City> cityList = CityController.Instance.GetCities();
                while (cityList.Count > 0) {
                    Destroy(cityList[0].gameObject);
                }
            }
        }
    }
}
