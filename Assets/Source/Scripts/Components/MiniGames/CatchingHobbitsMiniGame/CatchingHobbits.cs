using System.Collections;
using UnityEngine;

public class CatchingHobbits : MonoBehaviour
{
    [SerializeField] private int _countToWin;

    [SerializeField] private float _gameDuration;

    [SerializeField] private Transform _spawn;
    [SerializeField] private Transform _target;

    [SerializeField] private GameObject _hobbit;
    [SerializeField] private GameObject _fastHobbit;

    private int _count;
    private bool _isEnd;

    private MiniGame _miniGame;

    private void Start()
    {
        GameEvents.CatchHobbit_E += AddScore;

        _miniGame = GetComponent<MiniGame>();

        GameObject hobbit;
        hobbit = Instantiate(_hobbit);

        Vector2 position = new Vector2(Random.Range(-2, 2), _spawn.position.y);
        hobbit.transform.position = position;
        hobbit.GetComponent<Hobbit>().Target = _target;

        StartCoroutine(FinishGame());
        StartCoroutine(Spawn());
    }

    private void AddScore()
    {
        _count++;
    }

    private IEnumerator Spawn()
    {
        while (_isEnd == false)
        {
            yield return new WaitForSeconds(1.5f);

            int chance = Random.Range(0, 100);
            GameObject hobbit;

            if (chance > 20)
            {
                hobbit = Instantiate(_hobbit);
            }

            else
            {
                hobbit = Instantiate(_fastHobbit);
            }

            Vector2 position = new Vector2(Random.Range(-2, 2), _spawn.position.y);
            hobbit.transform.position = position;
            hobbit.GetComponent<Hobbit>().Target = _target;
        }
    }
    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(_gameDuration);
        GameEvents.CatchHobbit_E = null;
        _isEnd = true;

        if (_count >= _countToWin)
        {
            _miniGame.Win();
        }

        else
        {
            _miniGame.Fail();
        }
    }
}
