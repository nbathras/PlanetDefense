using System;
using UnityEngine;

public class CannonController : MonoBehaviour {

    public static CannonController Instance;

    public event EventHandler OnAmmoAmountChangedEvent;

    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int ammoMax;
    private int ammo;

    private Camera mainCamera;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        // Cached main camera
        mainCamera = Camera.main;

        ammo = ammoMax;
        OnAmmoAmountChangedEvent?.Invoke(this, EventArgs.Empty);
    }
    
    private void Update() {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle > 2 && angle < 178) {
            transform.eulerAngles = new Vector3(0, 0, angle - 90);

            // Fire Cannonball
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                ReduceAmmountCount(1);
                Projectile.Create(projectileSpawnPosition.position, mousePosition);
            }
        }
    }

    private void ReduceAmmountCount(int a) {
        ammo -= a;

        OnAmmoAmountChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public int GetAmmo() {
        return ammo;
    }
} 
