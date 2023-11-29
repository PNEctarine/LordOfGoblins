using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class GoblinWithCartComponent : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation[] _animations;
    private int _layer;

    private void Start()
    {
        _layer = 2;
        GameEvents.CartGoblinLayer_E += ChangeLayer;
    }

    public void MoveGoblin(float pos)
    {
        if (pos > gameObject.transform.position.y)
        {
            _animations[0].gameObject.SetActive(false);
            _animations[1].gameObject.SetActive(true);
            _animations[1].state.AddAnimation(0, "Run_1", true, 1);
        }

        else
        {
            _animations[0].gameObject.SetActive(true);
            _animations[0].state.AddAnimation(0, "Run_1", true, 1);
            _animations[1].gameObject.SetActive(false);
        }
    }

    public void StopGoblin(float pos)
    {
        if (pos > gameObject.transform.position.y)
        {
            _animations[0].gameObject.SetActive(false);
            _animations[1].gameObject.SetActive(true);
            _animations[1].state.AddAnimation(0, "Idle_2", true, 1);
        }

        else
        {
            _animations[0].gameObject.SetActive(true);
            _animations[0].state.AddAnimation(0, "Idle_1", true, 1);
            _animations[1].gameObject.SetActive(false);
        }
    }

    private void ChangeLayer()
    {
        if (_layer == 1)
        {
            _layer = 2;
        }

        else
        {
            _layer = 1;
        }

        for (int i = 0; i < _animations.Length; i++)
        {
            _animations[i].GetComponent<MeshRenderer>().sortingOrder = _layer;
        }
    }
}
