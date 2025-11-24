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
    [Header("Score Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreText2;
    [Header("Round Manager")]
    [SerializeField] private RoundManager rm;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI roundText2;

    private AudioSource _audioSource;

    void Start()
    {

        _audioSource = GetComponent<AudioSource>();

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
        UpdateScoreUI();
    }

    public void ShowRound(int round)
    {
        StartCoroutine(ShowRoundRoutine(round));
    }

    private IEnumerator ShowRoundRoutine(int round)
    {
        roundText.gameObject.SetActive(true);
        roundText2.gameObject.SetActive(true);
        roundText.text = "ROUND " + round;
        roundText2.text = "ROUND " + round;

        // fade in
        roundText.alpha = 0f;
        roundText2.alpha = 0f;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            roundText.alpha = t;
            roundText2.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f); // display time

        // fade out
        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            roundText.alpha = t;
            roundText2.alpha = t;
            yield return null;
        }

        roundText.gameObject.SetActive(false);
        roundText2.gameObject.SetActive(false);
    }

    public int currentPowerUpRoll = 0;
    public void PowerUp(int roll)
    {
        _audioSource.Play();
        currentPowerUpRoll = roll;
        Animator anim = powerUpSquare.GetComponent<Animator>();
        anim.SetInteger("Roll", roll);
        //when last frame reached after 2.5 seconds, give player the powerup effect
        Invoke("PowerUpAllowed", 2.5f);
    }

    public void PowerUpAllowed()
    {
        playerShooting.roll = currentPowerUpRoll;
        Debug.Log("Powerup " + currentPowerUpRoll + " applied to player.");
        Invoke("ResetPowerUp", 10f);
    }

    public void ResetPowerUp()
    {
        Animator anim = powerUpSquare.GetComponent<Animator>();
        anim.SetInteger("Roll", 0);
        currentPowerUpRoll = 0;
        playerShooting.roll = 0;
        Debug.Log("Powerup removed from player.");
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

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + rm.score.ToString();
        scoreText2.text = "Score: " + rm.score.ToString();
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