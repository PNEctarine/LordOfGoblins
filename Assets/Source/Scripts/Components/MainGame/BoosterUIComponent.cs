using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUIComponent : MonoBehaviour
{
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ClaimTMP { get; private set; }

    [SerializeField] private LocalizationParamsManager _localizationParamsManager;
    [SerializeField] private Localize _localize;
    [SerializeField] private Image _boosterImage;

    private void Start()
    {
        Button.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    public void SetBoosterInfo(float persent, float duration, Sprite boosterSprite, string term)
    {
        _localizationParamsManager.SetParameterValue("PERCENT", persent.ToString());
        _localizationParamsManager.SetParameterValue("DURATION", duration.ToString());
        _boosterImage.sprite = boosterSprite;

        _localize.Term = term;

    }
}
