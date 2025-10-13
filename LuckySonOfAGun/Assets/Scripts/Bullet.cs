using UnityEngine;
using System.Collections;


public class Bullet : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float speed = 10f;
    public float rotateSpeed = 200f; // how fast the bullet can turn
    private Rigidbody2D rb;
    private Transform target;
    private bool isHoming = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Roll the die
        int roll = DiceRoll.Instance.RollDie(6);
        Debug.Log("Bullet Roll: " + roll);

        // trigger UI animation
        GameUI gameUI = FindFirstObjectByType<GameUI>();
        if (gameUI != null)
            gameUI.PlayerShooting();

        // If roll == 6, find and lock onto the closest target and just ABSOLUTELY home in on it
        if (roll == 6)
        {
            speed = 30f; // go faster
            Debug.Log("Critical Roll!");
            GameObject closest = FindClosestObjectWithTag(targetTag);
            if (closest != null)
            {
                target = closest.transform;
                isHoming = true;
                Debug.Log("Homing in on: " + target.name);
            }
        }
    }

    // to fix bullet jitter after changing Time.timeScale
    private IEnumerator Start()
    {
        // Wait one fixed physics frame so Time.timeScale changes take effect
        yield return new WaitForFixedUpdate();

        // Now safely set velocity
        rb.linearVelocity = transform.up * speed * Time.timeScale;
    }

    private void FixedUpdate()
    {
        if (isHoming && target != null)
        {
            // direction to target
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            // calculate rotation towards the target
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // rotate gradually toward target
            rb.angularVelocity = -rotateAmount * rotateSpeed;

            // keep moving forward
            rb.linearVelocity = transform.up * speed * Time.timeScale;
        }
        else
        {
            // straight shot if not homing
            rb.linearVelocity = transform.up * speed;
        }

        // Destroy bullet if too far
        if (transform.position.magnitude > 20f)
            Destroy(gameObject);
    }

    //simple delete on collision
    private void OnCollisionEnter2D(Collision2D other)
    {

        // to avoid killing other bullets or player
        if (other.gameObject.CompareTag(targetTag))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    //find the closest enemy or whatever tag is passed in
    private GameObject FindClosestObjectWithTag(string tag)
    {
        //array of all objects with the tag
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        //distances and current
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        //loop through all objects and find the closest one
        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(obj.transform.position, currentPosition);
            if (distance < minDistance)
            {
                closest = obj;
                minDistance = distance;
            }
        }
        return closest;
    }
}