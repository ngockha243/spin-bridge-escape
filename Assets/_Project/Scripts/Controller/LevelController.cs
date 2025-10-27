using System;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] GroundController[] grounds;
    [SerializeField] BasePlatform[] platforms;
    public GroundController[] Grounds => grounds;
    public BasePlatform[] Platforms => platforms;
    //private void Start()
    //{
    //    SetCurrentGround(grounds[0]);
    //}

}
