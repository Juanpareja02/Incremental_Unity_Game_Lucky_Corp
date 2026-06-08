using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickInteractor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private EconomySystem economy;

    [Header("Raycast")]
    [SerializeField] private float range = 6f;
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private LayerMask coreMask;
    [SerializeField] private HandPunch handPunch;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
        if (!economy) economy = FindAnyObjectByType<EconomySystem>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hitCoin, range, pickupMask))
        {
            var coin = hitCoin.collider.GetComponentInParent<CoinPickup>();
            if (coin != null)
            {
                coin.PickUp(economy);
                handPunch?.Play();
                return;
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hitCore, range, coreMask))
        {
            var source = hitCore.collider.GetComponent<CoinSource>();
            if (source != null)
                source.Collect(economy);
        }
    }
}
