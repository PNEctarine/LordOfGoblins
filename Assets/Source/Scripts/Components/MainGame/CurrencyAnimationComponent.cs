using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CurrencyAnimationComponent : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void Play(Vector3 moveVector, Vector3 targetPosition)
    {
        gameObject.SetActive(true);

        _rectTransform.DOMove(_rectTransform.position + moveVector, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            _rectTransform.DOMove(targetPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
