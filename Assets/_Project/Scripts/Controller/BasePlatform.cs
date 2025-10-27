using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public abstract class BasePlatform : MonoBehaviour
{
    [SerializeField] protected Transform model;
    protected Rigidbody rb;
    public bool PERFECT { protected set; get; }
    protected bool isStop;
    public virtual void Initialize()
    {
        isStop = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    public virtual void UpdateLogic() 
    {
    }
    public void SetStop(bool isStop)
    {
        this.isStop = isStop;
    }
    public void OnLose()
    {
        isStop = true;
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * UnityEngine.Random.Range(1f, 6f), ForceMode.VelocityChange);
    }
    public float GetRandomValue(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
}
