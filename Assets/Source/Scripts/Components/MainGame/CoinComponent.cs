using Code.Enums;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class CoinComponent : MonoBehaviour
{
    [field: SerializeField] public SkeletonAnimation SkeletonAnimation { get; private set; }
    [HideInInspector] public Vector3 Target;

    public void AddCoins(int income, int index)
    {
        gameObject.transform.localPosition *= 0;

        gameObject.transform.DOJump(gameObject.transform.position + Vector3.one * Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.4f), 1, 1).OnComplete(() =>
        {
            SkeletonAnimation.AnimationName = "idle";
            gameObject.transform.DOMove(Target, 2f).OnComplete(() =>
            {
                GameEvents.AddCoins_E?.Invoke(income);
                gameObject.SetActive(false);

                if (index == 1)
                    AudioManager.Instance.PlayAudio(AudioTypes.CreditingCoins);
            });
        });
    }
}
