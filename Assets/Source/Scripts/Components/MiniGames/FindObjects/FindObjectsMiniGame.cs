using DG.Tweening;
using UnityEngine;

public class FindObjectsMiniGame : MonoBehaviour
{
    [SerializeField] private Transform[] _objectsImage;

    public void MoveObject(Transform foundObject, int index)
    {
        foundObject.DOMove(_objectsImage[index].position, 1f);
    }
}
