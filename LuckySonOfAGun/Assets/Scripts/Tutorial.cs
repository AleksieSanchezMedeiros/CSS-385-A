using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI underneathText;

    public string[] arr;

    public GameObject backButton;

    public GameObject enemy;

    public Transform[] fodderEnemySpawn;

    private string textDispalyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backButton.SetActive(false);
        tutorialText.text = arr[0];
        underneathText.text = arr[0];
        StartCoroutine(tutorial(arr[0], 0));
    }

    IEnumerator tutorial(string textToDispaly, int curr)
    {
        textDispalyed = textToDispaly;

        if (arr.Length <= curr)
        {
            StopAllCoroutines();
            DisplayReturn();
            yield break;
        }

        if (textToDispaly == "Defeat The Enemy")
        {
            Instantiate(enemy, fodderEnemySpawn[Random.Range(0, 4)]);
        }
        else if (textToDispaly == "Defeat More Enemies")
        {
            Instantiate(enemy, fodderEnemySpawn[0]);
            Instantiate(enemy, fodderEnemySpawn[1]);
            Instantiate(enemy, fodderEnemySpawn[2]);
            Instantiate(enemy, fodderEnemySpawn[3]);

        }


        tutorialText.text = textToDispaly;
        underneathText.text = textToDispaly;

        curr++;

        yield return new WaitForSeconds(5f);

        StartCoroutine(tutorial(arr[curr], curr));
    }

    void Update()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (textDispalyed == "Go Get Em You Lucky Son Of A Gun" && taggedObjects.Length == 0)
        {
            DisplayReturn();
        }
    }

    public void DisplayReturn()
    {
        backButton.SetActive(true);
    }

    public Animator transition;
    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(index);
    }

    public void onBackButtonClicked()
    {
        StartCoroutine(LoadLevel(0));
    }
}
