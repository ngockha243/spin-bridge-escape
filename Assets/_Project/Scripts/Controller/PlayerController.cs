using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private GameController gameCtrl;
    private LevelController levelCtrl;
    public LayerMask groundLayer, platformLayer;
    public RaycastCheck foot_1, foot_2;

    public float speedMovement = 5f;
    public float groundRayDistance = 0.2f;
    public float footOffset;

    private Rigidbody rb;
    private bool isMoving;
    public Animator animator;
    public void Initialize(GameController gameCtrl)
    {
        this.gameCtrl = gameCtrl;
        levelCtrl = LevelController.Instance;
        isMoving = false;
        rb = GetComponent<Rigidbody>();
    }
    public void UpdatePhysic()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AcceptMoving();

        }
        if (isMoving)
        {
            Moving();
        }
    }
    private void AcceptMoving()
    {
        isMoving = true;
        gameCtrl.startGround.NextPlatform.SetStop(true);
    }
    private void Moving()
    {

        rb.AddForce(Vector3.forward * speedMovement);
        animator.Play("run");
        if (!IsOnPlatform())
        {
            if (IsOnGround() && currentGround == gameCtrl.endGround)
            {
                gameCtrl.SetStartGround(currentGround);
                var v = currentGround.CenterPos;
                rb.velocity = Vector3.zero;
                rb.MovePosition(v);
                isMoving = false;
                animator.Play("idle");
                Debug.Log("You Win!");
                return;
            }
            isMoving = false;
            Debug.Log("You Lose!");
        }
    }
    GroundController currentGround;
    public bool IsOnPlatform()
    {
        return (foot_1.IsOnPlatform() || foot_2.IsOnPlatform());
    }
    public bool IsOnGround()
    {
        return RayCast(transform.position + new Vector3(0, 0, footOffset), Vector2.down, groundRayDistance, groundLayer);
    }

    private bool RayCast(Vector3 position, Vector3 direction, float distance, LayerMask layerMask)
    {
        Ray ray = new Ray(position, direction);
        var raycast = Physics.Raycast(ray, out RaycastHit hit, distance, layerMask);
        if (hit.collider != null)
        {
            currentGround = hit.collider.GetComponentInParent<GroundController>();
        }
        Color color = raycast ? Color.red : Color.green;
        Debug.DrawRay(position, Vector3.down * distance, color);
        return raycast;
    }
}
