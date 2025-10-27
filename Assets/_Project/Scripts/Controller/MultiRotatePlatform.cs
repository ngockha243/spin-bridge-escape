using UnityEngine;

public class MultiRotatePlatform : BasePlatform
{
    [SerializeField] float speedRotate = 50f;
    [SerializeField] Transform[] models;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (isStop) return;
        for (int i = 0; i < models.Length; i++)
        {
            if (models[i] == null) continue;

            float direction = (i % 2 == 0) ? -1f : 1f;

            models[i].Rotate(Vector3.up * direction * speedRotate * Time.deltaTime, Space.Self);

            float y = models[i].localEulerAngles.y;
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
}
