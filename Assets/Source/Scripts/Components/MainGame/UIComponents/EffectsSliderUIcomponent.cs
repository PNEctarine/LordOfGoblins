using UnityEngine;
using UnityEngine.UI;

public class EffectsSliderUIcomponent : MonoBehaviour
{
    private Slider _slider;
    private const string _effectsKey = "EffectsValue";

    public void SetSetting()
    {
        _slider = GetComponent<Slider>();

        if (PlayerPrefs.HasKey(_effectsKey))
        {
            float sliderValue = PlayerPrefs.GetFloat(_effectsKey);
            _slider.value = sliderValue;
            AudioManager.Instance.EffectsVolume(sliderValue);
        }

        _slider.onValueChanged.AddListener(delegate
        {
            AudioManager.Instance.EffectsVolume(_slider.value);
        });
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(_effectsKey, _slider.value);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            PlayerPrefs.SetFloat(_effectsKey, _slider.value);
    }
}
