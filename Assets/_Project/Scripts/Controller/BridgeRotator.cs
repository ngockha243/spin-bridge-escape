using UnityEngine;

public class BridgeRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 90f;       // độ/giây
    public bool rotateClockwise = true;

    [Header("Angle Check")]
    [Range(0f, 10f)] public float safeAngleRange = 5f; // ± biên độ an toàn

    private float currentAngle;

    void Update()
    {
        float dir = rotateClockwise ? 1f : -1f;
        transform.Rotate(Vector3.up * dir * rotateSpeed * Time.deltaTime);
        currentAngle = transform.eulerAngles.y % 180f;
    }

    public bool IsBridgeAligned()
    {
        float diff = Mathf.Abs(currentAngle - 90f);
        return diff <= safeAngleRange;
    }

    public float GetAngleDiff()
    {
        return Mathf.Abs(currentAngle - 90f);
    }
}
