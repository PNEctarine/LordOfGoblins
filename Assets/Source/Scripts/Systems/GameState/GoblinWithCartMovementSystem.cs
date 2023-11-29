using System.Collections;
using Code.Enums;
using DG.Tweening;
using Kuhpik;
using Spine.Unity;
using UnityEngine;

public class GoblinWithCartMovementSystem : GameSystem
{
    private const float _pathDuration = 7.5f;

    private Vector3[] _points;
    private bool _isDelivering;
    private bool _isDeliver;
    private bool _isState;

    public override void OnInit()
    {
        _points = new Vector3[game.CartWaypoints.Length];
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = game.CartWaypoints[i].position;
        }
    }

    public override void OnUpdate()
    {
        if (_isDelivering == true)
        {
            _isDelivering = false;
            GameEvents.GatesAnimation_E?.Invoke();

            game.GoblinWithCartComponent.transform.DOScale(Vector3.one * 1f, _pathDuration);
            game.GoblinWithCartComponent.transform.DOMoveY(_points[0].y, _pathDuration).OnComplete(() =>
            {
                AudioManager.Instance.PlayAudio(AudioTypes.Steps);
                _isDeliver = false;

                if (_isState)
                {
                    game.GoblinWithCartComponent.MoveGoblin(_points[1].y);
                }

                else
                {
                    game.GoblinWithCartComponent.StopGoblin(_points[0].y);
                }
            });
        }

        else if (_isDeliver == false)
        {
            _isDeliver = true;
            GameEvents.GatesAnimation_E?.Invoke();

            game.GoblinWithCartComponent.transform.DOScale(Vector3.one * 0.6f, _pathDuration);
            game.GoblinWithCartComponent.transform.DOMoveY(_points[1].y, _pathDuration).OnComplete(() =>
            {
                if (_isState)
                {
                    game.GoblinWithCartComponent.MoveGoblin(_points[0].y);
                    StartCoroutine(CartCooldown());
                }

                else
                {
                    game.GoblinWithCartComponent.StopGoblin(_points[1].y);
                }
            });
        }
    }

    public override void OnStateEnter()
    {
        _isState = true;
        game.GoblinWithCartComponent.transform.DOKill();
        if (_isDelivering == false && _isDeliver == false)
        {
            _isDeliver = false;
            game.GoblinWithCartComponent.MoveGoblin(_points[1].y);

        }

        else if (_isDeliver)
        {
            _isDelivering = true;
            game.GoblinWithCartComponent.MoveGoblin(_points[0].y);
        }
    }

    public override void OnStateExit()
    {
        _isState = false;
        //game.GoblinWithCartComponent.transform.DOKill();
        //StopAllCoroutines();

        //if (_isDelivering == true)
        //{
        //    game.GoblinWithCartComponent.StopGoblin(_points[1].y);
        //    _isDeliver = false;
        //}

        //else if (_isDeliver == true)
        //{
        //    game.GoblinWithCartComponent.StopGoblin(_points[0].y);
        //    _isDelivering = true;
        //}
    }

    private IEnumerator CartCooldown()
    {
        yield return new WaitForSeconds(2);
        _isDelivering = true;
    }
}
