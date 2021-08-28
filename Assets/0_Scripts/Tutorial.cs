using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation _animation1;
    [SerializeField] private DOTweenAnimation _animation2;
    
    public async void Close()
    {
        _animation1.DOPlayBackwards();
        _animation2.DOPlayBackwards();
        await UniTask.Delay(500);
        InGameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
