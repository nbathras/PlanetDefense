using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour {

    [SerializeField] private Transform holder;

    [SerializeField] private TextMeshProUGUI countDownText;

    [SerializeField] private float countDownTimerMax;
    private float countDownTimer;

    [SerializeField] private int minFontSize;
    [SerializeField] private int maxFontSize;

    private int countDown;

    private bool isCountDown;

    private void Awake() {
        holder.gameObject.SetActive(false);
        isCountDown = false;
    }

    private void Update() {
        if (isCountDown) {
            if (countDown <= -1) {
                isCountDown = false;
                holder.gameObject.SetActive(false);
            }

            countDownTimer -= Time.deltaTime;

            float difference = maxFontSize - minFontSize;
            float percentage = 1 - (countDownTimer / countDownTimerMax);
            float updatedPercentage = ((-1f / (percentage - 1.2f)) - .83f) / 4.2f;
            Debug.Log("x: " + percentage.ToString() + " | y:" + updatedPercentage.ToString());
            float newFontSize = maxFontSize - difference * updatedPercentage;
            countDownText.fontSize = newFontSize;

            if (countDownTimer < 0f) {
                countDown -= 1;
                if (countDown == 0) {
                    countDownText.SetText("Incoming!!!");
                } else {
                    countDownText.SetText(countDown.ToString());
                }
                countDownTimer += countDownTimerMax;
                countDownText.fontSize = maxFontSize;
            }
        }
    }

    public void StartLevelCountdown() {
        isCountDown = true;
        holder.gameObject.SetActive(true);

        countDown = 3;
        countDownText.SetText(countDown.ToString());
        countDownText.fontSize = maxFontSize;
        countDownTimer = countDownTimerMax;
    }
}
