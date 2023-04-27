using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class OnboardingCurrencyComponent : MonoBehaviour
{
    public void Play(Vector3 moveVector, Vector3 targetPosition)
    {
        gameObject.SetActive(true);

        gameObject.transform.DOMove(gameObject.transform.position + moveVector, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            gameObject.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
