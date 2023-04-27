using UnityEngine;
using UnityEngine.UI;

public class MusicSliderUIComponent : MonoBehaviour
{
    private Slider _slider;
    private const string _soundKey = "SoundValue";

    public void SetSetting()
    {
        _slider = GetComponent<Slider>();

        if (PlayerPrefs.HasKey(_soundKey))
        {
            float sliderValue = PlayerPrefs.GetFloat(_soundKey);
            _slider.value = sliderValue;
            AudioManager.Instance.SoundVolume(sliderValue);
        }

        _slider.onValueChanged.AddListener(delegate
        {
            AudioManager.Instance.SoundVolume(_slider.value);
        });
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(_soundKey, _slider.value);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            PlayerPrefs.SetFloat(_soundKey, _slider.value);
    }
}
