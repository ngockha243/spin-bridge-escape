using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField] Transform centerPos;
    [SerializeField] BasePlatform nextPlatform;
    public void StopPlatform(bool isStop)
    {
        nextPlatform.SetStop(isStop);
    }
    public BasePlatform NextPlatform => nextPlatform;
    public Vector3 CenterPos => centerPos.position;
    public bool isPlayerHere { private set; get; }
    public void SetPlayerHere(bool status)
    {
        isPlayerHere = status;
    }
}
