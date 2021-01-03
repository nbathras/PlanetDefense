using System;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public static CityController Instance;

    public event EventHandler OnDestoryCityEvent;

    [SerializeField] private List<Transform> cityHolderList;
    private List<City> cityList;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        Pause(true);
    }

    public void Setup() {
        Cleanup();
        Pause(false);

        cityList = new List<City>();
        for (int i = 0; i < cityHolderList.Count; i++) {
            cityList.Add(City.Create(cityHolderList[i], "City " + i.ToString()));
        }
    }

    public void Cleanup() {
        if (cityList != null) {
            for (int i = 0; i < cityList.Count; i++) {
                if (cityList[i] != null) {
                    Destroy(cityList[i].gameObject);
                }
            }

            cityList = null;
        }
        Pause(true);
    }

    public void Pause(bool isPaused) {
        enabled = !isPaused;
        if (cityList != null) {
            for (int i = 0; i < cityList.Count; i++) {
                if (cityList[i] != null) {
                    cityList[i].Pause(isPaused);
                }
            }
        }
    }

    public int GetNumberAliveCities() {
        return cityList.Count;
    }

    public List<City> GetCities() {
        return cityList;
    }

    public int GetScore() {
        return cityList.Count * 100;
    }

    public bool DestoryCity(City city) {
        if (city == null) {
            throw new Exception("Error: Attempted to destory an city with a null references");
        }
        if (!cityList.Remove(city)) {
            throw new Exception("Error: Attempted to remove an city not in city list");
        }

        Destroy(city.gameObject);

        OnDestoryCityEvent?.Invoke(this, EventArgs.Empty);

        return true;
    }
}
