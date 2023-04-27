using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class GoblinCollectorSystem : GameSystem
{
    private float _boost;

    private int _sellPointIndex;
    private int _maxLevel = 5;

    private bool _isBoost;
    private bool _isStop;

    Vector2 _targetPosition = new Vector2(0, -2);

    public override void OnStateEnter()
    {
        for (int i = 0; i < game.GoblinsCollectors.Count; i++)
        {
            game.GoblinMovementComponents[i].Stop(false);
        }

        GameEvents.CollectorBoost_E += CollectorBoost;
    }

    public override void OnStateExit()
    {
        for (int i = 0; i < game.GoblinsCollectors.Count; i++)
        {
            game.GoblinMovementComponents[i].Stop(true);
        }

        GameEvents.CollectorBoost_E -= CollectorBoost;
    }

    public override void OnUpdate()
    {
        for (int g = 0; g < game.GoblinsCollectors.Count; g++)
        {
            if (game.GoblinMovementComponents[g].IsGrab && game.GoblinMovementComponents[g].IsPath == false)
            {
                game.GoblinMovementComponents[g].IsPath = true;
                _sellPointIndex = game.LevelComponent.SellPointsComponent.FreePlace;
                _targetPosition = game.SellPoints[_sellPointIndex].position;

                //if (_isBoost && _isStop == false)
                //{
                //    game.GoblinMovementComponents[g].CollectorBoost(0);
                //}

                if (_isStop == false)
                {
                    game.GoblinMovementComponents[g].MoveCollector(_targetPosition, player.CollectorSpeed - player.CollectorSpeed * _boost / 100);
                }
            }

            else if (game.GoblinMovementComponents[g].IsGrab == false && game.GoblinMovementComponents[g].IsPath == false && _isStop == false)
            {
                _targetPosition = new Vector2(0, -2);
                int maxLevel = _maxLevel;
                List<int> ind = new List<int>();

                for (int l = 0; l <= _maxLevel; l++)
                {
                    for (int i = 10; i < game.ResourceBlockComponents.Length; i++)
                    {
                        if (game.ResourceBlockComponents[i].IsResource == true && game.ResourceBlockComponents[i].MergeLevel == maxLevel || game.ResourceBlockComponents[i].Key == ResourceKeys.TreasureKey
                            && game.GoblinMovementComponents[g].IsPath == false
                            && game.ResourceBlockComponents[i].Key != ResourceKeys.CouponKey)
                        {
                            if (game.ResourceBlockComponents[i].Key == ResourceKeys.TreasureKey)
                            {
                                ind.Clear();
                                ind.Add(i);
                                break;
                            }

                            ind.Add(i);
                        }
                    }

                    if (ind.Count == 0 && game.GoblinMovementComponents[g].IsPath == false)
                    {
                        maxLevel--;
                    }

                    else if (ind.Count > 0 && game.GoblinMovementComponents[g].IsPath == false)
                    {
                        int i = Random.Range(0, ind.Count);
                        _targetPosition = new Vector3(game.ResourceBlockComponents[ind[i]].transform.position.x, 0.2f, 0);
                        game.GoblinMovementComponents[g].GetComponent<GoblinCollectorComponent>().SetKeyID(game.ResourceBlockComponents[ind[i]].Index);
                        game.GoblinMovementComponents[g].IsPath = true;
                        ind.Clear();
                    }
                }

                //if (_isBoost)
                //{
                //    game.GoblinMovementComponents[g].CollectorBoost(0);
                //}

                game.GoblinMovementComponents[g].MoveCollector(_targetPosition, player.CollectorSpeed - player.CollectorSpeed * _boost / 100);
            }
        }
    }

    private void CollectorBoost(float percent)
    {
        _boost += percent;
    }
}