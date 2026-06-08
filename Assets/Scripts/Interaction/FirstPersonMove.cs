using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    CharacterController cc;
    Vector3 vel;
    const float gravity = -9.81f;

    void Awake() => cc = GetComponent<CharacterController>();

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * speed * Time.deltaTime);

        if (cc.isGrounded && vel.y < 0) vel.y = -2f;
        vel.y += gravity * Time.deltaTime;
        cc.Move(vel * Time.deltaTime);
    }
}