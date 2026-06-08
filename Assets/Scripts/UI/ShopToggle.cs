using UnityEngine;

public class ShopToggle : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private bool pauseWhenOpen;

    private void Start()
    {
        if (shopPanel) shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab) || shopPanel == null)
            return;

        bool open = !shopPanel.activeSelf;
        shopPanel.SetActive(open);

        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;

        if (pauseWhenOpen)
            Time.timeScale = open ? 0f : 1f;
    }
}
