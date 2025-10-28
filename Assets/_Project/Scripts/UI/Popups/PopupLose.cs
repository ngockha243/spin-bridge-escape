using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupLose : PopupUI
{
    [SerializeField] private Button replayBtn;
    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);
        replayBtn.onClick.AddListener(OnReplay);
    }
    private void OnReplay()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
        replayBtn.interactable = false;
        GameManager.Instance.ReloadScene();
    }
}
