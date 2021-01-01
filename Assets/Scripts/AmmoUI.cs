using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour {

    [SerializeField] private Transform ammoUIWorldTransform;

    [SerializeField] private TextMeshProUGUI ammoText;

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void Start() {
        CannonController.Instance.OnAmmoAmountChangedEvent += AmmoUI_OnAmmoAmountChangedEvent;

        ammoText.SetText(CannonController.Instance.GetAmmoCount().ToString());
    }

    private void Update() {
        Vector3 ammoUIScreenPosition = mainCamera.WorldToScreenPoint(ammoUIWorldTransform.position) + (new Vector3(0, -10, 0));
        ammoText.transform.position = ammoUIScreenPosition;
    }

    private void AmmoUI_OnAmmoAmountChangedEvent(object sender, System.EventArgs e) {
        ammoText.SetText(CannonController.Instance.GetAmmoCount().ToString());
    }
}
