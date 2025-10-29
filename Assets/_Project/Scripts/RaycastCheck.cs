using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public LayerMask platformLayer;
    public float distance = 1f;
    public float footOffset = 0.2f;
    BasePlatform a;

    private void Update()
    {
        IsOnPlatform(ref a);
    }

    public bool IsOnPlatform(ref BasePlatform platformDetected)
    {
        Vector3 startPos = transform.position + transform.forward * footOffset + Vector3.up * 0.2f;
        return RayCast(startPos, Vector3.down, distance, platformLayer, ref platformDetected);
    }

    private bool RayCast(Vector3 position, Vector3 direction, float distance, LayerMask layerMask, ref BasePlatform platformDetected)
    {
        Ray ray = new Ray(position, direction);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, distance, layerMask);

        if (hit.collider != null)
            platformDetected = hit.collider.GetComponentInParent<BasePlatform>();

        Color color = hitSomething ? Color.red : Color.green;
        Debug.DrawRay(position, direction * distance, color);

        return hitSomething;
    }
}
