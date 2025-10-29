using DG.Tweening;
using FastFood;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : ScreenUI
{
    [SerializeField] Button settingBtn, playBtn, leftNavBtn, rightNavBtn, buyBtn, tutorialBtn;
    [SerializeField] TextMeshProUGUI levelTxt, skinCostTxt, currentCoinTxt;
    [SerializeField] ParticleSystem fireworkFX;
    [SerializeField] GameObject coinObj, tutorialObj;
    public GameObject[] AllButtons => new GameObject[] { playBtn.gameObject, leftNavBtn.gameObject, rightNavBtn.gameObject, buyBtn.gameObject, tutorialBtn.gameObject, settingBtn.gameObject };
    public override void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        currentCoinTxt.text = LevelManager.Coin.ToString();
        settingBtn.onClick.AddListener(OnSetting);
        ActiveBtn(!GameManager.NEW_LEVEL);
        if (GameManager.NEW_LEVEL)
        {
            CameraController.Instance.Back2StartPos();
            PlayerController.Instance.SetRotate(Vector3.up * GameController.Instance.startGround.GetDirectionInt(), 1f, () => GameManager.Instance.SwitchGameState(GameState.PLAY));
        }
        playBtn.onClick.AddListener(OnPlay);
        HandleSkin();
        tutorialBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

            uiManager.ShowPopup<PopupTutorial>(null);
        });

        levelTxt.text = $"LEVEL {LevelManager.Level}";
        tutorialObj.SetActive(false);
    }
    private void HandleSkin()
    {
        var s = PlayerController.Instance.skinCtrl;
        int cost = 0;
        bool owned = false;
        buyBtn.gameObject.SetActive(false);
        leftNavBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

            s.Left(ref cost, ref owned);
            skinCostTxt.text = cost.ToString();
            skinCostTxt.text = cost.ToString();
            if (s.currentSkin.Owned > 0)
            {
                buyBtn.gameObject.SetActive(false);
            }
            else
            {
                buyBtn.gameObject.SetActive(true);
            }
        });
        rightNavBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

            s.Right(ref cost, ref owned);
            skinCostTxt.text = cost.ToString();
            if (s.currentSkin.Owned > 0)
            {
                buyBtn.gameObject.SetActive(false);
            }
            else
            {
                buyBtn.gameObject.SetActive(true);
            }
        });

        buyBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

            bool isOkay = false;
            s.Buy(ref isOkay);
            if (!isOkay)
            {
                skinCostTxt.DOKill();
                Sequence seq = DOTween.Sequence();

                seq.Append(skinCostTxt.DOColor(Color.red, 0.1f))
                   .Append(skinCostTxt.DOColor(Color.white, 0.1f))
                   .SetLoops(3)
                   .OnComplete(() => skinCostTxt.color = Color.white);
            }
            else
            {
                buyBtn.gameObject.SetActive(false);
            }
        });
    }
    private void OnPlay()
    {
        ActiveBtn(false);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

        PlayerController.Instance.skinCtrl.LoadSkin();
        if (LevelManager.Tutorial == 0)
        {
            uiManager.ShowPopup<PopupTutorial>(action);
            //LevelManager.Tutorial++;
            return;
        }
        action();
        void action()
        {
            CameraController.Instance.Back2StartPos();
            PlayerController.Instance.SetRotate(Vector3.up * GameController.Instance.startGround.GetDirectionInt(), 1f, () =>
            {
                if (LevelManager.Tutorial == 0)
                {
                    HandleTutorial(true);
                }
                GameManager.Instance.SwitchGameState(GameState.PLAY);
            });
        }
    }
    public void HandleTutorial(bool isActive)
    {
        if (isActive)
        {
            tutorialObj.SetActive(true);
            LevelManager.Tutorial++;
        }
        else
        {
            tutorialObj.SetActive(false);
        }
    }
    private void OnSetting()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
        uiManager.ShowPopup<PopupSettingMain>(null);
    }
    public void PlayFirework()
    {
        fireworkFX.Play();
    }
    public void UpdateCoin()
    {
        currentCoinTxt.text = LevelManager.Coin.ToString();
    }
    private void ActiveBtn(bool active)
    {
        playBtn.gameObject.SetActive(active);
        leftNavBtn.gameObject.SetActive(active);
        tutorialBtn.gameObject.SetActive(active);
        rightNavBtn.gameObject.SetActive(active);
        buyBtn.gameObject.SetActive(active);

        coinObj.gameObject.SetActive(active);
    }
}
