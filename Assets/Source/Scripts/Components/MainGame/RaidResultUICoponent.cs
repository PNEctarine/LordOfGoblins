using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaidResultUICoponent : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _resultTMP;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        GameEvents.RaidResult_E += RaidResult;

        gameObject.SetActive(false);
    }

    private void RaidResult(string result)
    {
        gameObject.SetActive(true);
        _resultTMP.text = result;
    }
}
