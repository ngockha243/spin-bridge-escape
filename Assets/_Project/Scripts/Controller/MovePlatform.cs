using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : BasePlatform
{
    [SerializeField] Transform[] points;
    [SerializeField] float speed;
    int count;
    public override void Initialize()
    {
        base.Initialize();
        count = 0;
        model.position = points[count].position;
    }
    public override void UpdateLogic()
    {
        if(isStop) return;
        if (count < points.Length)
        {
            model.position = Vector3.MoveTowards(model.position, points[count].position, speed * Time.deltaTime);
            if (Vector3.Distance(model.position, points[count].position) < 0.1f)
            {
                count++;
            }
        }
        else
        {
            count = 0;
        }
        var d = Vector3.Distance(model.position, points[0].position);
        if (d >= -0.05f && d <= 0.05f)
        {
            PERFECT = true;
        }
        else
        {
            PERFECT = false;
        }
    }
}
