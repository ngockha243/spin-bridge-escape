using DG.Tweening;
using FastFood;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public ParticleSystem perfectFX, warningFX;
    public SkinController skinCtrl;

    public void Initialize(GameController gameCtrl)
    {
        this.gameCtrl = gameCtrl;
        skinCtrl.Initialize();
        levelCtrl = LevelController.Instance;
        isMoving = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        if (moveCurve == null)
        {
            moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
        transform.position = gameCtrl.startGround.CenterPos;
        animator.Play($"idle{UnityEngine.Random.Range(1, 4)}");
        shadow.DOFade(0.2f, 0.2f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        if (GameManager.NEW_LEVEL) return;
        SetRotate(Vector3.up * 180f);
    }
    public void SetRotate(Vector3 value, float duration = 0.5f, Action onComplete = null)
    {
        transform.DORotate(value, duration).OnComplete(() => onComplete?.Invoke());
    }
    public void UpdateLogic()
    {
        if (isMoving || isFalling) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (UIManager.Instance.GetScreen<InGameUI>().AllButtons.Contains(EventSystem.current.currentSelectedGameObject)) return; // lmao
            TryCrossBridge();
        }
    }
    void TryCrossBridge()
    {
        if (IsOnPlatform())
        {
            StartCoroutine(Moving());
        }
        else
        {
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
        warningFX.Play();
        warningFX.transform.position = transform.position + Vector3.up * 1.58f;
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowPopup<PopupLose>(null);
        AudioManager.Instance.PlayOneShot(SFXStr.LOSE_VOICE, 2);
    }
    float stepLength;
    private IEnumerator Moving()
    {
        isMoving = true;
        stepLength = AudioManager.Instance.GetSFXLength(SFXStr.STEP);
        gameCtrl.SetCurrentPlatform(PlatformDetected);
        if (PlatformDetected.PERFECT)
        {
            perfectFX.Play();
            perfectFX.transform.position = transform.position + Vector3.up * 1.58f;
        }
        gameCtrl.CurrentPlatform.SetStop(true);
        AudioManager.Instance.PlayOneShot(SFXStr.CLACK, 2);
        animator.Play("run");
        Vector3 startPos = gameCtrl.startGround.CenterPos;
        Vector3 endPos = gameCtrl.endGround.CenterPos;
        float t = 0f;
        float stepTimer = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / speedDuration;
            stepTimer-= Time.deltaTime;
            if(stepTimer <= 0f)
            {
                AudioManager.Instance.PlayOneShot(SFXStr.STEP, 2);
                stepTimer = stepLength;
            }
            float curveT = moveCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, endPos, curveT);
            CameraController.Instance.FollowTo(transform.position);
            yield return null;
        }

        isMoving = false;
        animator.Play($"idle{UnityEngine.Random.Range(1, 4)}");
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
            UIManager.Instance.GetScreen<InGameUI>().PlayFirework();
            AudioManager.Instance.PlayOneShot(SFXStr.FIREWORK, 2);
            GameManager.Instance.SwitchGameState(GameState.WIN);
            animator.Play($"Dance_{UnityEngine.Random.Range(1, 5)}");
            SetRotate(Vector3.up * 180f);
            UIManager.Instance.ShowPopup<PopupWin>(null);

        }
    }
}
