using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody2D rb;
    Vector2 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // movement for player
        movement = new Vector2(moveHorizontal, moveVertical);
    }

    void FixedUpdate()
    {
        // Use physics movement in FixedUpdate for smooth collision handling
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
