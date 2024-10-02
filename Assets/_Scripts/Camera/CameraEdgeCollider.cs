using UnityEngine;

public class CameraEdgeColliders : MonoBehaviour
{
    private void Start()
    {
        AddCollider();
    }

    private void AddCollider()
    {
        var camera = Camera.main;

        if (camera == null) return;
        if (!camera.orthographic) return;

        var bottomLeft = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        var topLeft = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, camera.nearClipPlane));
        var topRight = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, camera.nearClipPlane));
        var bottomRight = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, camera.nearClipPlane));

        // Adicionar ou usar EdgeCollider2D existente
        var edge = (GetComponent<EdgeCollider2D>() == null) ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

        var edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        edge.points = edgePoints;
    }
}