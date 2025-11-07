using UnityEngine;
using System.Collections;

public class FodderEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    public Health playerHealth;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate the direction from the current object to the player
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Move the Rigidbody in the calculated direction
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    //when collided, check if player, deal damage to player, dont deal damage for 1 second after
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.takeDamage(1);
        }
    }
}
