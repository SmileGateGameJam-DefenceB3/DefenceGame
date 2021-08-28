using UnityEngine;

public class Kingdom : MonoBehaviour
{
    [SerializeField] private Team _team;
    
    private int _life;
    public int Life
    {
        get => _life;
        set => _life = Mathf.Max(0, value);
    }
}
