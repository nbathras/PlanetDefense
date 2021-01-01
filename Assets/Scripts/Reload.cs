using UnityEngine;

public class Reload : MonoBehaviour {
    public static Reload Create(Transform cityTransform, Vector3[] transferPositionList) {
        if (transferPositionList.Length < 2) {
            throw new System.Exception("Error: Transfer Positon List must be greater than 2");
        }

        Transform pfReload = Resources.Load<Transform>("pfReload");
        Transform reloadTransform = Instantiate(pfReload, transferPositionList[0], Quaternion.identity);
        reloadTransform.SetParent(cityTransform);

        Reload reload = reloadTransform.GetComponent<Reload>();
        reload.transferPositionList = transferPositionList;
        reload.currentIndex = 0;

        return reload;
    }

    [SerializeField] private float travelSpeed;

    private Vector3[] transferPositionList;
    private int currentIndex = 0;

    private void Update() {
        if (currentIndex == transferPositionList.Length) {
            CannonController.Instance.SetAmountCount(CannonController.Instance.GetAmmoCount() + 1);
            Destroy(gameObject);
        } else {
            Vector3 targetPosition = transferPositionList[currentIndex];
            Vector3 normalizedDirection = (targetPosition - transform.position).normalized;

            transform.position += normalizedDirection * Time.deltaTime * travelSpeed;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
                currentIndex += 1;
            }
        }
    }
}
