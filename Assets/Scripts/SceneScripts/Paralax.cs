using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Layer Settings")]
    [SerializeField] private Transform[] backgroundLayers;
    [SerializeField] private float[] parallaxMultipliers;
    
    [Header("Camera Reference")]
    [SerializeField] private Camera mainCamera;
    
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    
    private void Start()
    {
        lastCameraPosition = mainCamera.transform.position;
        
        Sprite sprite = backgroundLayers[0].GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }
    
    private void LateUpdate()
    {
        if (backgroundLayers.Length != 2 || parallaxMultipliers.Length != 2) return;
        
        Vector3 deltaMovement = mainCamera.transform.position - lastCameraPosition;
        
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            Vector3 newPosition = backgroundLayers[i].position;
            newPosition.x += deltaMovement.x * parallaxMultipliers[i];
            backgroundLayers[i].position = newPosition;
        }
        
        lastCameraPosition = mainCamera.transform.position;
    }
}