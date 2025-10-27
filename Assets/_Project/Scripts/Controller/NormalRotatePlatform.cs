using UnityEngine;

public class NormalRotatePlatform : BasePlatform
{
    [SerializeField] float speedRotate = 5f;
    private float currentRotateValue;
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (isStop) return;
        currentRotateValue += speedRotate * Time.deltaTime;
        model.transform.rotation = Quaternion.Euler(new Vector3(0f, currentRotateValue, 0f));
        var y = model.transform.rotation.eulerAngles.y;
        if ((y >= 87f && y <= 93f) || (y >= 267f && y <= 273f))
        {
            PERFECT = true;
        }
        else
        {
            PERFECT = false;
        }
    }
}
