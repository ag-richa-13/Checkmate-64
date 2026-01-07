using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonStyler : MonoBehaviour
{
    void Awake()
    {
        Button btn = GetComponent<Button>();
        ColorBlock cb = btn.colors;

        cb.normalColor = Hex("#4F46E5");
        cb.highlightedColor = Hex("#4338CA");
        cb.pressedColor = Hex("#3730A3");
        cb.selectedColor = cb.highlightedColor;
        cb.disabledColor = new Color(0.4f, 0.4f, 0.4f);

        btn.colors = cb;

        TMPFix();
    }

    void TMPFix()
    {
        TMPro.TMP_Text t = GetComponentInChildren<TMPro.TMP_Text>();
        if (t != null)
            t.color = Hex("#F9FAFB");
    }

    Color Hex(string h)
    {
        ColorUtility.TryParseHtmlString(h, out Color c);
        return c;
    }
}
