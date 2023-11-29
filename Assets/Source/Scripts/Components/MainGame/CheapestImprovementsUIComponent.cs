using Kuhpik;
using UnityEngine;

public class CheapestImprovementsUIComponent : MonoBehaviour
{
    [SerializeField] private UpdatesTabUIComponent[] UpdateTabs;
    [SerializeField] private Transform _tab;

    private UpdateCardUIComponent[] _updateCardUIComponent;
    private UpdateCardUIComponent[] _cheapestImprovements;
    private Bootstrap _bootstrap;

    public void SetUp()
    {
        GameEvents.CheapestImprovements_E += GenerateUpdates;

        _updateCardUIComponent = new UpdateCardUIComponent[UpdateTabs.Length];
        _cheapestImprovements = new UpdateCardUIComponent[UpdateTabs.Length];
        _bootstrap = Bootstrap.Instance;

        GenerateUpdates();
    }

    private void GenerateUpdates()
    {
        UpdateCardUIComponent[] cheapestImprovements = new UpdateCardUIComponent[UpdateTabs.Length];

        for (int i = 0; i < cheapestImprovements.Length; i++)
        {
            if (_cheapestImprovements[i] != null)
            {
                GameEvents.BuyUpgrade_E?.Invoke(_cheapestImprovements[i].UpdateType, _updateCardUIComponent[i], false);
                Destroy(_cheapestImprovements[i].gameObject);
            }
        }

        for (int i = 0; i < UpdateTabs.Length; i++)
        {
            long cheapestCost = UpdateTabs[i].UpdateCardUIComponents[0].Cost();
            cheapestImprovements[i] = UpdateTabs[i].UpdateCardUIComponents[0];

            for (int j = 0; j < UpdateTabs[i].UpdateCardUIComponents.Length; j++)
            {
                if (UpdateTabs[i].UpdateCardUIComponents[j].Cost() < cheapestCost && UpdateTabs[i].UpdateCardUIComponents[j].Cost() > 0)
                {
                    cheapestCost = UpdateTabs[i].UpdateCardUIComponents[j].Cost();
                    cheapestImprovements[i] = UpdateTabs[i].UpdateCardUIComponents[j];
                }
            }
        }

        for (int i = 0; i < cheapestImprovements.Length; i++)
        {
            _updateCardUIComponent[i] = cheapestImprovements[i];
            _cheapestImprovements[i] = Instantiate(cheapestImprovements[i], _tab);
            _cheapestImprovements[i].SetUp();
            _cheapestImprovements[i].CheckCost(_bootstrap.PlayerData.Gold[_bootstrap.PlayerData.LocationLevel]);
        }

        if (_cheapestImprovements.Length > 0)
        {
            if (_cheapestImprovements[0].Cost() >= _cheapestImprovements[1].Cost())
            {
                GameEvents.CheapestUpdateCost_E?.Invoke(_cheapestImprovements[1].Cost());
            }

            else
            {
                GameEvents.CheapestUpdateCost_E(_cheapestImprovements[0].Cost());
            }
        }
    }
}
