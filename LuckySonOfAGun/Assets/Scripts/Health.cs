using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public GameUI gui;

    public bool canDamage;
    public float damageCooldownDuration = 1f;

    public GameObject[] healthBar = new GameObject[3];

    public int health = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canDamage = true;

        foreach (GameObject healthImg in healthBar)
        {
            healthImg.SetActive(true);
        }
    }

    public void takeDamage(int amountOfDamage)
    {
        if (!canDamage) return;

        for (int i = health - 1; i >= health - amountOfDamage; i--)
        {
            healthBar[i].SetActive(false);
        }

        if ((health - amountOfDamage) <= 0)
        {
            gui.endGame();
        }

        health -= amountOfDamage;

        StartCoroutine("damageCooldown");
    }

    public void healDamage(int amountOfHeal)
    {
        int newHealth = health + amountOfHeal;

        if (newHealth > 3)
        {
            newHealth = 3;
        }

        for (int i = health - 1; i < newHealth - 1; i++)
        {
            healthBar[i].SetActive(true);
        }
    }

    IEnumerator damageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldownDuration);
        canDamage = true;
    }
}
