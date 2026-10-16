using UnityEngine;

/// <summary>
/// Fixes camera viewport issues that cause "Screen position out of view frustum" errors
/// </summary>
public class CameraFix : MonoBehaviour
{
    private Camera mainCamera;
    private Rect targetViewportRect = new Rect(0, 0, 1, 1);
    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        // Set the viewport rect to full screen
        if (mainCamera != null)
        {
            mainCamera.rect = targetViewportRect;
        }
    }
    
    void LateUpdate()
    {
        if (mainCamera == null)
            return;
            
        // Force the viewport rect to be full screen every frame
        // This prevents any other script from modifying it incorrectly
        if (mainCamera.rect != targetViewportRect)
        {
            mainCamera.rect = targetViewportRect;
        }
    }
    
    void OnPreRender()
    {
        // Ensure viewport is correct before rendering
        if (mainCamera != null)
        {
            mainCamera.rect = targetViewportRect;
        }
    }
}
