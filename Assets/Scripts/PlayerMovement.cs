using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(horizontal, 0, 0) * moveSpeed * Time.deltaTime;
    }
}