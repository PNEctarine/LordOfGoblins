using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using UnityEngine;

public class LevelLoadigSystem : GameSystemWithScreen<GameUI>
{
    [SerializeField] private LocationsConfig _locationsConfig;

    public override void OnInit()
    {
#if UNITY_EDITOR
        player.IsTutorialCompelete = true;
#endif

        DOTween.SetTweensCapacity(500, 50);

        LevelComponent levelComponent = Instantiate(_locationsConfig.LevelComponents[player.LocationLevel]);

        game.LevelComponent = levelComponent;
        game.LevelComponent.CoinsTarget = screen.SoftCurrencyBar.CoinsTarget.position;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        levelComponent.SellPointsComponent.SetCoinsTarget();
        levelComponent.LocationLevel = player.LocationLevel + 1;
        levelComponent.MergePointsComponent.AddPlace(levelComponent.MergePointsComponent.LockComponents.Length - player.MergePointsLevel[player.LocationLevel]);
        levelComponent.Init();

        game.SellPoints = new Transform[levelComponent.SellPointsComponent.SellPoints.Length];
        game.GoblinsCollectors = new List<GameObject>();
        game.GoblinMovementComponents = new List<GoblinMovementComponent>();

        game.GoblinWithCartComponent = levelComponent.GoblinWithCartComponent;
        game.CartWaypoints = new Transform[levelComponent.CartWaypoints.Length];
        game.GoblinsCollectors.Add(levelComponent.GoblinCollector);
        game.GoblinMovementComponents.Add(game.GoblinsCollectors[0].GetComponent<GoblinMovementComponent>());
        game.GoblinsCollectors[0].GetComponent<GoblinCollectorComponent>().MaxResources = player.CollectorCapacity[player.LocationLevel] + 1;
        game.ResourceBlockComponents = new ResourceBlockComponent[levelComponent.BlockComponents.Length];

        for (int i = 0; i < game.ResourceBlockComponents.Length; i++)
        {
            game.ResourceBlockComponents[i] = levelComponent.BlockComponents[i];
        }

        for (int i = 0; i < game.SellPoints.Length; i++)
        {
            game.SellPoints[i] = levelComponent.SellPointsComponent.SellPoints[i].transform;
        }

        for (int i = 0; i < levelComponent.CartWaypoints.Length; i++)
        {
            game.CartWaypoints[i] = levelComponent.CartWaypoints[i];
        }

        if (player.IsNotFirstLaunch == false)
        {
            player.CartSpeed = 20f;
            //player.CollectorSpeed = 2.5f;
            player.ChanceOfTreasure = 0.03f;
        }

        if (player.IsDialogue)
        {
            screen.ShopComponent.SetUp();
            screen.CloseAll();
            Bootstrap.Instance.ChangeGameState(GameStateID.Dialog);
        }

        else if (player.IsTutorialCompelete)
        {
            Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        }

        else if (player.IsTutorialCompelete == false)
        {
            Bootstrap.Instance.ChangeGameState(GameStateID.Tutorial);
        }

        //else if (player.IsDialogue)
        //{
        //    Bootstrap.Instance.ChangeGameState(GameStateID.Dialog);
        //}
    }
}