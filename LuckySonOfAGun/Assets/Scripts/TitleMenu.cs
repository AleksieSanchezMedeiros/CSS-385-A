using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class TitleMenu : MonoBehaviour
{
    [Header("Initial UI Elements")]
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject exitButton;

    [Header("Player Button Pressed Elements")]
    public GameObject newGameButton;
    public GameObject loadButton;
    public GameObject backButton;

    [Header("Show/Hide Anim Settings")]
    public float targetX = 0f;          // Center position
    public float offscreenLeftX = -2000f;    // Offscreen position
    public float offscreenRightX = 2000f; // Right offscreen position
    public float moveSpeed = 10f;       // Speed of sliding animation

    GameObject[] initialItems;

    GameObject[] playPressedItems;

    bool isTransitioning = false;


    void Start()
    {
        // Create arrays for easy management
        initialItems = new GameObject[] { playButton, optionsButton, exitButton };
        playPressedItems = new GameObject[] { newGameButton, loadButton, backButton };

        // Initialize title menu
        ShowItems(initialItems, true);
        HideItems(playPressedItems, true);
    }

    public void onPlayButtonClicked()
    {
        if (isTransitioning) return;

        // Hide initial UI elements
        HideItems(initialItems);

        // Show play button pressed elements
        ShowItems(playPressedItems);
    }

    public void onBackButtonClicked()
    {
        if (isTransitioning) return;

        // Show initial UI elements
        ShowItems(initialItems);

        // Hide play button pressed elements
        HideItems(playPressedItems);
    }

    void ShowItems(GameObject[] items, bool instant = false)
    {
        //slide items to center of screen
        foreach (GameObject item in items)
        {
            //rect cause regular transform is useless for UI
            item.SetActive(true);
            RectTransform rect = item.GetComponent<RectTransform>();
            if (rect == null) continue;

            // Start offscreen (right)
            if (!instant)
                rect.anchoredPosition = new Vector2(offscreenRightX, rect.anchoredPosition.y);

            Vector2 targetPos = new Vector2(targetX, rect.anchoredPosition.y);

            if (instant)
                rect.anchoredPosition = targetPos;
            else
                StartCoroutine(Slide(rect, targetPos));
        }
    }

    void HideItems(GameObject[] items, bool instant = false)
    {
        //slide items offscreen
        foreach (GameObject item in items)
        {
            RectTransform rect = item.GetComponent<RectTransform>();
            if (rect == null) continue;

            Vector2 targetPos = new Vector2(offscreenLeftX, rect.anchoredPosition.y);

            if (instant)
            {
                rect.anchoredPosition = targetPos;
                item.SetActive(false);
            }
            else
            {
                StartCoroutine(Slide(rect, targetPos, deactivateAfter: item));
            }
        }
    }

    //slide could be used for random things so made it not deactivate if needed
    IEnumerator Slide(RectTransform rect, Vector2 targetPos, GameObject deactivateAfter = null)
    {
        isTransitioning = true;
        while (Vector2.Distance(rect.anchoredPosition, targetPos) > 5f) //margin of error
        {
            //move to end position
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, Time.deltaTime * moveSpeed);
            yield return null;
        }


        //skip when near there
        rect.anchoredPosition = targetPos;

        isTransitioning = false;

        if (deactivateAfter != null)
            deactivateAfter.SetActive(false);
    }

    public void onOptionsButtonClicked()
    {
        StartCoroutine(LoadLevel(3));
    }

    public void onNewGameButtonClicked()
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

    public void onRestartGameButtonClicked()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void onLoadGameButtonClicked()
    {
        // Load game logic here
        Debug.Log("Load Game button clicked");
    }

    public void onExitButtonClicked()
    {
        // Exit the application
        Application.Quit();
    }
}
