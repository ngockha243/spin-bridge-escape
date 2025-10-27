using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : BasePlatform
{
    [SerializeField] float speedRotate = 5f;
    private float currentRotateValue;
    public override void OnActive()
    {
    }

    public override void OnDeactive()
    {
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        currentRotateValue += speedRotate * Time.deltaTime;
        model.transform.rotation = Quaternion.Euler(new Vector3(0f, currentRotateValue, 0f));
    }
}
