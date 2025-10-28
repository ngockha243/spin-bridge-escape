using FastFood;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : ScreenUI
{
    [SerializeField] Button settingBtn, playBtn, randomSkinBtn, tutorialBtn;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] ParticleSystem fireworkFX;
    public override void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        settingBtn.onClick.AddListener(OnSetting);
        ActiveBtn(true);
        playBtn.onClick.AddListener(OnPlay);
        randomSkinBtn.onClick.AddListener(() =>
        {
            PlayerController.Instance.skinCtrl.MixRandom();
        });
        tutorialBtn.onClick.AddListener(() =>
        {
            uiManager.ShowPopup<PopupTutorial>(null);
        });
        levelTxt.text = $"LEVEL {LevelManager.Level}";
    }

    private void OnPlay()
    {
        ActiveBtn(false);

        if(LevelManager.Tutorial == 0)
        {
            uiManager.ShowPopup<PopupTutorial>(action);
            LevelManager.Tutorial++;
            return;
        }
        action();
        void action()
        {
            CameraController.Instance.Back2StartPos();
            PlayerController.Instance.SetRotate(Vector3.zero, 1f, () => GameManager.Instance.SwitchGameState(GameState.PLAY));
        }
    }

    private void OnSetting()
    {
        uiManager.ShowPopup<PopupSettingMain>(null);
    }
    public void PlayFirework()
    {
        fireworkFX.Play();
    }
    private void ActiveBtn(bool active)
    {
        playBtn.gameObject.SetActive(active);
        randomSkinBtn.gameObject.SetActive(active);
        tutorialBtn.gameObject.SetActive(active);
    }
}
