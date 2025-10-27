using DG.Tweening;
using FastFood;
using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private GameController gameCtrl;
    private LevelController levelCtrl;

    public LayerMask groundLayer;
    public RaycastCheck rayCheckPlatform;

    public AnimationCurve moveCurve;
    public float speedDuration = 5f;
    public float groundRayDistance = 0.2f;
    public float footOffset;

    private Rigidbody rb;
    private bool isMoving, isFalling;

    public Animator animator;
    public SpriteRenderer shadow;
    public void Initialize(GameController gameCtrl)
    {
        this.gameCtrl = gameCtrl;
        levelCtrl = LevelController.Instance;
        isMoving = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        if (moveCurve == null)
        {
            moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
        transform.position = gameCtrl.startGround.CenterPos;
        animator.Play($"idle{Random.Range(1, 4)}");
        shadow.DOFade(0.2f, 0.2f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    public void UpdateLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryCrossBridge();

        }
    }
    public void UpdateLate()
    {
        if (!isFalling)
        {
            CameraController.Instance.FollowTo(transform.position);
        }

    }
    void TryCrossBridge()
    {
        if (IsOnPlatform())
        {
            Debug.Log("On Platform");
            StartCoroutine(Moving());
        }
        else
        {
            Debug.Log("Not On Platform - Fall Straight Down");
            StartCoroutine(FallStraightDown());
        }
    }
    IEnumerator FallStraightDown()
    {
        isMoving = false;
        isFalling = true;
        rb.isKinematic = false;
        animator.Play("jump");
        shadow.DOFade(0, 0.2f);
        Vector3 jumpDirection = (transform.forward * 2) + (Vector3.up * 3);
        rb.AddForce(jumpDirection, ForceMode.VelocityChange);
        levelCtrl.OnLose();
        GameManager.Instance.SwitchGameState(GameState.LOSE);
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator Moving()
    {
        isMoving = true;
        gameCtrl.SetCurrentPlatform(PlatformDetected);
        gameCtrl.CurrentPlatform.SetStop(true);

        animator.Play("run");
        Vector3 startPos = gameCtrl.startGround.CenterPos;
        Vector3 endPos = gameCtrl.endGround.CenterPos;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / speedDuration;
            float curveT = moveCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, endPos, curveT);
            yield return null;
        }

        isMoving = false;
        animator.Play($"idle{Random.Range(1, 4)}");
        gameCtrl.SetStartGround(gameCtrl.endGround);
    }
    GroundController currentGround;
    BasePlatform PlatformDetected;
    public bool IsOnPlatform()
    {
        return (rayCheckPlatform.IsOnPlatform(ref PlatformDetected));
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinGate"))
        {
            StopAllCoroutines();
            GameManager.Instance.SwitchGameState(GameState.WIN);
            animator.Play($"Dance_{Random.Range(1, 5)}");
            transform.DORotate(new Vector3(0, 180, 0), 0.5f);

        }
    }
}
