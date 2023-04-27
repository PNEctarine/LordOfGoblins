using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellsCardUIComponent : MonoBehaviour
{
    [SerializeField] private SpellTypes _spellType;

    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _costTMP;

    [SerializeField] private int _cost;
    [SerializeField] private int _percent;

    private void Start()
    {
        _buyButton.onClick.AddListener(() => GameEvents.BuySpell_E?.Invoke(_cost, _percent, _spellType));
    }
}
