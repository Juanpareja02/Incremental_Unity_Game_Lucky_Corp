using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private RunManager runManager;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float interactRange = 4f;
    [SerializeField] private Vector3 openedOffset = new Vector3(0f, 2.5f, 0f);

    private bool unlocked;
    private bool opened;
    private Vector3 closedPosition;

    private void Awake()
    {
        if (!playerCamera) playerCamera = Camera.main;
        if (!targetRenderer) targetRenderer = GetComponentInChildren<Renderer>();
        closedPosition = transform.position;
        ApplyVisualState();
    }

    private void Update()
    {
        if (!unlocked || opened)
            return;

        if (Input.GetKeyDown(KeyCode.E) && IsPlayerLookingAtDoor())
            Open();
    }

    public void SetRunManager(RunManager manager)
    {
        runManager = manager;
    }

    public void SetUnlocked(bool value)
    {
        unlocked = value;
        ApplyVisualState();
    }

    private bool IsPlayerLookingAtDoor()
    {
        if (!playerCamera)
            return false;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange))
            return false;

        return hit.collider != null
               && (hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform));
    }

    private void Open()
    {
        opened = true;
        transform.position = closedPosition + openedOffset;

        var col = GetComponent<Collider>();
        if (col) col.enabled = false;

        runManager?.Win();
    }

    private void ApplyVisualState()
    {
        if (!targetRenderer)
            return;

        Color color = unlocked ? new Color(0.1f, 1f, 0.35f) : new Color(0.45f, 0.08f, 0.08f);
        var mat = targetRenderer.material;

        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);
        else
            mat.color = color;

        if (mat.HasProperty("_EmissionColor"))
            mat.SetColor("_EmissionColor", unlocked ? color * 1.8f : Color.black);
    }
}
