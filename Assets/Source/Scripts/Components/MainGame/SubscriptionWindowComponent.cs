using UnityEngine;
using UnityEngine.UI;

public class SubscriptionWindowComponent : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    private void Update()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
