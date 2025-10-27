using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public LayerMask platformLayer;
    public float distance;
    public float footOffset;
    BasePlatform a;
    private void Update()
    {
        IsOnPlatform(ref a);
    }
    public bool IsOnPlatform(ref BasePlatform platformDetected)
    {
        return RayCast(transform.position + new Vector3(0, 0, footOffset), Vector2.down, distance, platformLayer, ref platformDetected);
    }

    private bool RayCast(Vector3 position, Vector3 direction, float distance, LayerMask layerMask, ref BasePlatform platformDetected)
    {
        Ray ray = new Ray(position, direction);
        var raycast = Physics.Raycast(ray, out RaycastHit hit, distance, layerMask);
        if(hit.collider != null)
        {
            platformDetected = hit.collider.GetComponentInParent<BasePlatform>();
        }
        Color color = raycast ? Color.red : Color.green;
        Debug.DrawRay(position, Vector3.down * distance, color);
        return raycast;
    }
}
