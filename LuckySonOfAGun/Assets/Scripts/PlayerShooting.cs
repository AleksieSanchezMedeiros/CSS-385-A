using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] prefabBullets = new GameObject[2];

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

    public Health health;

    private AudioSource _audioSource;

    //6 powerups:
    //1: poop gun - shoots poop that deal less damage but apply a slowing effect on enemies
    //2: plus dmg plus speed - increases bullet damage and speed and decreases cooldown
    //3: homing bullets - bullets home in on nearest enemy
    //4: heal - heal up a bit of health, shot gun - shoots 3 bullets in a spread
    //5: splitting bullets - bullets split into 2 smaller bullets on impact
    //6: explosive bullets - bullets explode on impact dealing area damage

    // Update is called once per frame

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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
        _audioSource.Play();
        int numBullets = 1;
        switch (roll)
        {
            case 1:
                //heal gun
                //change bullet prefab to reg bullet
                health.healDamage(1);
                bulletPrefab = prefabBullets[1];
                break;
            case 2:
                //plus dmg plus speed
                bulletSpeed = 30f;
                shootCooldown = 0.1f;
                break;
            case 4:
                //heal
                //heal the player for 1 health;
                //shot gun
                numBullets = 4;
                break;
            case 0:
                //normal bullet
                bulletSpeed = 20f;
                shootCooldown = 0.5f;
                bulletPrefab = prefabBullets[1];
                break;
            default:
                //normal bullet
                bulletSpeed = 20f;
                shootCooldown = 0.5f;
                bulletPrefab = prefabBullets[1];
                break;
        }

        float spreadAngle = 15f;

        for (int i = 0; i < numBullets; i++)
        {
            Quaternion bulletRot = firePoint.rotation;

            if (i != 0)
            {
                float randomSpread = Random.Range(-spreadAngle, spreadAngle);
                bulletRot = Quaternion.Euler(0, 0, randomSpread) * firePoint.rotation;
            }

            GameObject currBullet = Instantiate(bulletPrefab, firePoint.position, bulletRot);
            currBullet.GetComponent<Rigidbody2D>().linearVelocity = currBullet.transform.up * bulletSpeed;
            currBullet.GetComponent<Bullet>().SetRoll(roll);
            Debug.Log("Player shot a bullet with powerup roll " + roll);
        }
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
