using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] PlayerController playerCtrl;
    private LevelController levelCtrl => LevelController.Instance;
    public GroundController startGround;
    public GroundController endGround;
    public BasePlatform CurrentPlatform { private set; get; }
    public void SetCurrentPlatform(BasePlatform plf)
    {
        CurrentPlatform = plf;
    }
    public void Initialize()
    {
        levelCtrl.Initialize();
        SetStartGround(levelCtrl.Grounds[0]);
        playerCtrl.Initialize(this);

    }
    public void UpdateLogic()
    {
        if (GameManager.currentState != GameState.PLAY) return;
        levelCtrl.UpdateLogic();
        playerCtrl.UpdateLogic();
    }
    public void OnWinGame()
    {

    }

    public void SetStartGround(GroundController ground)
    {
        startGround = ground;
        var grounds = levelCtrl.Grounds;    
        int index = Array.IndexOf(grounds, ground);
        if (index < grounds.Length - 1)
        {
            endGround = grounds[index + 1];
        }
        else
        {
            endGround = null;
        }
    }
}
