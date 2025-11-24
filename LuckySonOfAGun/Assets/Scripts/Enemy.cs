using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private float saveSpeed;

    public int scoreValue = 10;

    void Awake()
    {
        saveSpeed = speed;
    }

    public void SlowDown()
    {
        saveSpeed = speed;
        speed = 1f;
        Invoke("SpeedUp", 3f);
    }

    private void SpeedUp()
    {
        speed = saveSpeed;
    }
}
