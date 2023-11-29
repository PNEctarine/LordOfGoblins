using UnityEngine;

public class ResourceBlockComponent : MonoBehaviour
{
    [field: SerializeField] public int Index { get; private set; }

    public int MergeLevel { get; private set; }
    public int ResourceLevel { get; private set; }
    public string ResourceName { get; private set; }
    public ResourceComponent ResourceComponent;

    public string KeyID { get; private set; }
    [field: SerializeField] public bool IsResource { get; set; }

    public string Key { get; private set; }

    private bool _isSuperMerge;

    public void SetResource(ResourceComponent resourceComponent, Sprite[] visual, Sprite[] glow, int mergeLevel, string resourceName, int resourceLevel, int[] cost)
    {
        gameObject.transform.localScale = Vector3.one;

        KeyID = Key + resourceComponent.MergeLevel;
        IsResource = true;
        MergeLevel = mergeLevel;
        ResourceName = resourceName;
        ResourceLevel = resourceLevel;

        ResourceComponent = resourceComponent;
        ResourceComponent.transform.parent = gameObject.transform;
        ResourceComponent.ResourceName = resourceName;
        ResourceComponent.ResourceLevel = resourceLevel;
        ResourceComponent.Cost = new int[cost.Length];
        ResourceComponent.Cost = cost;
        ResourceComponent.ResourceVisual = visual;
        ResourceComponent.ResourceGlow = glow;
        ResourceComponent.ResourceBlockComponent = this;
        ResourceComponent.SetKey(Key, MergeLevel);
        ResourceComponent.IsSuperMerge = _isSuperMerge;
    }

    public void SetCoupon(ResourceComponent resourceComponent, Sprite visual, Sprite glow, ParticleSystem couponVFX, ParticleSystem lostCouponVFX, int percent)
    {
        gameObject.transform.localScale = Vector3.one;

        KeyID = Key + resourceComponent.MergeLevel;

        ResourceComponent = resourceComponent;
        ResourceComponent.CouponVisual = visual;
        ResourceComponent.CouponGlow = glow;
        ResourceComponent.CouponVFX = couponVFX;
        ResourceComponent.LostCouponVFX = lostCouponVFX;
        ResourceComponent.CouponPercent = percent;
        ResourceComponent.ResourceBlockComponent = this;
        ResourceComponent.MergeLevel = 6;

        MergeLevel = 6;
        IsResource = true;

        ResourceComponent.SetKey(Key, MergeLevel);
    }

    public void SuperMerge(bool isSuperMerge)
    {
        _isSuperMerge = isSuperMerge;

        if (ResourceComponent != null)
            ResourceComponent.IsSuperMerge = _isSuperMerge;
    }

    public void SetKey(string key)
    {
        Key = key.ToString();
    }
}
