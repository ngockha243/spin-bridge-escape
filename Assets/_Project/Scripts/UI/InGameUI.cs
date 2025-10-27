using UnityEngine;
using UnityEngine.UI;

public class InGameUI : ScreenUI
{
    [SerializeField] Button settingBtn;
    public override void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        settingBtn.onClick.AddListener(OnSetting);
    }

    private void OnSetting()
    {
        GameManager.Instance.SwitchGameState(GameState.PAUSE);
        uiManager.ShowPopup<PopupSettingMain>(null, true);
    }
}
