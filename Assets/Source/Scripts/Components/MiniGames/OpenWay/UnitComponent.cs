using System.Collections;
using Pathfinding;
using UnityEngine;

public class UnitComponent : MonoBehaviour
{
    [SerializeField] private AIDestinationSetter _AIDestinationSetter;
    [SerializeField] private Transform[] _points;

    private int _stage;

    private void Start()
    {
        _AIDestinationSetter.target = gameObject.transform;
        GameEvents.SetStagePath_E += SetTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bow actionObject))
        {
            actionObject.Action(this);
        }
    }

    public void NextStage()
    {
        GameEvents.CheckStagePath_E?.Invoke(_stage);
    }

    private void SetTarget()
    {
        _stage++;
        _AIDestinationSetter.target = _points[_stage];

        StartCoroutine(CheckTarget());
    }

    private IEnumerator CheckTarget()
    {
        while (Vector2.Distance(gameObject.transform.position, _points[_stage].position) > 0.5f)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log(Vector2.Distance(gameObject.transform.position, _points[_stage].position));
        }

        NextStage();
    }
}
