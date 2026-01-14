using UnityEngine;

[ExecuteAlways]
public class BoundaryWallsBuilder : MonoBehaviour
{
    [Header("References")]
    public Renderer groundRenderer; // arrastra el Ground (que tenga Renderer) o un plano del suelo

    [Header("Wall Settings")]
    public float wallHeight = 6f;
    public float wallThickness = 0.6f;
    public float margin = 0.2f; // separa un poco hacia fuera

    [ContextMenu("Build / Rebuild Walls")]
    public void Rebuild()
    {
        if (groundRenderer == null)
        {
            Debug.LogError("Asigna groundRenderer (Renderer del Ground).");
            return;
        }

        // Limpia hijos previos
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }

        Bounds b = groundRenderer.bounds;

        // Centro en Y: levantamos la pared para que toque el suelo
        float yCenter = b.center.y + wallHeight * 0.5f;

        // North / South (largo en X, fino en Z)
        CreateWall("Wall_North",
            new Vector3(b.center.x, yCenter, b.max.z + margin + wallThickness * 0.5f),
            new Vector3(b.size.x + margin * 2f, wallHeight, wallThickness));

        CreateWall("Wall_South",
            new Vector3(b.center.x, yCenter, b.min.z - margin - wallThickness * 0.5f),
            new Vector3(b.size.x + margin * 2f, wallHeight, wallThickness));

        // East / West (largo en Z, fino en X)
        CreateWall("Wall_East",
            new Vector3(b.max.x + margin + wallThickness * 0.5f, yCenter, b.center.z),
            new Vector3(wallThickness, wallHeight, b.size.z + margin * 2f));

        CreateWall("Wall_West",
            new Vector3(b.min.x - margin - wallThickness * 0.5f, yCenter, b.center.z),
            new Vector3(wallThickness, wallHeight, b.size.z + margin * 2f));
    }

    void CreateWall(string name, Vector3 center, Vector3 size)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
        go.transform.position = center;

        BoxCollider bc = go.AddComponent<BoxCollider>();
        bc.size = size;
        bc.isTrigger = false;
    }
}
