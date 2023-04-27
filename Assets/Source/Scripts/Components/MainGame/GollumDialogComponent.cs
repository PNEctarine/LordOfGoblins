using TMPro;
using UnityEngine;

public class GollumDialogComponent: MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI GollumTMP { get; private set; }

    public void OnMouseDown()
    {
        gameObject.SetActive(false);
    }
}
