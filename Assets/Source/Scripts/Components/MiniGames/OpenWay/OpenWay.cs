using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OpenWay : MonoBehaviour
{
    [SerializeField] GameObject _goblin;

    public void SetPath(Vector3[] path)
    {
        _goblin.transform.DOPath(path, 2f);
    }
}
