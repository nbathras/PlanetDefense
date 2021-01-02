using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

    public static CannonController Instance;

    public event EventHandler OnAmmoAmountChangedEvent;

    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int ammoCountStart;
    private int ammoCount;

    private Camera mainCamera;

    private List<Projectile> firedProjectileList;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        Pause(true);
    }

    private void Start() {
        // Cached main camera
        mainCamera = Camera.main;
    }
    
    private void Update() {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (angle > 2 && angle < 178) {
            transform.eulerAngles = new Vector3(0, 0, angle - 90);

            // Fire Cannonball
            if (Input.GetKeyDown(KeyCode.Mouse0) && ammoCount > 0) {
                SetAmountCount(ammoCount - 1);
                firedProjectileList.Add(Projectile.Create(projectileSpawnPosition.position, mousePosition));
            }
        }
    }

    public void Setup() {
        Cleanup();
        Pause(false);

        SetAmountCount(ammoCountStart);

        firedProjectileList = new List<Projectile>();
    }

    public void Cleanup() {
        if (firedProjectileList != null) {
            for (int i = 0; i < firedProjectileList.Count; i++) {
                Destroy(firedProjectileList[i].gameObject);
            }

            firedProjectileList = null;
        }
        Pause(true);
    }

    public void Pause(bool isPaused) {
        Instance.enabled = !isPaused;
        if (firedProjectileList != null) {
            for (int i = 0; i < firedProjectileList.Count; i++) {
                if (firedProjectileList[i]) {
                    firedProjectileList[i].enabled = !isPaused;
                }
            }
        }
    }

    public void SetAmountCount(int ammoCount) {
        this.ammoCount = ammoCount;

        OnAmmoAmountChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public int GetAmmoCount() {
        return ammoCount;
    }

    public bool DestroyProjectile(Projectile projectile) {
        if (projectile == null) {
            throw new Exception("Error: Attempted to destory an asteroid with a null references");
        }
        if (!firedProjectileList.Remove(projectile)) {
            throw new Exception("Error: Attempted to destory an asteroid not in asteroid list");
        }

        Destroy(projectile.gameObject);

        return true;
    }
} 
