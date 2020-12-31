using UnityEngine;

public class City : MonoBehaviour {
    public static City Create(Transform cityHolder) {
        Transform pfCity = Resources.Load<Transform>("pfCity");
        Transform cityTransform = Instantiate(pfCity, cityHolder.transform.position, Quaternion.identity);
        cityTransform.SetParent(cityHolder);

        City city = cityTransform.GetComponent<City>();

        return city;
    }
}
