using Common;
using UnityEngine;

namespace UI
{
    public class InGameUIManager : SingletonMonoBehaviour<InGameUIManager>
    {
        [SerializeField] private RectTransform _uiRoot;
        [SerializeField] private Camera _camera;

        public RectTransform UIRoot => _uiRoot;
        public Camera Camera => _camera;
    }
}
