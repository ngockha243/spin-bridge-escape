//using DG.Tweening;
//using Spine.Unity;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;
//public class AnimationHandle : MonoBehaviour
//{
//    private SkeletonAnimation _skeletonAnimation;
//    //private Material material { get { return m_renderer.material; } }
//    public SkeletonAnimation skeletonAnimation
//    {
//        get
//        {
//            if (_skeletonAnimation == null)
//            {
//                _skeletonAnimation = GetComponent<SkeletonAnimation>();
//                _skeletonAnimation.Initialize(true);
//            }
//            return _skeletonAnimation;
//        }
//    }
//    //public Renderer m_renderer;
//    private Dictionary<int, string> layerAnimator = new Dictionary<int, string>();
//    public virtual void ResetAnimator()
//    {
//        layerAnimator.Clear();
//        skeletonAnimation.ClearState();
//    }
//    public void ClearTrack(int index)
//    {
//        layerAnimator[index] = "";
//        skeletonAnimation.AnimationState.ClearTrack(index);
//    }
//    public void AddAnimation(Spine.Animation animation, int layer, bool isLoop, float delay)
//    {
//        Spine.TrackEntry track = skeletonAnimation.AnimationState.AddAnimation(layer, animation, isLoop, delay);
//    }
//    public void AddAnimation(Spine.Animation animation, int layer, bool isLoop, float delay, float timeScale)
//    {
//        Spine.TrackEntry track = skeletonAnimation.AnimationState.AddAnimation(layer, animation, isLoop, delay);
//    }
//    public void AddAnimation(string stateName, int layer, bool isLoop, float delay)
//    {
//        skeletonAnimation.AnimationState.AddAnimation(layer, stateName, isLoop, delay);
//    }
//    public void AddEmptyAnimation(int layer, float mixDuration, float delay)
//    {
//        skeletonAnimation.AnimationState.AddEmptyAnimation(layer, mixDuration, delay);
//    }
//    public void PlayAnimation(Spine.Animation animation, float mixDuration, int layer, bool isLoop, bool isLastState = false)
//    {
//        if (animation == null)
//        {
//            if (layerAnimator.ContainsKey(layer))
//            {
//                layerAnimator[layer] = "";
//            }
//            skeletonAnimation.AnimationState.SetEmptyAnimation(layer, 0);
//            return;
//        }
//        if (layerAnimator.ContainsKey(layer))
//        {
//            if (layerAnimator[layer].Equals(animation.Name))
//            {
//                if (isLoop)
//                {
//                    return;
//                }
//            }
//            else
//            {
//                layerAnimator[layer] = animation.Name;
//            }
//        }
//        else
//        {
//            if (isLoop)
//            {
//                layerAnimator.Add(layer, animation.Name);
//            }
//        }
//        var a = skeletonAnimation.AnimationState.SetAnimation(layer, animation, isLoop);
//        a.MixDuration = mixDuration;
//        if (!isLoop && !isLastState)
//        {
//            skeletonAnimation.AnimationState.AddEmptyAnimation(layer, 0, animation.Duration);
//        }
//    }
//    public void PlayAnimation(string stateName, float mixDuration, int layer, bool isLoop, bool isLastState = false)
//    {
//        Spine.Animation state = GetAnimation(stateName);
//        PlayAnimation(state, mixDuration, layer, isLoop, isLastState);
//    }
//    public Spine.Animation GetAnimation(string animationName)
//    {
//        return skeletonAnimation.Skeleton.Data.FindAnimation(animationName);
//    }
//    public void AddEventListener(Spine.AnimationState.TrackEntryEventDelegate eventDelegate)
//    {
//        if (skeletonAnimation != null)
//        {
//            skeletonAnimation.AnimationState.Event += eventDelegate;
//        }
//    }

//    public void RemoveEventListener(Spine.AnimationState.TrackEntryEventDelegate eventDelegate)
//    {
//        if (skeletonAnimation != null)
//        {
//            skeletonAnimation.AnimationState.Event -= eventDelegate;
//        }
//    }
//    public void SetTimeScale(float timeScale)
//    {
//        skeletonAnimation.AnimationState.TimeScale = timeScale;
//    }
//    public virtual void Deactive()
//    {
//        gameObject.SetActive(false);
//    }
//    public virtual void Active()
//    {
//        gameObject.SetActive(true);
//    }

//    public void SetSortingLayer(string layer)
//    {
//        skeletonAnimation.gameObject.GetComponent<SortingGroup>().sortingLayerName = layer;
//    }
//    public void SetSortingValueLayer(int value)
//    {
//        skeletonAnimation.gameObject.GetComponent<MeshRenderer>().sortingOrder = value;
//    }
//    //public float GetValueColor()
//    //{
//    //    return material.GetFloat("_FillPhase");
//    //}
//    //Tween x;
//    //public void SetValueColor(float endValue)
//    //{

//    //    material.SetColor("_FillColor", Color.black);
//    //    float value = GetValueColor();
//    //    if (x != null) x.Kill();
//    //    x = DOTween.To(() => value, x => value = x, endValue, 0.35f).OnUpdate(() =>
//    //    {
//    //        material.SetFloat("_FillPhase", value);
//    //    });
//    //}
//}


