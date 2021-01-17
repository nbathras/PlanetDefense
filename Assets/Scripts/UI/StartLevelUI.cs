using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class StartLevelUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform holder;

    [Header("CountDown Text")]
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private int minFontSize;
    [SerializeField] private int maxFontSize;
    [SerializeField] private List<string> countDownStrings;
    [SerializeField] private float timerMax;
    private float timer;

    private int countDownIndex;
    private bool isCountingDown;

    private void Awake() {
        Disable();
    }

    private void Start() {
        GameManager.Instance.OnLevelSetupEvent += LevelUI_OnLevelSetupEvent;
    }

    private void LevelUI_OnLevelSetupEvent(object sender, EventArgs e) {
        Enable();
        SetCountDownText(0);
    }

    private void Update() {
        if (isCountingDown) {
            timer -= Time.deltaTime;

            countDownText.fontSize = CalculateFontSize();

            if (timer < 0f) {
                countDownIndex++;
                if (countDownIndex >= countDownStrings.Count) {
                    Disable();
                    GameManager.Instance.StartLevel();
                } else {
                    SetCountDownText(countDownIndex);
                }
            }
        }
    }

    private float CalculateFontSize() {
        float maxMinDiff = maxFontSize - minFontSize;
        float percentage = 1 - (timer / timerMax);
        float scaledPercentage = ((-1f / (percentage - 1.2f)) - .83f) / 4.2f;
        float newFontSize = maxFontSize - maxMinDiff * scaledPercentage;

        return newFontSize;
    }

    private void SetCountDownText(int index) {
        countDownIndex = index;
        countDownText.SetText(countDownStrings[countDownIndex]);
        countDownText.fontSize = maxFontSize;
        timer = timerMax;
    }

    private void Disable() {
        holder.gameObject.SetActive(false);
        isCountingDown = false;
    }

    private void Enable() {
        holder.gameObject.SetActive(true);
        isCountingDown = true;
    }
}
