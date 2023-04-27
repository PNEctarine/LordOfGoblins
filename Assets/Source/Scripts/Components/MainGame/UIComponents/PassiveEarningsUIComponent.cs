using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveEarningsUIComponent : MonoBehaviour
{
    public Button TakeButton;
    [SerializeField] private TextMeshProUGUI InfoTMP;
    [SerializeField] private TextMeshProUGUI IncomeTMP;
    [SerializeField] private TextMeshProUGUI[] _textMeshProUGUIs;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetInfo(string info, int income, bool isInternet)
    {
        InfoTMP.text = info;
        IncomeTMP.text = $"{income}<sprite=0> ";
        IncomeTMP.gameObject.SetActive(isInternet);
    }
}
