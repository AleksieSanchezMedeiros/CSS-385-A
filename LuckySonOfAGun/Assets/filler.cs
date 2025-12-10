using UnityEngine;

public class RandomTileBackground : MonoBehaviour
{
    public Sprite tileSprite;
    public float tileSize = 8f; // world units that your tile covers

    void Start()
    {
        FillBackground();
    }

    void FillBackground()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        int tilesX = Mathf.CeilToInt(width / tileSize);
        int tilesY = Mathf.CeilToInt(height / tileSize);

        Vector3 bottomLeft = cam.transform.position - new Vector3(width / 2f, height / 2f, 0);

        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                GameObject go = new GameObject($"Tile_{x}_{y}");
                go.transform.parent = transform;

                // Tile position
                Vector3 pos = bottomLeft + new Vector3(x * tileSize, y * tileSize, 0);
                go.transform.position = pos;

                // Sprite renderer
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = tileSprite;

                // Random rotation (0°, 90°, 180°, 270°)
                float angle = Random.Range(0, 4) * 90f;
                go.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}