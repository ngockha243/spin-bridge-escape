using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupUI
{
    [SerializeField] GameObject presentObj;
    [SerializeField] private Button nextLvBtn, replayBtn, closeBtn;
    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);
        nextLvBtn.onClick.AddListener(OnNextLevel);
        closeBtn.onClick.AddListener(OnClose);
    }
    public override void Show(Action onClose, bool isShowTopUI)
    {
        base.Show(onClose, isShowTopUI);
        presentObj.SetActive(false);
    }

    private void SetAllBtnInteract(bool isInteractable)
    {
        nextLvBtn.interactable = isInteractable;
        closeBtn.interactable = isInteractable;
    }
   
    private void OnClose()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
        SetAllBtnInteract(false);

    }
    private void OnNextLevel()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

        SetAllBtnInteract(false);

    }
}
