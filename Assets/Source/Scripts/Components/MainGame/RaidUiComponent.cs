using UnityEngine;
using UnityEngine.UI;

public class RaidUiComponent : MonoBehaviour
{
    [SerializeField] private SquadComponent[] _squads;
    [SerializeField] private Button _closeWindow;

    private int _squadToOpen;

    private void Start()
    {
        GameEvents.NewSquad_E += OpenSquad;
        _closeWindow.onClick.AddListener(() => gameObject.SetActive(false));

        _squadToOpen = PlayerPrefs.GetInt("SquadToOpen", 0);

        if (_squadToOpen > 0)
        {
            for (int i = 0; i <= _squadToOpen; i++)
            {
                if (_squads[i].RaidCheck() == false)
                {
                    _squads[i].OpenSquad();
                }
            }
        }

        gameObject.SetActive(false);
    }

    private void OpenSquad()
    {
        _squadToOpen++;
        _squads[_squadToOpen].OpenSquad();

        PlayerPrefs.SetInt("SquadToOpen", _squadToOpen);
    }
}
