using System.Collections.Generic;
using UnityEngine;

public class BottleComponent : MonoBehaviour
{
    [SerializeField] private GameObject _water;
    [field: SerializeField] public List<WaterComponent> Water { get; private set; }
    [field: SerializeField] public int WaterCount { get; private set; }
    public Color LastColor { get; private set; }

    private WaterComponent _waterComponent;

    private void Awake()
    {
        _waterComponent = _water.GetComponent<WaterComponent>();

        if (Water.Count > 0)
        {
            for (int i = 0; i < Water.Count; i++)
            {
                WaterCount += (int)Water[i].transform.localScale.y;
            }

            LastColor = Water[Water.Count - 1].Water.color;
        }

        else
        {
            WaterCount = 0;
        }
    }

    public void OnMouseDown()
    {
        GetComponentInParent<WaterSortMiniGame>().SelectBottle(this);
    }

    public void PourOut(int pourOut)
    {
        WaterCount -= pourOut;

        if (Water.Count > 0 && Water.Count > 1)
        {
            Destroy(Water[Water.Count - 1].gameObject);
            Water.Remove(Water[Water.Count - 1]);

            LastColor = Water[Water.Count - 1].Water.color;
        }

        else
        {
            Destroy(Water[Water.Count - 1].gameObject);
            Water.Remove(Water[Water.Count - 1]);
        }
    }

    public void PourBotlle(int waterCount)
    {
        WaterCount += waterCount;

        if (WaterCount >= 4)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public bool CheckColor(Color color)
    {
        if (WaterCount > 0)
        {
            if (color == LastColor)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            WaterComponent waterComponent = Instantiate(_waterComponent, gameObject.transform);
            waterComponent.transform.localScale = new Vector3(1, 0, 1);
            waterComponent.SetColor(color);
            Water.Add(waterComponent);

            LastColor = waterComponent.Water.color;

            return true;
        }
    }
}
