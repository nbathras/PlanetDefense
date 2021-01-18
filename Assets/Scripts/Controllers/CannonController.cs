using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

    public static CannonController Instance;

    public event EventHandler OnAmmoAmountChangedEvent;

    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int ammoCountStart;
    private int ammoCount;

    [SerializeField] private float laserReloadTimerMax;
    private float laserReloadTimer;
    [SerializeField] private float laserTimerMax;
    private float laserTimer;

    [SerializeField] private GameObject laser;
    [SerializeField] private SpriteRenderer laserChargeSpriteRenderer;
    [SerializeField] private List<Sprite> laserSpriteList;

    private Camera mainCamera;

    private List<Projectile> firedProjectileList;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        // Cached main camera
        mainCamera = Camera.main;
    }
    
    private void Update() {
        if (!PauseMenuUI.IsGamePaused) {
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

                // Fire laser
                if (Input.GetKeyDown(KeyCode.Mouse1) && laserReloadTimer < 0f) {
                    laser.gameObject.SetActive(true);
                    laserReloadTimer = laserReloadTimerMax;
                    laserTimer = laserTimerMax;
                    SetLaserSprite();
                }
            }

            if (laserReloadTimer >= 0f) {
                laserReloadTimer -= Time.deltaTime;
                SetLaserSprite();
            }

            if (laserTimer >= 0f) {
                laserTimer -= Time.deltaTime;
            } else if (laserTimer < 0 && laser.gameObject.activeSelf) {
                laser.gameObject.SetActive(false);
            }
        }
    }

    private void SetLaserSprite() {
        float percentage = Mathf.Clamp(1f - (laserReloadTimer / laserReloadTimerMax), 0f, 1f);
        int index = Mathf.RoundToInt((laserSpriteList.Count - 1) * percentage);
        laserChargeSpriteRenderer.sprite = laserSpriteList[index];
    }

    public void Setup() {
        Cleanup();

        SetAmountCount(ammoCountStart);

        firedProjectileList = new List<Projectile>();
    }

    public void SetupLevel() {
        Cleanup();
        SetAmountCount(ammoCount + ammoCountStart);
        firedProjectileList = new List<Projectile>();

        laser.gameObject.SetActive(false);
        laserReloadTimer = -1;
        laserTimer = -1;
        SetLaserSprite();
    }
    
    public void StartLevel() {
        // Do Nothing
    }

    public void Cleanup() {
        if (firedProjectileList != null) {
            for (int i = 0; i < firedProjectileList.Count; i++) {
                if (firedProjectileList[i]) {
                    Destroy(firedProjectileList[i].gameObject);
                }
            }

            firedProjectileList = null;
        }
    }

    public void SetAmountCount(int ammoCount) {
        this.ammoCount = ammoCount;

        OnAmmoAmountChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public int GetAmmoCount() {
        return ammoCount;
    }

    public bool RemoveProjectile(Projectile projectile) {
        if (projectile == null) {
            throw new Exception("Error: Attempted to destory an asteroid with a null references");
        }
        if (!firedProjectileList.Remove(projectile)) {
            throw new Exception("Error: Attempted to destory an asteroid not in asteroid list");
        }

        return true;
    }
} 
