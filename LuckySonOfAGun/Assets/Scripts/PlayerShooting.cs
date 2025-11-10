using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    public bool onCooldown = false;
    public float shootCooldown = 0.5f; // half a second between shots

    public int ammoCount = 6;
    public int maxAmmo = 6;
    public float reloadTime = 2f; // time to reload

    public bool onReloadCooldown;

    public int roll = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return; // do nothing if the game is paused
        }

        //rotate to look at mouse position in 2d
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get it in 3d

        // turn it into 2d space
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = direction;


        // shoot bullet on left mouse click
        if (Input.GetButtonDown("Fire1") && !onCooldown && !onReloadCooldown)
        {
            if (ammoCount == 0)
            {
                //force reload
                Reload();
                onReloadCooldown = true;
                return;
            }

            if (ammoCount > 0)
            {
                ammoCount--;
                Shoot();
                // start cooldown
                onCooldown = true;
                Invoke("ResetCooldown", shootCooldown);
            }
        }

        //if r pressed and not full ammo and not already reloading, reload
        if (Input.GetKeyDown(KeyCode.R) && ammoCount < maxAmmo && !onReloadCooldown)
        {
            if (ammoCount > 0)
            {
                //give the player a shorter reload if they have some ammo left
                reloadTime = 1.0f;
            }
            Reload();
            onReloadCooldown = true;
        }

    }

    private void Shoot()
    {
        // instantiate a bullet prefab at the player's position and make it move
        GameObject currBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        currBullet.GetComponent<Rigidbody2D>().linearVelocity = firePoint.up * bulletSpeed;
    }

    private void Reload()
    {
        // simple reload that just resets ammo after a delay
        Invoke("FinishReload", reloadTime);
    }

    private void FinishReload()
    {
        ammoCount = maxAmmo;
        onReloadCooldown = false;
        reloadTime = 2f; // reset reload time
    }

    private void ResetCooldown()
    {
        onCooldown = false;
    }
}
