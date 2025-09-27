using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public GameObject player;
    private Vector3 offset = new Vector3(0, 9, -15); //changed it to a higher offset position compared to tutorial

    // LateUpdate is called after all updates
    void LateUpdate()
    {
        transform.position = player.transform.position + offset; 
    }
}
