using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UiDisplayText : MonoBehaviour
{
    protected TextMeshProUGUI Text;

    public virtual void DisplayText(int value)
    {
        Text.SetText(value.ToString());
    }
    
    public virtual void DisplayText(string value)
    {
        Text.SetText(value);
    }

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }
}
