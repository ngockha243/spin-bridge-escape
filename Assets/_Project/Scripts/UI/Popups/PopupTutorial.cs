using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupTutorial : PopupUI
{
    [Serializable] public struct TutorialData
    {
        public Sprite iconFunction;
        public string content;
        public Sprite image;
    }
    [SerializeField] TutorialData[] tutorials;
    [SerializeField] TextMeshProUGUI textContent;
    [SerializeField] Image imgTutorial, imgFunc;
    [SerializeField] TextMeshProUGUI textBtn;
    [SerializeField] Button btnNext;
    private int currentIndex = 0;
    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);
        btnNext.onClick.AddListener(OnClickNext);
    }
    public override void Show(Action onClose)
    {
        base.Show(onClose);
        currentIndex = 0;
        SetTutorial(tutorials[0]);
        if (GameManager.currentState != GameState.NONE)
        {
            GameManager.Instance.SwitchGameState(GameState.PAUSE);
        }
    }

    private void OnClickNext()
    {
        if(currentIndex < tutorials.Length - 1)
        {
            currentIndex++;
            SetTutorial(tutorials[currentIndex]);
        }
        else
        {
            if (GameManager.currentState != GameState.NONE)
            {
                GameManager.Instance.SwitchGameState(GameState.PLAY);
            }
            Hide();
        }
    }

    private void SetTutorial(TutorialData data)
    {
        textContent.text = data.content;
        imgTutorial.sprite = data.image;
        imgFunc.sprite = data.iconFunction;
        if (currentIndex >= tutorials.Length - 1)
        {
            textBtn.text = "Got it!";
        }
        else
        {
            textBtn.text = "Next";
        }
    }
}
