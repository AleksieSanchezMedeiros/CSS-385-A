using UnityEngine;
using System.Collections;

public class ShooterEnemy : Enemy
{
    public GameObject player;

    public float stopDistance = 10f;
    public float bulletSpeed = 15f;
    Rigidbody2D rb;

    public GameObject enemyBullet;

    public bool canShoot = false;
    public float shootCooldowntime = 1.5f;

    Vector2 direction;

    public float distance;

    public Transform shootPoint;
    public Transform shootPoint2;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreValue = 15;
        StartCoroutine("shootCooldown");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //stop a certain distance from the player
        distance = Vector2.Distance(player.transform.position, transform.position);

        // calculate the direction from the current object to the player
        direction = (player.transform.position - transform.position).normalized;

        if (distance > stopDistance)
        {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }

        // shoot if close enough
        if (distance < stopDistance && canShoot)
        {
            ShootPlayer();
            StartCoroutine(shootCooldown());
        }
    }

    void ShootPlayer()
    {
        float dotProduct = Vector2.Dot(direction, this.transform.right);
        if (dotProduct > 0)
        {
            GameObject currBullet = Instantiate(enemyBullet, shootPoint.position, transform.rotation);
            currBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        }
        else
        {
            GameObject currBullet = Instantiate(enemyBullet, shootPoint2.position, transform.rotation);
            currBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        }
    }

    IEnumerator shootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldowntime);
        canShoot = true;
    }
}
