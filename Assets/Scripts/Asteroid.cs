﻿using UnityEngine;

public class Asteroid : MonoBehaviour {
    public static Asteroid Create(Vector3 spawnPosition, Vector3 direction) {
        Transform pfAsteroid = Resources.Load<Transform>("pfAsteroid");
        Transform asteroidTransform = Instantiate(pfAsteroid, spawnPosition, Quaternion.identity);

        Asteroid asteroid = asteroidTransform.GetComponent<Asteroid>();
        Vector3 normalizedDirection = direction.normalized;

        asteroid.normalizedDirection = new Vector3(normalizedDirection.x, normalizedDirection.y, 0f);
        asteroid.spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);

        return asteroid;
    }

    [SerializeField] private float despawnTimerMax;
    private float despawnTimer;

    [SerializeField] private float speed;

    private Vector3 spawnPosition;
    private Vector3 normalizedDirection;
    private LineRenderer lineRenderer;

    private void Awake() {
        despawnTimer = despawnTimerMax;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = .02f;
        lineRenderer.endWidth = .02f;
    }

    private void Update() {
        transform.position += normalizedDirection * speed * Time.deltaTime;
        lineRenderer.SetPosition(0, spawnPosition);
        lineRenderer.SetPosition(1, transform.position);

        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile")) {
            GameManager.Instance.AddScore(1);
            Destroy();
        } else if (other.CompareTag("Ground")) {
            Destroy();
        } else if (other.CompareTag("City")) {
            GameManager.Instance.RemoveCity(1);
            Destroy(other.gameObject);
            Destroy();
        }
    }

    private void Destroy() {
        AsteroidSpawnerController.Instance.DestroyAsteroid(this);
    }
}
