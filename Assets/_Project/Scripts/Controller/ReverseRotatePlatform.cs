using UnityEngine;

public class ReverseRotatePlatform : BasePlatform
{
    [SerializeField] float speedRotate = 5f;
    private float currentRotateValue;
    private float currentTime;
    public override void Initialize()
    {
        base.Initialize();
        currentTime = GetRandomValue(1f, 5f);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (isStop) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            speedRotate = -speedRotate;
            currentTime = GetRandomValue(1f, 5f);
        }
        Debug.Log(currentTime);
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
