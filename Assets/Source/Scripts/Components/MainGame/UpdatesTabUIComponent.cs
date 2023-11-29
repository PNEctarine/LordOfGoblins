using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpdatesTabUIComponent : MonoBehaviour
{
    [field: SerializeField] public UpdateCardUIComponent[] UpdateCardUIComponents { get; private set; }

    private void Start()
    {
        //GameEvents.CheapestImprovements_E += SortUpdates;
        //SortUpdates();
    }

    private void OnEnable()
    {
        //SortUpdates();
    }

    private void SortUpdates()
    {
        int length = UpdateCardUIComponents.Length - 1;

        for (int j = 0; j < length; j++)
        {
            for (int i = 0; i < UpdateCardUIComponents.Length - 1; i++)
            {
                if (UpdateCardUIComponents[i + 1].Cost() < UpdateCardUIComponents[i].Cost())
                {
                    (UpdateCardUIComponents[i + 1], UpdateCardUIComponents[i]) = (UpdateCardUIComponents[i], UpdateCardUIComponents[i + 1]);
                }

                else if (UpdateCardUIComponents[i + 1].Cost() == UpdateCardUIComponents[i].Cost())
                {
                    Dictionary<int, string> str = new Dictionary<int, string>
                    {
                        { i + 1, UpdateCardUIComponents[i + 1].Name.text },
                        { i, UpdateCardUIComponents[i].Name.text }
                    };

                    var sortedDict = str.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    List<int> indexes = new List<int>();

                    foreach (var ind in sortedDict)
                    {
                        indexes.Add(ind.Key);
                    }

                    UpdateCardUIComponent temp = UpdateCardUIComponents[indexes[1]];
                    UpdateCardUIComponents[i] = UpdateCardUIComponents[indexes[0]];

                    UpdateCardUIComponents[i + 1] = temp;
                }
            }
        }

        for (int i = UpdateCardUIComponents.Length - 1; i >= 0; i--)
        {
            UpdateCardUIComponents[i].transform.SetAsFirstSibling();
        }
    }
}


