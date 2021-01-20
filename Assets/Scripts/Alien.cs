using System.Collections.Generic;
using UnityEngine;

public class Alien : Enemy {

    public static Alien Create(string name) {
        Transform pfAlien = Resources.Load<Transform>("pfAlien");

        // Corner screen coordinates 
        Vector2 upperLeftScreen = new Vector2(0, Screen.height);
        Vector2 upperRightScreen = new Vector2(Screen.width, Screen.height);

        // Corner locations in world coordinates
        Vector2 leftMovePosition = Camera.main.ScreenToWorldPoint(upperLeftScreen) + new Vector3(1f, -1f);
        Vector2 rightMovePosition = Camera.main.ScreenToWorldPoint(upperRightScreen) + new Vector3(-1f, -1f);
        Vector3 targetPostion;

        Vector3 spawnPosition;
        if (Random.Range(0, 100) < 50) {
            spawnPosition = leftMovePosition + new Vector2(-2f, 0f);
            targetPostion = rightMovePosition;
        } else {
            spawnPosition = rightMovePosition + new Vector2(2f, 0f);
            targetPostion = leftMovePosition;
        }
        spawnPosition.z = 0;

        Transform alienTransform = Instantiate(pfAlien, spawnPosition, Quaternion.identity);
        Alien alien = alienTransform.GetComponent<Alien>();

        alien.leftMovePosition = leftMovePosition;
        alien.rightMovePosition = rightMovePosition;
        alien.targetPostion = targetPostion;
        alien.normalizedDirection = (targetPostion - spawnPosition).normalized;
        alien.name = name;

        return alien;
    }

    private Vector3 leftMovePosition;
    private Vector3 rightMovePosition;
    private Vector3 targetPostion;
    private Vector3 normalizedDirection;

    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject shield;

    [SerializeField] private float fireRateTimerMax;
    private float fireRateTimer;
    [SerializeField] private float shieldTimerMax;
    private float shieldTimer;

    [SerializeField] private float speed;

    private void Awake() {
        fireRateTimer = fireRateTimerMax * 1.5f;
        DeactiveShield();
    }

    private void Update() {
        if (!PauseMenuUI.IsGamePaused) {
            fireRateTimer -= Time.deltaTime;
            if (fireRateTimer < 0f) {
                PlasmaProjectile.Create(firePosition.position, ChooseTarget());
                fireRateTimer += fireRateTimerMax;
            }

            transform.position += normalizedDirection * Time.deltaTime * speed;

            if (Vector3.Distance(transform.position, targetPostion) < 0.1f) {
                if (Vector3.Distance(leftMovePosition, targetPostion) < 0.1f) {
                    targetPostion = rightMovePosition;
                } else {
                    targetPostion = leftMovePosition;
                }
                normalizedDirection = (targetPostion - transform.position).normalized;
            } 

            if (shield.activeSelf) {
                shieldTimer -= Time.deltaTime;
                if (shieldTimer < 0f) {
                    DeactiveShield();
                }
            }
        }
    }

    private void ActivateShield() {
        shield.SetActive(true);
        shieldTimer = shieldTimerMax;
    }

    private void DeactiveShield() {
        shield.SetActive(false);
        shieldTimer = -1;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile")) {
            ScoreController.Instance.AddScore(ScoreController.ScoreCategories.AsteroidsDestroyed, 50);
            Destroy(gameObject);
        }
        if (other.CompareTag("Laser")) {
            ActivateShield();
        }
    }

    private Vector3 ChooseTarget() {
        List<City> cityList = CityController.Instance.GetCities();

        int index = Random.Range(0, cityList.Count - 1);
        return cityList[index].transform.position;
    }

    private void OnDestroy() {
        EnemySpawnerController.Instance.RemoveAlien(this);
    }
}
