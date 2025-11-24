using UnityEngine;
public class ScreenBounds : MonoBehaviour
{
    [SerializeField] private float thickness = 1f; // how thick the walls are
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null) return;

        // Calculate the visible area
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // Create borders as empty GameObjects with BoxCollider2D
        CreateBorder("TopBorder", new Vector2(0, camHeight / 2 + thickness / 2), new Vector2(camWidth + 2 * thickness, thickness));
        CreateBorder("BottomBorder", new Vector2(0, -camHeight / 2 - thickness / 2), new Vector2(camWidth + 2 * thickness, thickness));
        CreateBorder("LeftBorder", new Vector2(-camWidth / 2 - thickness / 2, 0), new Vector2(thickness, camHeight));
        CreateBorder("RightBorder", new Vector2(camWidth / 2 + thickness / 2, 0), new Vector2(thickness, camHeight));
    }

    private void CreateBorder(string name, Vector2 position, Vector2 size)
    {
        GameObject border = new GameObject(name);
        border.transform.parent = transform;
        border.transform.localPosition = position;
        //set border layermask to screenbounds
        border.layer = LayerMask.NameToLayer("ScreenBounds");

        BoxCollider2D collider = border.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = false; // make true if you just want to detect, not block

        Rigidbody2D rb = border.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.mass = 1000000000000f;

        //ignore enemies
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("ScreenBounds"), true);
    }
}
