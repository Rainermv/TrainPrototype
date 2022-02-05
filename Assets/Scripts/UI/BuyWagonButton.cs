using UnityEngine;
using UnityEngine.UI;

public class BuyWagonButton : MonoBehaviour
{
    public Button ButtonComponent;
    public Image ImageComponent; // wagon image
    public Text NameComponent;
    public Text MoneyValueComponent;

    public Color DisabledColor;

    private Image _buttonImage; // Actual button image
    private Color _enabledColor;


    public WagonComponent WagonComponent { get; set; }


    public void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _enabledColor = _buttonImage.color;
    }


    public void SetEnabled(bool isEnabled)
    {
        var transparent = new Color(1f, 1f, 1f, 0.5f);
        _buttonImage.color = isEnabled ? Color.white : transparent;
        MoneyValueComponent.color = isEnabled ? Color.white : Color.red;
        NameComponent.color = isEnabled ? Color.white : Color.red;
        ImageComponent.color = isEnabled ? Color.white : transparent;
    }
}
