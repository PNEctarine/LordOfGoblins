using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using UnityEngine;

public class SpawnResourcesSystem : GameSystem
{
    [SerializeField] private ResourcesConfig _resourcesConfig;
    [SerializeField] private TreasureConfig _treasureConfig;
    [SerializeField] private CouponsConfig _couponsConfig;
    [SerializeField] private GameObject _resource;

    private int _locksCount;
    private float _timer;

    public override void OnInit()
    {
        GameEvents.SpawnResource_E += SpawnResource;
        _locksCount = game.LevelComponent.MergePointsComponent.LockComponents.Length;
        game.ResourceComponents = new ResourceComponent[game.ResourceBlockComponents.Length];

        for (int i = _locksCount - player.MergePointsLevel[player.LocationLevel]; i < game.ResourceBlockComponents.Length; i++)
        {
            if (game.ResourceBlockComponents[i].IsResource == false)
            {
                int random = Random.Range(4 * player.ResurcesSpawnLevel[player.LocationLevel], 4 * player.ResurcesSpawnLevel[player.LocationLevel] + 4);

                GameObject resource = Instantiate(_resource, game.LevelComponent.transform);
                resource.transform.position = game.ResourceBlockComponents[i].transform.position;

                game.ResourceBlockComponents[i].SetKey(random.ToString());
                game.ResourceBlockComponents[i].SetResource(resource.GetComponent<ResourceComponent>(),
                    _resourcesConfig.Resource[random].Visuals,
                    _resourcesConfig.Resource[random].Glow,
                    0,
                    _resourcesConfig.Resource[random].ResourceName,
                    random,
                    _resourcesConfig.Resource[random].Cost);

                game.ResourceBlockComponents[i].ResourceComponent = resource.GetComponent<ResourceComponent>();
            }
        }
    }

    public override void OnUpdate()
    {
        _timer += 1 * Time.deltaTime;
    }

    private void SpawnResource(string last)
    {
        for (int lines = 0; lines < 3; lines++)
        {
            for (int i = _locksCount - player.MergePointsLevel[player.LocationLevel]; i < game.ResourceBlockComponents.Length; i++)
            {
                if (game.ResourceBlockComponents[i].IsResource == false)
                {
                    if (i - 5 >= _locksCount - player.MergePointsLevel[player.LocationLevel])
                    {
                        ResourceBlockComponent resourceBlockComponent = game.ResourceBlockComponents[i - 5];
                        ResourceComponent resourceComponent = resourceBlockComponent.ResourceComponent;

                        game.ResourceBlockComponents[i].SetKey(resourceBlockComponent.Key);

                        game.ResourceBlockComponents[i].SetResource(resourceBlockComponent.ResourceComponent,
                            resourceBlockComponent.ResourceComponent.ResourceVisual,
                            resourceBlockComponent.ResourceComponent.ResourceGlow,
                            resourceBlockComponent.MergeLevel,
                            resourceBlockComponent.ResourceName,
                            resourceBlockComponent.ResourceLevel,
                            resourceBlockComponent.ResourceComponent.Cost);

                        if (resourceComponent.IsCollected == false)
                        {
                            resourceComponent.transform.DOKill();
                            resourceComponent.transform.DOMove(game.ResourceBlockComponents[i].transform.position, 0.5f);
                        }

                        resourceBlockComponent.IsResource = false;
                    }

                    else if (i - 5 < 0 && _timer >= 10 * 60)
                    {
                        _timer = 0f;
                        GameObject resource = Instantiate(_resource, game.LevelComponent.BlockComponents[i].transform);
                        resource.transform.position = game.ResourceBlockComponents[i].transform.position;

                        game.ResourceBlockComponents[i].SetKey(ResourceKeys.CouponKey);
                        game.ResourceBlockComponents[i].SetCoupon(resource.GetComponent<ResourceComponent>(),
                            _couponsConfig.Coupons[0].Visual,
                            _couponsConfig.Coupons[0].Glow,
                            _couponsConfig.Coupons[0].CouponParticle,
                            _couponsConfig.LostParticle,
                            _couponsConfig.Coupons[0].Percent);
                    }

                    else
                    {
                        float chance = Random.Range(0f, 1f);

                        if (chance > player.ChanceOfTreasure)
                        {
                            int random;

                            do
                            {
                                random = Random.Range(4 * player.ResurcesSpawnLevel[player.LocationLevel], 4 * player.ResurcesSpawnLevel[player.LocationLevel] + 4);
                            } while (random.ToString() == last);

                            GameObject resource = Instantiate(_resource, game.LevelComponent.transform);
                            resource.transform.position = game.ResourceBlockComponents[i].transform.position;

                            game.ResourceBlockComponents[i].SetKey(random.ToString());
                            game.ResourceBlockComponents[i].SetResource(resource.GetComponent<ResourceComponent>(),
                                _resourcesConfig.Resource[random].Visuals,
                                _resourcesConfig.Resource[random].Glow,
                                0,
                                _resourcesConfig.Resource[random].ResourceName,
                                random,
                                _resourcesConfig.Resource[random].Cost);
                        }

                        else
                        {
                            GameObject resource = Instantiate(_resource, game.LevelComponent.transform);

                            resource.GetComponent<SpriteRenderer>().sprite = _treasureConfig.Treasures[0].Visual[0];
                            resource.transform.position = game.ResourceBlockComponents[i].transform.position;

                            game.ResourceBlockComponents[i].SetKey(ResourceKeys.TreasureKey);
                            game.ResourceBlockComponents[i].SetResource(resource.GetComponent<ResourceComponent>(),
                                _treasureConfig.Treasures[player.ResurcesSpawnLevel[player.LocationLevel]].Visual,
                                _treasureConfig.Treasures[player.ResurcesSpawnLevel[player.LocationLevel]].Glow,
                                0,
                                "Treasure",
                                -1,
                                _treasureConfig.Treasures[player.TreasureLevel[player.LocationLevel]].Cost);
                            game.ResourceBlockComponents[i].ResourceComponent = resource.GetComponent<ResourceComponent>();
                        }
                    }
                }
            }
        }
    }

    public void SpawnCoupons()
    {
        List<int> usedIndexes = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int random;
            do
            {
                random = Random.Range(_locksCount - player.MergePointsLevel[player.LocationLevel], game.ResourceBlockComponents.Length);
            } while (usedIndexes.Contains(random));

            usedIndexes.Add(random);
            GameObject resource = game.ResourceBlockComponents[random].ResourceComponent.gameObject;

            game.ResourceBlockComponents[random].SetKey(ResourceKeys.CouponKey);
            game.ResourceBlockComponents[random].SetCoupon(resource.GetComponent<ResourceComponent>(),
                _couponsConfig.Coupons[0].Visual,
                _couponsConfig.Coupons[0].Glow,
                _couponsConfig.Coupons[0].CouponParticle,
                _couponsConfig.LostParticle,
                _couponsConfig.Coupons[0].Percent);
        }
    }
}