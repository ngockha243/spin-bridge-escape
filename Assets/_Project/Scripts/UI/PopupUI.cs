using FastFood;
using System;
using UnityEngine;
public abstract class PopupUI : MonoBehaviour
{
    protected Animator animator;
    protected UIManager uiManager;
    protected event Action onClose;
    public static event Action<PopupUI> OnDestroyPopup;
    public static event Action<PopupUI> OnHide;
    public static event Action<PopupUI> OnShow;
    public bool isCache = true;
    public bool isShowing;
    //private GameState preState;
    private bool isShowTopUI;
    //protected TopUI topUI;
    public virtual void Initialize(UIManager manager)
    {
        this.uiManager = manager;
        //topUI = uiManager.topUI;
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        isShowing = false;
    }

    public virtual void Show(Action onClose, bool isShowTopUI)
    {
        animator.Play("OnShow");
        //preState = GameManager.currentState;
        //GameManager.Instance.SwitchGameState(GameState.PAUSE);

        this.isShowTopUI = isShowTopUI;
        //if (!topUI.isLock)
        //{
        //    if (isShowTopUI)
        //    {
        //        RectTransform rect = transform.Find("BG").GetComponent<RectTransform>();
        //        topUI.SetParent(rect);
        //        topUI.SetActive(true);
        //    }
        //    else
        //    {
        //        topUI.SetActive(false, true);
        //    }
        //}
        this.onClose = onClose;
        isShowing = true;
        gameObject.SetActive(true);
        OnShow?.Invoke(this);
    }
    public virtual void Hide()
    {
        //AudioManager.Instance.PlayOneShot("ClickSound", 1f);
        //GameManager.Instance.SwitchGameState(preState);
        animator.Play("OnHide");

        //if (!topUI.isLock && isShowTopUI) topUI.SetActive(false);
        GameManager.Instance.Delay(0.3f, () =>
        {
            gameObject.SetActive(false);
            isShowing = false;
            onClose?.Invoke();
            onClose = null;
            OnHide?.Invoke(this);
        });
        if (!isCache)
        {
            OnDestroyPopup?.Invoke(this);
            OnPopupDestroyed();
        }
    }
    protected virtual void OnPopupDestroyed()
    {

    }
}