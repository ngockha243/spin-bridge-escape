using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Crystal
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaPanel : MonoBehaviour
    {
        public RectTransform rectTransform
        {
            get
            {
                if (_rect == null)
                {
                    _rect = GetComponent<RectTransform>();
                }
                return _rect;
            }
        }
        private RectTransform _rect;
        private SafeArea s;
        public bool isSetBanner;
        private void Start()
        {
            s = SafeArea.Instance;
            if (s)
            {
                s.AddPanel(this);
            }
        }
        private void OnDestroy()
        {
            if (s)
            {
                s.RemovePanel(this);
            }
        }
    }
}

