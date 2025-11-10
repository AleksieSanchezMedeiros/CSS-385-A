using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    //Scripts
    //health
    //bullets
    [Header("Player Scripts")]
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerMovement playerMovement;

    //UI Elements
    [Header("Player UI Elements")]
    [SerializeField] private GameObject[] ammoUIElements = new GameObject[6]; //list of bullet images


    [Header("Pause Menu Elements")]
    [SerializeField] private GameObject pauseMenuBg;
    [SerializeField] private GameObject pauseMenuTitleText;
    [SerializeField] private GameObject saveGameButton, loadGameButton;

    [Header("Power Up Elements")]
    [SerializeField] private GameObject powerUpSquare;

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

    public void PowerUp(int roll)
    {
        Animator anim = powerUpSquare.GetComponent<Animator>();
        anim.SetInteger("Roll", roll);
        //stop on the last frame of anim
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

    public void endGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public Animator transition;
    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(index);
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

    public void OnSaveGameButtonPressed()
    {
        SaveAndLoad.SaveGame(playerMovement, playerShooting);
    }

    public void OnLoadGameButtonPressed()
    {
        SaveData loadedData = SaveAndLoad.LoadGame();
        if (loadedData != null)
        {
            //set player position
            playerMovement.transform.position = new Vector2(loadedData.playerX, loadedData.playerY);
            //set player ammo
            playerShooting.ammoCount = loadedData.playerAmmoCount;
            PauseGame();
        }
        else
        {
            Debug.LogError("Failed to load game data.");
            return;
        }
    }
}