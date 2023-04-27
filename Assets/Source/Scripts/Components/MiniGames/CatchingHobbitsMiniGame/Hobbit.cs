using System;
using UnityEngine;

public class Hobbit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [NonSerialized] public Transform Target;

    private void Update()
    {
        //gameObject.transform.DOMoveY(Target.position.y, _speed);
        gameObject.transform.Translate(_speed * Time.deltaTime * Vector2.up);

        if (gameObject.transform.position.y >= Target.transform.position.y)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnMouseDown()
    {
        GameEvents.CatchHobbit_E?.Invoke();
        gameObject.SetActive(false);
    }
}
