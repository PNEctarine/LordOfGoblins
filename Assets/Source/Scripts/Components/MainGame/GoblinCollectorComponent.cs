using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using DG.Tweening;
using Kuhpik;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoblinCollectorComponent : MonoBehaviour, IPointerClickHandler
{
    public int MaxResources;
    [SerializeField] private ParticleSystem _sellEffect;

    private int _respurcesCount;
    private int _resourceKeyID;
    private int _count;

    private List<ResourceComponent> _grabbedResource = new List<ResourceComponent>();
    private ResourceBlockComponent _resourceBlockComponent;
    private GoblinMovementComponent _goblinMovementComponent;
    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = Bootstrap.Instance.PlayerData;
        _goblinMovementComponent = gameObject.GetComponent<GoblinMovementComponent>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ResourceBlockComponent resourceBlockComponent) && resourceBlockComponent.IsResource && resourceBlockComponent.Index == _resourceKeyID
            && resourceBlockComponent.ResourceComponent.IsCollected == false && _goblinMovementComponent.IsGrab == false)
        {
            if (resourceBlockComponent.Key != ResourceKeys.CouponKey)
            {
                AudioManager.Instance.PlayAudio(AudioTypes.TakingResource);

                _resourceBlockComponent = resourceBlockComponent;

                _grabbedResource.Add(resourceBlockComponent.ResourceComponent);
                _grabbedResource[_respurcesCount].transform.parent = gameObject.transform;
                _grabbedResource[_respurcesCount].GetComponent<BoxCollider2D>().enabled = false;
                _grabbedResource[_respurcesCount].IsCollector = true;
                _grabbedResource[_respurcesCount].gameObject.SetActive(false);

                _resourceBlockComponent.ResourceComponent.IsCollected = true;
                _resourceBlockComponent.ResourceComponent = null;
                _resourceBlockComponent.IsResource = false;

                GameEvents.SpawnResource_E?.Invoke(_grabbedResource[_respurcesCount].Key);
                _respurcesCount++;

                _goblinMovementComponent.GrabSet(false);

                if (_respurcesCount >= MaxResources)
                {
                    _respurcesCount = 0;
                    _goblinMovementComponent.GrabSet(true);
                    CheckInstantSale();
                }
            }

            else
                _goblinMovementComponent.IsPath = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out SellPointsComponent sellPointsComponent) && _grabbedResource != null && _goblinMovementComponent != null)
        {
            for (int i = 0; i < _grabbedResource.Count; i++)
            {
                _grabbedResource[i].transform.DOKill();
                sellPointsComponent.AddResource(_grabbedResource[i], _grabbedResource[i].Cost[_grabbedResource[i].MergeLevel]);
            }

            _goblinMovementComponent.GrabSet(false);
            DropResource();
        }
    }

    private void CheckInstantSale()
    {
        for (int i = 0; i < _grabbedResource.Count; i++)
        {
            float chance = Random.Range(0, 100);

            if (chance <= _playerData.InstantSaleChance)
            {
                GameEvents.AddCoins_E(_grabbedResource[i].Cost[_grabbedResource[i].MergeLevel]);

                Destroy(_grabbedResource[i].gameObject);
                _grabbedResource.RemoveAt(i);
                _sellEffect.Play();
                _goblinMovementComponent.GrabSet(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        _respurcesCount++;
        //_goblinMovementComponent.CollectorBoost(_respurcesCount);

        StopAllCoroutines();

        if (_respurcesCount < 3)
        {
            StartCoroutine(ClicksReset());
        }

        else
        {
            _respurcesCount = 0;
        }
    }

    public void SetKeyID(int keyID)
    {
        _resourceKeyID = keyID;
    }

    public void DropResource()
    {
        for (int i = 0; i < _grabbedResource.Count; i++)
        {
            _grabbedResource[i].gameObject.SetActive(true);
            _grabbedResource[i].transform.localScale = Vector3.one * 1.7f;
            _grabbedResource[i] = null;
        }

        _grabbedResource.Clear();
    }

    private IEnumerator ClicksReset()
    {
        yield return new WaitForSeconds(1f);
        _respurcesCount = 0;
    }
}