using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LineComponent : MonoBehaviour
{
    [field: SerializeField] public TrailRenderer TrailRenderer { get; private set; }
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private List<Vector3> WayPoints = new List<Vector3>();
    public int TouchCount { get; private set; }
    public int Count { get; private set; }
    public float Length { get; private set; } = 1.27f;

    private List<int> _mergeIndexes = new List<int>();
    private int _totalLevel;
    private string _keyID;
    private ResourceComponent _lastResourceComponent;
    private ResourceBlockComponent _firstResourceBlockComponent;
    private List<ResourceComponent> _resources = new List<ResourceComponent>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ResourceComponent resourceComponent) && resourceComponent.IsCollected == false && resourceComponent.IsCollector == false && resourceComponent.KeyID == _keyID)
        {
            _totalLevel += resourceComponent.MergeLevel + 1;

            if (_totalLevel <= 2 && resourceComponent.KeyID == _keyID)
            {
                for (int i = 0; i < _mergeIndexes.Count; i++)
                {
                    if (resourceComponent.ResourceBlockComponent.Index == _mergeIndexes[i])
                    {
                        Count += resourceComponent.MergeLevel + 1;
                        resourceComponent.IsCollected = true;
                        resourceComponent.transform.DOScale(1.2f, 0.1f);
                        _resources.Add(resourceComponent);
                        _lastResourceComponent = resourceComponent;

                        SetMergeIndexes();
                    }
                }
            }

            else if (_totalLevel >= 3)
            {
                _totalLevel = 0;
            }
        }
    }

    private void SetMergeIndexes()
    {
        _mergeIndexes.Clear();
        _mergeIndexes = new List<int>
        {
            _lastResourceComponent.ResourceBlockComponent.Index - 1, _lastResourceComponent.ResourceBlockComponent.Index + 1,
            _lastResourceComponent.ResourceBlockComponent.Index - 5, _lastResourceComponent.ResourceBlockComponent.Index + 5,
            _lastResourceComponent.ResourceBlockComponent.Index + 5 - 1, _lastResourceComponent.ResourceBlockComponent.Index + 5 + 1,
            _lastResourceComponent.ResourceBlockComponent.Index - 5 - 1, _lastResourceComponent.ResourceBlockComponent.Index - 5 + 1
        };
    }

    public void Set(string keyID, Vector3 position, ResourceBlockComponent resourceBlockComponent)
    {
        TouchCount++;

        gameObject.transform.position = position;

        _keyID = keyID;
        _firstResourceBlockComponent = resourceBlockComponent;
        _lastResourceComponent = resourceBlockComponent.ResourceComponent;

        TrailRenderer.Clear();
        ColliderSwitch(true);
        SetMergeIndexes();
    }

    public List<Vector3> SetWayPoints()
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            WayPoints.Add(_resources[i].ResourceBlockComponent.transform.position);
        }

        return WayPoints;
    }

    public ResourceBlockComponent Clear(bool isDestroy)
    {
        ResourceBlockComponent resourceBlockComponent = null;

        if (isDestroy)
        {
            resourceBlockComponent = _resources[_resources.Count - 1].ResourceBlockComponent;
            StartCoroutine(DestroyResources());
        }

        else if (_resources.Count == 0)
        {
            StopDestroy();

            ResetTouches();
        }

        ColliderSwitch(false);
        return resourceBlockComponent;
    }

    public void ResetTouches()
    {
        _mergeIndexes.Clear();
        _resources.Clear();
        WayPoints.Clear();
        SetWayPoints().Clear();

        TouchCount = 0;
        Count = 0;
        _totalLevel = 0;
    }

    public void ColliderSwitch(bool isEnabled)
    {
        _collider2D.enabled = isEnabled;
    }

    public void StopDestroy()
    {
        StopCoroutine(DestroyResources());
    }

    private IEnumerator DestroyResources()
    {
        int count = 0;
        int resourcesCount = _resources.Count;
        string keyID = _resources[count].KeyID;
        List<ResourceBlockComponent> upperBlockComponents = new List<ResourceBlockComponent>();
        _firstResourceBlockComponent.IsResource = false;

        while (count < resourcesCount)
        {
            yield return new WaitForSeconds(0.25f);

            if (count != resourcesCount - 1)
            {
                _resources[count].ResourceBlockComponent.IsResource = false;
            }

            _resources[count].transform.DOKill();
            GameEvents.SpawnResource_E?.Invoke(keyID);
            Destroy(_resources[count].gameObject);
            count++;
        }

        if (upperBlockComponents.Count > 0)
        {
            for (int i = 0; i < upperBlockComponents.Count; i++)
            {
                upperBlockComponents[i].IsResource = false;
            }

            GameEvents.SpawnResource_E?.Invoke(keyID);
        }

        ResetTouches();
    }
}