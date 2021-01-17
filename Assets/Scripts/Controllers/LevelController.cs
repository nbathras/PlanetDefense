using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController Instance;

    private int level;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Setup() {
        Cleanup();
        level = 1;
    }

    public void SetupLevel() {
        // Do nothing
    }

    public void StartLevel() {
        // Do nthing
    }

    public void Cleanup() {
        level = 0;
    }

    public void NextLevel() {
        level += 1;
        GameManager.Instance.SetupLevel();
    }

    public int GetLevel() {
        return level;
    } 
}
