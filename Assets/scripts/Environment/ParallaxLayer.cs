using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField]
    private Transform targetCamera;

    [SerializeField]
    private float parallaxFactor = 0.3f;

    [SerializeField]
    private bool applyX = true; // X√‡

    [SerializeField]
    private bool applyY = false; // Y√‡

    private Vector3 previousCameraPosition = Vector3.zero;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousCameraPosition = targetCamera.position;
    }

    private void LateUpdate()
    {
        if(targetCamera == null)
        {
            return; 
        }

        Vector3 currentCameraPosition = targetCamera.position;

        Vector3 cameraDelta = currentCameraPosition - previousCameraPosition;

        float moveX = 0.0f;
        float moveY = 0.0f;

        if (applyX == true)
        {
            moveX = cameraDelta.x * parallaxFactor;
        }

        if (applyY == true)
        {
            moveY = cameraDelta.y * parallaxFactor;
        }

        Vector3 layerPosition = transform.position;

        float newX = layerPosition.x + moveX;
        float newY = layerPosition.y + moveY;

        transform.position = new Vector3(newX, newY,layerPosition.z);

        previousCameraPosition = currentCameraPosition;

    }

}
