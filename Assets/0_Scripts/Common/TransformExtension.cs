using UnityEngine;

namespace Common
{
    public static class TransformExtension
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }
    }
}
