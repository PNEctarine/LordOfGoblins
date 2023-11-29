using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class SuperMergeSystem : GameSystemWithScreen<GameUI>
{
    private bool _isRewardRequested;

    public override void OnStateEnter()
    {
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        screen.SuperMergeUIComponent.GetSuperMergeButton.onClick.AddListener(() =>
        {
            _isRewardRequested = true;
            ApplovinSDK.Instance.ShowRewardedAd();
        });
    }

    public override void OnStateExit()
    {
        screen.SuperMergeUIComponent.GetSuperMergeButton.onClick.RemoveAllListeners();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        if (_isRewardRequested)
        {
            _isRewardRequested = false;
            screen.CloseAll();
            SuperMerge();
        }
    }

    private void SuperMerge()
    {
        screen.SuperMergeButton.gameObject.SetActive(false);

        for (int i = 0; i < game.ResourceBlockComponents.Length; i++)
        {
            game.ResourceBlockComponents[i].SuperMerge(true);
        }

        StartCoroutine(SuperMergeCoolDown());
        StartCoroutine(SuperMergeActivate());
    }

    private IEnumerator SuperMergeActivate()
    {
        yield return new WaitForSeconds(5 * 60);

        for (int i = 0; i < game.ResourceBlockComponents.Length; i++)
        {
            game.ResourceBlockComponents[i].SuperMerge(false);
        }
    }

    private IEnumerator SuperMergeCoolDown()
    {
        yield return new WaitForSeconds(15 * 60);
        screen.SuperMergeButton.gameObject.SetActive(true);
    }
}
