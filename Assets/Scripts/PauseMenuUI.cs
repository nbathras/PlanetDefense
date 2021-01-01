using UnityEngine;

public class PauseMenuUI : MonoBehaviour {
    [SerializeField] private Transform pauseMenuUIHolder;

    private void Awake() {
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
