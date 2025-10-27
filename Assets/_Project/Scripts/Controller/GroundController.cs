using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField] Transform centerPos;
    public Vector3 CenterPos => centerPos.position;
    public bool isPlayerHere { private set; get; }
    public void SetPlayerHere(bool status)
    {
        isPlayerHere = status;
    }
}
