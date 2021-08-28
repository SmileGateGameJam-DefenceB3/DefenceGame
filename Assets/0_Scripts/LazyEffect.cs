using Cysharp.Threading.Tasks;
using UnityEngine;

public class LazyEffect : MonoBehaviour
{
    private async void Awake()
    {
        await UniTask.Delay(5000);
        if (this != null)
        {
            Destroy(gameObject);
        }
    }
}
