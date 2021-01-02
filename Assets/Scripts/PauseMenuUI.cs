using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour {
    [SerializeField] private Transform pauseMenuUIHolder;

    [SerializeField] private Button exitButton;

    [SerializeField] private Button restartButton;

    private void Awake() {
        pauseMenuUIHolder.gameObject.SetActive(false);
        exitButton.onClick.AddListener(delegate { Application.Quit(); });
        restartButton.onClick.AddListener(delegate { GameManager.Instance.SetUpGame(); });
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += PauseMenuUI_OnGameSetupEvent;
    }

    private void PauseMenuUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        pauseMenuUIHolder.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            bool isPaused = !GameManager.Instance.IsPaused();
            pauseMenuUIHolder.gameObject.SetActive(isPaused);
            GameManager.Instance.Pause(isPaused);
        }
    }
}
