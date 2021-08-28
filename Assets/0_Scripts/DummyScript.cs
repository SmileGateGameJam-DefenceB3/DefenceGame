using UnityEngine;
using UnityEngine.UI;

public class DummyScript : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    
    private void Awake()
    {
        _inputField.text = Constant.Instance.ActorMoveSpeed.ToString();
    }

    public void OnValueChanged()
    {
        if (float.TryParse(_inputField.text, out var value))
        {
            Constant.Instance.ActorMoveSpeed = value;
        }
    }
}
