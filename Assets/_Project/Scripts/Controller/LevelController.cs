using System;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] GroundController[] grounds;
    [SerializeField] BasePlatform[] platforms;
    public GroundController[] Grounds => grounds;
    public BasePlatform[] Platforms => platforms;
    [ContextMenu("GetReference")]
    void GetReference()
    {
        grounds = GetComponentsInChildren<GroundController>();
        platforms = GetComponentsInChildren<BasePlatform>();
    }
    public void Initialize()
    {
        foreach (var ground in grounds)
        {
            ground.Initialize();
        }
        foreach (var platform in platforms)
        {
            platform.Initialize();
        }
    }
    public void UpdateLogic()
    {
        if(GameManager.currentState != GameState.PLAY) return;
        foreach (var platform in platforms)
        {
            platform.UpdateLogic();
        }
    }
    public void OnLose()
    {
        foreach (var platform in platforms)
        {
            platform.OnLose();
        }
        foreach (var ground in grounds)
        {
            ground.OnLose();
        }

    }

}
