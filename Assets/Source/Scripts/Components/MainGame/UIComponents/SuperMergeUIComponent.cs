using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperMergeUIComponent : MonoBehaviour
{
    [field: SerializeField] public Button GetSuperMergeButton { get; private set; }
    [SerializeField] private RectTransform _crossPosition;
    public Vector2 CrossPosition { get; private set; }

    private void Awake()
    {
        CrossPosition = _crossPosition.position;
        gameObject.SetActive(false);
    }
}
