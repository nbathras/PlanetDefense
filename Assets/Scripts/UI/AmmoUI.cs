using System;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform ammoUIHolder;

    [Header("Dynamic Text")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Other Parameters")]
    [SerializeField] private Transform ammoUIWorldTransform;

    private Camera mainCamera;

    private void Awake() {
        ammoUIHolder.gameObject.SetActive(false);

        mainCamera = Camera.main;
    }

    private void Start() {
        GameManager.Instance.OnGameSetupEvent += AmmoUI_OnGameSetupEvent;
        GameManager.Instance.OnGameCleanupEvent += AmmoUI_OnGameCleanupEvent;

        CannonController.Instance.OnAmmoAmountChangedEvent += AmmoUI_OnAmmoAmountChangedEvent;

        enabled = false;
    }

    private void Update() {
        Vector3 ammoUIScreenPosition = mainCamera.WorldToScreenPoint(ammoUIWorldTransform.position) + (new Vector3(0, -5, 0));
        ammoText.transform.position = ammoUIScreenPosition;
    }

    private void AmmoUI_OnGameSetupEvent(object sender, EventArgs e) {
        enabled = true;
        ammoUIHolder.gameObject.SetActive(true);

        ammoText.SetText(CannonController.Instance.GetAmmoCount().ToString());
    }

    private void AmmoUI_OnGameCleanupEvent(object sender, EventArgs e) {
        enabled = false;
        ammoUIHolder.gameObject.SetActive(false);
    }

    private void AmmoUI_OnAmmoAmountChangedEvent(object sender, EventArgs e) {
        ammoText.SetText(CannonController.Instance.GetAmmoCount().ToString());
    }
}
