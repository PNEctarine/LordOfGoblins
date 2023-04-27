using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCDUIComponent : MonoBehaviour
{
    [field: SerializeField] public Image SpellImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Timer { get; private set; }
}
