using System.Collections.Generic;
using UnityEngine;

public class Alien : Enemy {

    public static Alien Create(string name) {
        Transform pfAlien = Resources.Load<Transform>("pfAlien");

        Vector3 spawnPosition;
        Vector3 targetPostion;
        if (Random.Range(0, 100) < 50) {
            // left side
            spawnPosition = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)) + new Vector3(-1f, -1f, 0f);
            targetPostion = spawnPosition + new Vector3(3f, 0f, 0f);
        } else {
            // right side
            spawnPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) + new Vector3(1f, -1f, 0f);
            targetPostion = spawnPosition + new Vector3(-3f, 0f, 0f);
        }
        spawnPosition.z = 0;
        targetPostion.z = 0;

        Transform alienTransform = Instantiate(pfAlien, spawnPosition, Quaternion.identity);
        Alien alien = alienTransform.GetComponent<Alien>();

        // alien.leftMovePosition = leftMovePosition;
        // alien.rightMovePosition = rightMovePosition;
        alien.targetPostion = targetPostion;
        alien.normalizedDirection = (targetPostion - spawnPosition).normalized;
        alien.yTop = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - 1;
        alien.yBottom = alien.yTop - 1.5f;
        alien.xLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).x + 1;
        alien.xRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - 1;
        alien.name = name;

        return alien;
    }

    private float yTop;
    private float yBottom;
    private float xLeft;
    private float xRight;

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
                SelectTarget();
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
        EnemySpawnerController.Instance.RemoveEnemy(this);
    }

    private void SelectTarget() {
        int vChoice = Random.Range(0, 100);
        if (vChoice < 50) {
            targetPostion.y = yTop;
        } else {
            targetPostion.y = yBottom;
        }

        int hChoice = Random.Range(0, 100);
        if (hChoice < 50) {
            targetPostion.x += 1.5f;
            if (targetPostion.x > xRight) {
                targetPostion.x -= 3f;
            }
        } else {
            targetPostion.x -= 1.5f;
            if (targetPostion.x < xLeft) {
                targetPostion.x += 3f;
            }
        }
        targetPostion.z = 0;
        normalizedDirection = (targetPostion - transform.position).normalized;
    }
}
