using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public string targetTag = "Enemy";
    public float speed = 10f;
    private Rigidbody2D rb;

    int currentPowerUpRoll = 0;

    RoundManager rm;

    public GameObject bulletPrefab;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        rm = GameObject.FindFirstObjectByType<RoundManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (currentPowerUpRoll == 3)
        {
            //homing
            GameObject target = FindClosestObjectWithTag("Enemy");
            if (target != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }

        // Destroy bullet if too far
        if (transform.position.magnitude > 20f)
            Destroy(gameObject);
    }

    public void SetRoll(int roll)
    {
        currentPowerUpRoll = roll;
    }

    //simple delete on collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (currentPowerUpRoll)
        {
            case 5:
                {
                    GameObject target = FindClosestObjectWithTag("Enemy");
                    if (target == null) break;

                    Vector2 dir = (target.transform.position - transform.position).normalized;

                    float spread = 25f;

                    Vector2 dirLeft = Quaternion.Euler(0, 0, -spread) * dir;
                    Vector2 dirMid = dir;
                    Vector2 dirRight = Quaternion.Euler(0, 0, spread) * dir;

                    Vector2[] dirs = { dirLeft, dirMid, dirRight };

                    foreach (Vector2 d in dirs)
                    {
                        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        b.GetComponent<Rigidbody2D>().linearVelocity = d * speed;
                        b.GetComponent<Bullet>().SetRoll(0);
                    }

                    Debug.Log("Bullet Split!");
                }
                break;
            case 6:
                //explosive bullets
                //bullets spawn around the point of impact
                _audioSource.Play();
                for (int i = 0; i < 8; i++)
                {
                    float angle = i * Mathf.PI / 4; // 45 degrees in radians
                    Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 0.5f;
                    GameObject currBulletExp = Instantiate(gameObject, spawnPosition, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));
                    currBulletExp.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                    currBulletExp.GetComponent<Bullet>().SetRoll(0);
                }
                break;
        }

        if (currentPowerUpRoll == 1)
        {
            other.gameObject.GetComponent<Enemy>().SlowDown();
            Destroy(gameObject);  // destroy bullet
            return;               // DO NOT kill enemy or score
        }

        // to avoid killing other bullets or player
        if (other.gameObject.CompareTag(targetTag))
        {
            rm.score += other.gameObject.GetComponent<Enemy>().scoreValue;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            return;
        }

        Debug.Log("Bullet hit non-target object: " + other.gameObject.name);
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