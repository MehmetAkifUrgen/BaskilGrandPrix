using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferEdgeColliderPoints : MonoBehaviour
{
    public GameObject existingEdgeColliderObject;
    private LineRenderer lineRenderer;

    void Start()
    {
        EdgeCollider2D existingEdgeCollider = existingEdgeColliderObject.GetComponent<EdgeCollider2D>();
        if (existingEdgeCollider == null)
        {
            Debug.LogError("No EdgeCollider2D found on the specified GameObject.");
            return;
        }

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        // LineRenderer ayarları
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.sortingOrder = 5;

        // Sprite atanması
        Texture texture = Resources.Load<Texture>("Sprites/MySprite");
        lineRenderer.material.mainTexture = texture;

        // Mevcut EdgeCollider2D noktalarını al ve LineRenderer'a aktar
        Vector2[] colliderPoints = existingEdgeCollider.points;
        Vector3[] lineRendererPositions = new Vector3[colliderPoints.Length];
        
        for (int i = 0; i < colliderPoints.Length; i++)
        {
            lineRendererPositions[i] = new Vector3(colliderPoints[i].x, colliderPoints[i].y, 0);
        }

        lineRenderer.positionCount = lineRendererPositions.Length;
        lineRenderer.SetPositions(lineRendererPositions);
    }
}
