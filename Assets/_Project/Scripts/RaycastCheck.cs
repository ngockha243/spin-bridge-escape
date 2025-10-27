using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public LayerMask platformLayer;
    public float distance;
    public float footOffset;
    private void Update()
    {
        IsOnPlatform();
    }
    public bool IsOnPlatform()
    {
        return RayCast(transform.position + new Vector3(0, 0, footOffset), Vector2.down, distance, platformLayer);
    }

    private bool RayCast(Vector3 position, Vector3 direction, float distance, LayerMask layerMask)
    {
        Ray ray = new Ray(position, direction);
        var raycast = Physics.Raycast(ray, out RaycastHit hit, distance, layerMask);
        Color color = raycast ? Color.red : Color.green;
        Debug.DrawRay(position, Vector3.down * distance, color);
        return raycast;
    }
}
