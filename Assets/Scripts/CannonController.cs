using UnityEngine;

public class CannonController : MonoBehaviour {

    [SerializeField] private Transform projectileSpawnPosition;

    private Camera mainCamera;

    private void Start() {
        // Cached main camera
        mainCamera = Camera.main;
    }
    
    private void Update() {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);

        // Fire Cannonball
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Projectile.Create(projectileSpawnPosition.position, mousePosition);
        }
    }
}
