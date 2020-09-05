using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UiDisplay : MonoBehaviour
{
    protected TextMeshProUGUI _displayText;

    protected void Awake()
    {
        _displayText = GetComponent<TextMeshProUGUI>();
    }

    protected virtual void DisplayText(object sender, int value)
    {
        _displayText.SetText(value.ToString());
    }
}
