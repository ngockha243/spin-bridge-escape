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
    private void Start()
    {
        playerCtrl.Initialize(this);
        SetStartGround(levelCtrl.Grounds[0]);
    }
    private void FixedUpdate()
    {
        playerCtrl.UpdatePhysic();
    }
    public void OnLoseGame()
    {
        Debug.Log("You Lose!");
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
