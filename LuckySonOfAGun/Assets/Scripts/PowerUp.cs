using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameUI gui;

    // Start is called before the first frame update
    void Start()
    {
        gui = GameObject.FindFirstObjectByType<GameUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int roll = Random.Range(1, 7);

            gui.PowerUp(roll);
            Debug.Log("rolled a" + roll);

            Destroy(this.gameObject);
        }
    }
}
