using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class GroundController : MonoBehaviour
{
    public bool isStartGround;
    public Direction direction;
    [SerializeField] Transform centerPos;
    public Vector3 CenterPos => centerPos.position;
    public bool isPlayerHere { private set; get; }
    Rigidbody rb;
    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    public void OnLose()
    {
        if (isStartGround) return;
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * UnityEngine.Random.Range(1f, 6f), ForceMode.VelocityChange);
    }
    public int GetDirectionInt()
    {
        int degree = 0;
        switch (direction)
        {
            case Direction.forward:
                degree = 0;
                break;
            case Direction.left:
                degree = 270;
                break;
            case Direction.right:
                degree = 90;
                break;
            case Direction.backward:
                degree = 180;
                break;
        }
        return degree;
    }
}
public enum Direction
{
    forward = 0, left, right, backward
}
