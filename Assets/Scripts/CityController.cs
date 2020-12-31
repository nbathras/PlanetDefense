using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public static CityController Instance;

    [SerializeField] private List<Transform> cityHolderList;
    private List<City> cityList;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        cityList = new List<City>();
    }

    public void SetupCities() {
        for (int i = 0; i < cityHolderList.Count; i++) {
            cityList.Add(City.Create(cityHolderList[i]));
        }
    }
}
