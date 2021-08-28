using UI;
using UnityEngine;

public class Kingdom : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private UIHpBar _hpBar;
    
    private int _life;

    public int Life
    {
        get => _life;
        set
        {
            var prev = _life;
            _life = Mathf.Clamp(value, 0, Constant.Instance.MaxHP); 

            float ratio = (float) _life / Constant.Instance.MaxHP;
            if (prev > _life)
            {
                _hpBar.SetValue(ratio);
            }
            else
            {
                _hpBar.SetValueNoAnimation(ratio);
            }
        }
    }
    
    public void Initialize()
    {
        _life = Constant.Instance.MaxHP;
    }
}