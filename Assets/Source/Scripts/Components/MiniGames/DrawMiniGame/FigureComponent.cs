using UnityEngine;

public class FigureComponent : MonoBehaviour
{
    [field: SerializeField] public Transform StartPoint { get; private set; }
    [field: SerializeField] public Transform[] Points { get; private set; }
}
