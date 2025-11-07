using UnityEngine;
using System.Collections;


public class Bullet : MonoBehaviour
{

    Health playerHealth;
    public string targetTag = "Enemy";
    public float speed = 10f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = Object.FindFirstObjectByType<Health>();
    }

    private void Update()
    {
        // Destroy bullet if too far
        if (transform.position.magnitude > 20f)
            Destroy(gameObject);
    }

    //simple delete on collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (targetTag == "Player" && other.gameObject.CompareTag(targetTag))
        {
            playerHealth.takeDamage(1);
            Destroy(this.gameObject);
            return;
        }

        // to avoid killing other bullets or player
        if (other.gameObject.CompareTag(targetTag))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        Destroy(this.gameObject);
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