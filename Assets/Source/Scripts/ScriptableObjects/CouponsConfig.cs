using System;
using UnityEngine;

[Serializable]
public struct Coupon
{
    [Tooltip("Шанс выпадения купона")] public int Chance;
    [Tooltip("Процент увеличения")] public int Percent;
    [Tooltip("Визуал купона")] public Sprite Visual;
    [Tooltip("Свечение купона")] public Sprite Glow;
    [Tooltip("Эффект купона")] public ParticleSystem CouponParticle;
}

[CreateAssetMenu(menuName = "Config/CouponsConfig")]
public class CouponsConfig : ScriptableObject
{
    public ParticleSystem LostParticle;
    public Coupon[] Coupons;
}
