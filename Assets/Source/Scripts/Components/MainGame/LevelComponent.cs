using UnityEngine;

public class LevelComponent : MonoBehaviour
{

    [field: SerializeField] public GameObject GoblinCollector { get; private set; }
    [field: SerializeField] public GoblinWithCartComponent GoblinWithCartComponent { get; private set; }

    [field: SerializeField] public Transform[] CartWaypoints { get; private set; }

    [field: SerializeField] public SellPointsComponent SellPointsComponent { get; private set; }
    [field: SerializeField] public MergePointsComponent MergePointsComponent { get; private set; }
    [field: SerializeField] public ResourceBlockComponent[] BlockComponents { get; private set; }
    [field: SerializeField] public RavenComponent RavenComponent { get; private set; }

    [HideInInspector] public int LocationLevel;
    public Vector3 CoinsTarget;


    public void Init()
    {
        MergePointsComponent.SetLockSkin(LocationLevel);
        GoblinCollector.GetComponent<GoblinMovementComponent>().SetSkin(LocationLevel);
    }
}
