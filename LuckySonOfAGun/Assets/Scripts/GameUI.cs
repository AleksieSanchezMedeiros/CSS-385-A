using UnityEngine;
using TMPro;
using System.Collections;

public class GameUI : MonoBehaviour
{
    //Scripts
    //health
    //bullets
    [Header("Player Scripts")]
    [SerializeField] private PlayerShooting playerShooting;

    //UI Elements
    [Header("Player UI Elements")]
    [SerializeField] private GameObject[] ammoUIElements = new GameObject[6]; //list of bullet images


    [Header("Pause Menu Elements")]
    [SerializeField] private GameObject pauseMenuBg;
    [SerializeField] private GameObject pauseMenuTitleText;
    [SerializeField] private GameObject saveGameButton, loadGameButton;

    void Start()
    {
        pauseMenuBg.SetActive(false);
        pauseMenuTitleText.SetActive(false);
        saveGameButton.SetActive(false);
        loadGameButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        UpdateAmmoUI();
    }

    void PauseGame()
    {
        // Toggle pause state
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f; // Pause the game
            pauseMenuBg.SetActive(true);
            pauseMenuTitleText.SetActive(true);
            saveGameButton.SetActive(true);
            loadGameButton.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            pauseMenuBg.SetActive(false);
            pauseMenuTitleText.SetActive(false);
            saveGameButton.SetActive(false);
            loadGameButton.SetActive(false);
        }
    }

    //use the ammo count from player shooting to update the UI
    //and then enable/disable the bullet images depending on their location in list
    void UpdateAmmoUI()
    {
        int currentAmmo = playerShooting.ammoCount;

        for (int i = 0; i < 6; i++)
        {
            if (i < currentAmmo)
            {
                ammoUIElements[i].SetActive(true);
            }
            else
            {
                ammoUIElements[i].SetActive(false);
            }
        }
    }

    void OnReloading()
    {

    }
}