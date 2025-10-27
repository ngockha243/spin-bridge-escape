using DG.Tweening;
using System;
using UnityEngine;
namespace FastFood
{
    public class CameraController : Singleton<CameraController>
    {
        public Camera mainCamera;
        public Vector3 offset;
        public void FollowTo(Vector3 target)
        {
            mainCamera.transform.position = Vector3.ClampMagnitude(new Vector3(target.x + offset.x, target.y + offset.y, target.z + offset.z), 1000f);
        }
        public void ResizeCameraLR(SpriteRenderer target)
        {
            float max = target.bounds.max.x;
            Vector3 v = mainCamera.WorldToViewportPoint(new Vector3(max, target.transform.position.y, target.transform.position.z));
            if (v.x != 0)
            {
                float distanceToLeftEdge = mainCamera.transform.position.x - max;
                mainCamera.orthographicSize = Mathf.Abs(distanceToLeftEdge / mainCamera.aspect);
            }
        }
        public void ResizeCameraTB(SpriteRenderer target)
        {
            float max = target.bounds.max.y;
            Vector3 v = mainCamera.WorldToViewportPoint(new Vector3(target.transform.position.x, max, target.transform.position.z));
            if (v.x != 1)
            {
                float distanceToTopEdge = max - mainCamera.transform.position.y;
                mainCamera.orthographicSize = Mathf.Abs(distanceToTopEdge);
            }
        }
        public void SetCamSize(float size)
        {
            mainCamera.orthographicSize = size;
        }
        public void MoveTo(Vector3 newPos, float duration, float delay, Ease ease, Action onComplete = null)
        {
            mainCamera.transform.DOMove(newPos, duration).SetDelay(delay).SetEase(ease).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        private Tween KillCameraShake;
        public void ShakeCamera(float dur, float stre, int vib, int randness, bool smooth = true, Action onComplete = null)
        {
            if (KillCameraShake != null)
            {
                KillCameraShake.Kill();
                mainCamera.transform.localPosition = Vector3.forward * -10f;
            }
            KillCameraShake = mainCamera.DOShakePosition(dur, stre, vib, randness, smooth).OnComplete(() =>
            {
                mainCamera.transform.localPosition = Vector3.forward * -10f;
                onComplete?.Invoke();
            });
        }
        public static float GetEdgeCamera(CameraEdge cameraEdge)
        {
            float orthoHeight = Camera.main.orthographicSize;
            float orthoWidth = orthoHeight * Camera.main.aspect;
            float value = 0;
            switch (cameraEdge)
            {
                case CameraEdge.Left:
                    value = Camera.main.transform.position.x - orthoWidth;
                    break;
                case CameraEdge.Right:
                    value = Camera.main.transform.position.x + orthoWidth;
                    break;
                case CameraEdge.Top:
                    value = 0;
                    break;
                case CameraEdge.Bottom:
                    value = 1;
                    break;
            }
            return value;
        }
        //private Transform target;
        //private Vector3 velocity;
        //public void FollowTo(Transform target, float duration, Action onComplete)
        //{
        //    this.target = target;
        //    //if(Vector2.Distance(mainCamera.transform.position, endValue) <= 0)
        //    SD_GameManager.Instance.Delay(duration, () =>
        //    {
        //        this.target = null;
        //        onComplete?.Invoke();
        //    });
        //}
        //private void Update()
        //{
        //    if (target != null)
        //    {
        //        Vector3 newPos = new Vector3(0, target.position.y, -10f);
        //        mainCamera.transform.position = newPos;//Vector3.SmoothDamp(mainCamera.transform.position, newPos, ref velocity, 0.5f);
        //    }
        //}

    }
    public enum CameraEdge
    {
        Left,
        Right,
        Top,
        Bottom
    }
}

