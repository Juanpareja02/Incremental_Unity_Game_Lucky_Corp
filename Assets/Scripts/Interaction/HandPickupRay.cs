using UnityEngine;
using UnityEngine.EventSystems;

public class HandPickupRay : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private EconomySystem economy;
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private float range = 3.5f;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, pickupMask))
        {
            var coin = hit.collider.GetComponentInParent<CoinPickup>();
            if (coin != null)
            {
                coin.PickUp(economy);
            }
        }
    }
}