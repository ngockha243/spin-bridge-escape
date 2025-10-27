using System;
using UnityEngine;

public abstract class BasePlatform : MonoBehaviour
{
    [SerializeField] protected PlatformType platformType;
    [SerializeField] protected Transform model;

    protected bool isStop;
    public virtual void Initialize()
    {
        isStop = false;
    }
    public abstract void OnActive();
    public abstract void OnDeactive();
    private void Update()
    {
        if (isStop) return;
        UpdateLogic();
    }
    public virtual void UpdateLogic() { }
    public void SetStop(bool isStop)
    {
        this.isStop = isStop;
    }
}
public enum PlatformType
{
    None,
    Normal,
    Move,
    Fall,
    Trap,
    Spring,
    Rotate,
    Disappear,
    Wind,
}
