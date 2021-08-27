using Common;
using UnityEngine;

namespace UI
{
    public class InGameUIManager : SingletonMonoBehaviour<InGameUIManager>
    {
        [SerializeField] private RectTransform _uiRoot;
        [SerializeField] private Camera _uiCamera;

        public RectTransform UIRoot => _uiRoot;
        public Camera UICamera => _uiCamera;
    }
}
