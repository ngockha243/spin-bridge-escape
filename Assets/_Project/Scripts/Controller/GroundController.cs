using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class GroundController : MonoBehaviour
{
    public bool isStartGround;
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
}
