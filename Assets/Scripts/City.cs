using System;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    public static City Create(Transform cityHolder, String name) {
        Transform pfCity = Resources.Load<Transform>("pfCity");
        Transform cityTransform = Instantiate(pfCity, cityHolder.transform.position, Quaternion.identity);
        cityTransform.SetParent(cityHolder);

        City city = cityTransform.GetComponent<City>();
        city.name = name;

        return city;
    }

    private LineRenderer lineRenderer;

    [SerializeField] private float reloadTimerMax;
    private float reloadTimer;

    private List<Reload> reloadList;

    private Vector3[] positionList;

    private void Awake() {
        reloadList = new List<Reload>();
    }

    private void Start() {
        reloadTimer = reloadTimerMax;

        lineRenderer = GetComponent<LineRenderer>();

        positionList = new Vector3[4];

        lineRenderer.startWidth = .1f;
        lineRenderer.endWidth = .1f;

        positionList[0] = transform.position;
        positionList[1] = transform.position - new Vector3(0, 0.3f, 0);
        positionList[2] = new Vector3(CannonController.Instance.transform.position.x, positionList[1].y);
        positionList[3] = CannonController.Instance.transform.position;

        int numberOfPoints = 5;
        lineRenderer.positionCount = numberOfPoints * 4;
        for (int i = 0; i < numberOfPoints; i++) {
            lineRenderer.SetPosition(numberOfPoints * 0 + i, positionList[0]);
            lineRenderer.SetPosition(numberOfPoints * 1 + i, positionList[1]);
            lineRenderer.SetPosition(numberOfPoints * 2 + i, positionList[2]);
            lineRenderer.SetPosition(numberOfPoints * 3 + i, positionList[3]);
        }
    }

    private void Update() {
        if (!PauseMenuUI.IsGamePaused) {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer < 0f) {
                reloadTimer = reloadTimerMax;
                reloadList.Add(Reload.Create(this, positionList));
            }
        }
    }

    public void RemoveReload(Reload reload) {
        if (reload == null) {
            throw new Exception("Error: Attempted to destory an reload with a null references");
        }
        if (!reloadList.Remove(reload)) {
            throw new Exception("Error: Attempted to destory an reload not in asteroid list");
        }
    }

    public void OnDestroy() {
        CityController.Instance.RemoveCity(this);
    }
}
