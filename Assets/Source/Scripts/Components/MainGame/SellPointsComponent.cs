using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using DG.Tweening;
using Kuhpik;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

public class SellPointsComponent : MonoBehaviour
{
    [field: SerializeField] public Transform[] SellPoints { get; private set; }
    [SerializeField] private CoinComponent[] _coins;
    [SerializeField] private ParticleSystem VFX;
    [SerializeField] private ParticleSystem _successfulSaleVFX;
    public int FreePlace { get; private set; }

    private LevelComponent _levelComponent;
    private List<SkeletonAnimation> _coinsAnimation = new List<SkeletonAnimation>();
    private List<GameObject> _resources = new List<GameObject>();
    private List<int> _income = new List<int>();
    private List<ParticleSystem> _sellVFX = new List<ParticleSystem>();
    private ParticleSystem _successfulSale;
    private PlayerData _playerData;

    private int _total;

    private void Awake()
    {
        _playerData = Bootstrap.Instance.PlayerData;
        _successfulSale = Instantiate(_successfulSaleVFX);
        _successfulSale.Stop();
        _levelComponent = GetComponentInParent<LevelComponent>();

        for (int i = 0; i < SellPoints.Length; i++)
        {
            _coins[i].transform.parent = SellPoints[i];
            _coins[i].transform.position = SellPoints[i].position;
            _coins[i].gameObject.SetActive(false);

            _coinsAnimation.Add(_coins[i].GetComponent<CoinComponent>().SkeletonAnimation);

            _sellVFX.Add(Instantiate(VFX));
            _sellVFX[i].transform.parent = SellPoints[i];
            _sellVFX[i].transform.position = _coins[i].transform.position;
            _sellVFX[i].Stop();
        }
    }

    public void SetCoinsTarget()
    {
        Vector3 coinsTarget = _levelComponent.CoinsTarget;

        for (int i = 0; i < SellPoints.Length; i++)
        {
            _coins[i].Target = coinsTarget;
        }
    }

    public void AddResource(ResourceComponent resourceComponent, int cost)
    {
        _resources.Add(resourceComponent.gameObject);
        resourceComponent.transform.DOMove(SellPoints[FreePlace].position, 0.5f).OnComplete(() =>
        {
            AudioManager.Instance.PlayAudio(AudioTypes.ResourceMoving);

            if (FreePlace >= SellPoints.Length)
            {
                FreePlace = 0;
                SellResources();
            }
        });

        resourceComponent.transform.parent = SellPoints[FreePlace];

        FreePlace++;
        _income.Add(cost);
    }

    private void SellResources()
    {
        GameObject[] resources = new GameObject[_resources.Count];
        int[] income = new int[_income.Count];

        for (int i = 0; i < resources.Length; i++)
        {
            resources[i] = _resources[i];
            income[i] = _income[i];
        }

        _resources.Clear();
        _income.Clear();

        StartCoroutine(AddCoins(resources, income));
    }

    private IEnumerator AddCoins(GameObject[] resources, int[] income)
    {
        for (int i = 0; i < SellPoints.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(resources[i]);

            _total = income[i];

            int chance = Random.Range(0, 100);

            if (chance <= _playerData.LuckySaleChance)
            {
                AudioManager.Instance.PlayAudio(AudioTypes.LuckySale);
                GameEvents.CheckTask_E?.Invoke(OnboardingTasks.LuckySale, new OnboardingTaskType[] { OnboardingTaskType.None });

                _total = income[i] * Mathf.RoundToInt(_playerData.LuckySaleIncome);
                _successfulSale.transform.parent = SellPoints[i];
                _successfulSale.transform.position = SellPoints[i].position;
                _successfulSale.Play();
            }

            _coins[i].transform.localPosition *= 0;
            _coins[i].gameObject.SetActive(true);
            _coins[i].AddCoins(_total, i);

            _sellVFX[i].Play();

            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.FillSalesArea, new OnboardingTaskType[] { OnboardingTaskType.None });
            AudioManager.Instance.PlayAudio(AudioTypes.SellResource);
        }
    }
}