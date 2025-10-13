using UnityEngine;
using TMPro;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public TMP_Text playerShootingText;

    // stop animation from stacking
    public void PlayerShooting()
    {
        StopAllCoroutines();
        StartCoroutine(RollAnimation());
    }

    //coroutine for animation
    private IEnumerator RollAnimation()
    {
        //get the result of the roll, idk why i did it with a singleton this coulda been by just passing in the variable
        int rollResult = DiceRoll.Instance.currentRoll;

        Time.timeScale = 0.025f; // Slow down time for dramatic effect

        // do a goofy rolling animation for now
        for (int i = 0; i < 2; i++)
        {
            for (int j = 1; j <= 6; j++)
            {
                playerShootingText.text = j.ToString();
                yield return new WaitForSecondsRealtime(0.1f); // Wait 0.1 seconds (ignores timeScale)

                // If this is the last loop and j matches the roll, stop early
                if (i == 1 && j == rollResult)
                {
                    playerShootingText.text = rollResult.ToString();
                    Time.timeScale = 1f; // Restore time
                    yield break;
                }
            }
        }

        //regular end of animation
        playerShootingText.text = rollResult.ToString();
        Time.timeScale = 1f;
    }
}