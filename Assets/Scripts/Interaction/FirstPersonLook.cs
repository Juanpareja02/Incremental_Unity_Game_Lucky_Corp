using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] float sensitivity = 2f;
    [SerializeField] Transform playerBody;

    float xRot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!playerBody) playerBody = transform.root;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X") * sensitivity;
        float my = Input.GetAxis("Mouse Y") * sensitivity;

        xRot -= my;
        xRot = Mathf.Clamp(xRot, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        playerBody.Rotate(Vector3.up * mx);
    }
}