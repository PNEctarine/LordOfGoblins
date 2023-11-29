using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WaterSortMiniGame : MonoBehaviour
{
    private int _count;
    private int _pourOutCount;
    private BottleComponent _bottleComponent;

    public void SelectBottle(BottleComponent bottleComponent)
    {
        if (_count == 0 && bottleComponent.WaterCount > 0)
        {
            _count++;
            _bottleComponent = bottleComponent;

            _bottleComponent.transform.DOMoveY(_bottleComponent.transform.position.y + 0.5f, 0.2f);
        }

        else if (_count > 0)
        {
            if (bottleComponent.CheckColor(_bottleComponent.LastColor))
            {
                int firstBottleIndex = _bottleComponent.Water.Count - 1;
                int secondBottleIndex = bottleComponent.Water.Count - 1;

                int scale = Convert.ToInt32(bottleComponent.Water[secondBottleIndex].transform.localScale.y + _bottleComponent.Water[firstBottleIndex].transform.localScale.y);
                int freeSpace = 4 - bottleComponent.WaterCount;
                int remains = (int)_bottleComponent.Water[firstBottleIndex].transform.localScale.y - freeSpace;

                if (_bottleComponent.Water[firstBottleIndex].transform.localScale.y - freeSpace <= 0)
                {
                    _pourOutCount = (int)_bottleComponent.Water[firstBottleIndex].transform.localScale.y;
                    _bottleComponent.Water[firstBottleIndex].transform.DOScaleY(0, 1f);
                    bottleComponent.Water[secondBottleIndex].transform.DOScaleY(scale, 1f);
                }

                else
                {
                    _pourOutCount = (int)_bottleComponent.Water[firstBottleIndex].transform.localScale.y - remains;
                    _bottleComponent.Water[firstBottleIndex].transform.DOScaleY(remains, 1f);
                    bottleComponent.Water[secondBottleIndex].transform.DOScaleY(bottleComponent.Water[secondBottleIndex].transform.localScale.y + freeSpace, 1f);
                }

                bottleComponent.PourBotlle(_pourOutCount);
                _count = 0;

                StartCoroutine(Bottle());
            }

            else
            {
                _bottleComponent.transform.DOMoveY(_bottleComponent.transform.position.y - 0.5f, 0.2f);
                bottleComponent.transform.DOMoveY(bottleComponent.transform.position.y + 0.5f, 0.2f);

                _bottleComponent = bottleComponent;
            }
        }
    }

    private IEnumerator Bottle()
    {
        yield return new WaitForSeconds(1f);
        _bottleComponent.PourOut(_pourOutCount);
        _bottleComponent.transform.DOMoveY(_bottleComponent.transform.position.y - 0.5f, 0.2f);
    }
}
