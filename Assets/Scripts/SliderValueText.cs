using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField] TMP_Text uguiText;

    void Awake()
    {
        var slider = GetComponent<Slider>();
        UpdateText(slider.value);
        slider.onValueChanged.AddListener((float value) =>
        {
            UpdateText(value);
        });
    }

    void UpdateText(float value)
    {
        uguiText.text = value.ToString("F2"); // Format to 2 decimal places //15.15
    }
}
