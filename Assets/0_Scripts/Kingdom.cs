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
            _life = Mathf.Max(0, value);
            _hpBar.SetValue((float) _life / Constant.Instance.MaxHP);
        }
    }

    public void Initialize()
    {
        _life = Constant.Instance.MaxHP;
    }
}