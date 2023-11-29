using DG.Tweening;
using Kuhpik;
using UnityEngine;

public class TutorialResourceSpawnSystem : GameSystemWithScreen<TutorialUI>
{
    [SerializeField] private ResourcesConfig _resourcesConfig;
    [SerializeField] private GameObject _resource;

    private bool _isSpawned;
    private int _spawnCount;
    private int _locksCount;

    public override void OnInit()
    {
        GameEvents.SpawnResource_E += SpawnResource;
        GameEvents.TutorialMerge_E += EnableResources;
        GameEvents.TutorialCompleate_E = TutorialCompleate;

        screen.ShopWindow.SetUp();
        GameEvents.CheckCost_E?.Invoke(0);

        _locksCount = game.LevelComponent.MergePointsComponent.LockComponents.Length;
        game.ResourceComponents = new ResourceComponent[game.ResourceBlockComponents.Length];

        for (int i = _locksCount - player.MergePointsLevel[player.LocationLevel]; i < game.ResourceBlockComponents.Length; i++)
        {
            if (i > 9 && i < 12 && _spawnCount < 1)
            {
                GameObject resource = Instantiate(_resource, game.LevelComponent.transform);
                resource.transform.position = game.ResourceBlockComponents[i].transform.position;
                resource.GetComponent<BoxCollider2D>().enabled = false;

                game.ResourceBlockComponents[i].SetKey(0.ToString());
                game.ResourceBlockComponents[i].SetResource(resource.GetComponent<ResourceComponent>(),
                    _resourcesConfig.Resource[0].Visuals,
                    _resourcesConfig.Resource[0].Glow,
                    0,
                    _resourcesConfig.Resource[0].ResourceName,
                    0,
                    _resourcesConfig.Resource[0].Cost);

                game.ResourceBlockComponents[i].IsResource = true;
                game.ResourceBlockComponents[i].ResourceComponent = resource.GetComponent<ResourceComponent>();
            }

            else if (game.ResourceBlockComponents[i].IsResource == false)
            {
                int random = Random.Range(4 * player.ResurcesSpawnLevel[player.LocationLevel], 4 * player.ResurcesSpawnLevel[player.LocationLevel] + 4);

                GameObject resource = Instantiate(_resource, game.LevelComponent.transform);
                resource.transform.position = game.ResourceBlockComponents[i].transform.position;
                resource.GetComponent<BoxCollider2D>().enabled = false;

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

                    else
                    {
                        int random;

                        do
                        {
                            random = Random.Range(4 * player.ResurcesSpawnLevel[player.LocationLevel], 4 * player.ResurcesSpawnLevel[player.LocationLevel] + 4);
                        } while (random.ToString() == last);

                        GameObject resource = Instantiate(_resource, game.LevelComponent.transform);
                        resource.transform.position = game.ResourceBlockComponents[i].transform.position;
                        _isSpawned = true;

                        game.ResourceBlockComponents[i].SetKey(random.ToString());
                        game.ResourceBlockComponents[i].SetResource(resource.GetComponent<ResourceComponent>(),
                            _resourcesConfig.Resource[random].Visuals,
                            _resourcesConfig.Resource[random].Glow,
                            0,
                            _resourcesConfig.Resource[random].ResourceName,
                            random,
                            _resourcesConfig.Resource[random].Cost);
                    }
                }
            }
        }

        if (_isSpawned && _spawnCount < 1)
        {
            _spawnCount++;
            GameEvents.TutorialNextStage_E?.Invoke();
        }
    }

    private void EnableResources()
    {
        for (int i = 10; i < 12; i++)
        {
            game.ResourceBlockComponents[i].ResourceComponent.GetComponent<BoxCollider2D>().enabled = true;
            //game.ResourceBlockComponents[i].ResourceComponent.GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
    }

    private void TutorialCompleate()
    {
        for (int i = 0; i < game.ResourceBlockComponents.Length; i++)
        {
            if (game.ResourceBlockComponents[i].ResourceComponent != null)
            {
                game.ResourceBlockComponents[i].ResourceComponent.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        player.IsTutorialCompelete = true;
        Bootstrap.Instance.SaveGame();

        GameEvents.TutorialMerge_E = null;
        GameEvents.SpawnResource_E = null;
        GameEvents.TutorialCompleate_E = null;

        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
    }
}
