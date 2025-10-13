using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        //rotate to look at mouse position in 2d
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get it in 3d

        // turn it into 2d space
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = direction;


        // shoot bullet on left mouse click
        if (Input.GetButtonDown("Fire1"))
        {
            // instantiate a bullet prefab at the player's position and make it move
            GameObject currBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            currBullet.GetComponent<Rigidbody2D>().linearVelocity = firePoint.up * bulletSpeed;
        }

    }
}
