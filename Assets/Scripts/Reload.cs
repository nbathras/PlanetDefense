using UnityEngine;

public class Reload : MonoBehaviour {
    public static Reload Create(City spawnCity, Vector3[] transferPositionList) {
        if (transferPositionList.Length < 2) {
            throw new System.Exception("Error: Transfer Positon List must be greater than 2");
        }

        Transform pfReload = Resources.Load<Transform>("pfReload");
        Transform reloadTransform = Instantiate(pfReload, transferPositionList[0], Quaternion.identity);
        reloadTransform.SetParent(spawnCity.transform);

        Reload reload = reloadTransform.GetComponent<Reload>();
        reload.transferPositionList = transferPositionList;
        reload.spawnCity = spawnCity;
        reload.currentIndex = 0;

        return reload;
    }

    [SerializeField] private float travelSpeed;

    private City spawnCity;
    private Vector3[] transferPositionList;
    private int currentIndex = 0;

    private void Update() {
        if (currentIndex == transferPositionList.Length) {
            CannonController.Instance.SetAmountCount(CannonController.Instance.GetAmmoCount() + 1);
            spawnCity.DestoryReload(this);
        } else {
            Vector3 targetPosition = transferPositionList[currentIndex];
            Vector3 normalizedDirection = (targetPosition - transform.position).normalized;

            transform.position += normalizedDirection * Time.deltaTime * travelSpeed;

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
                currentIndex += 1;
            }
        }
    }
}
