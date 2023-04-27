using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ThimblesMiniGame : MonoBehaviour
{
    [SerializeField] private Transform _ballTransform;
    [SerializeField] private ThimbleComponent[] _thimbleComponents;
    [SerializeField] private int _numbersOfShuffling;

    private int _shufflingCount;

    private const float _shufflingSpeed = 0.5f;
    private const float _thimbleUpperPosition = 1.23f;

    private void Start()
    {
        var cam = Camera.main;
        cam.orthographic = false;
        StartCoroutine(ThimblesShuffling());
    }

    public void RestartGame()
    {
        _shufflingCount = 0;

        for (int i = 0; i < _thimbleComponents.Length; i++)
        {
            _thimbleComponents[i].transform.DOMoveY(_thimbleUpperPosition, 1f);
        }

        StartCoroutine(ThimblesShuffling());
    }

    private IEnumerator ThimblesShuffling()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < _thimbleComponents.Length; i++)
        {
            _thimbleComponents[i].EnableCollider(false);
            _thimbleComponents[i].transform.DOMoveY(0, 0.5f);
        }

        yield return new WaitForSeconds(1f);
        _ballTransform.parent = _thimbleComponents[1].transform;

        while (_shufflingCount < _numbersOfShuffling)
        {
            yield return new WaitForSeconds(1f);
            _shufflingCount++;

            int firstThimble = Random.Range(0, _thimbleComponents.Length);
            int secondThimble;

            do
            {
                secondThimble = Random.Range(0, _thimbleComponents.Length);

            } while (secondThimble == firstThimble);

            

            Vector3 firstThimblePosition = _thimbleComponents[firstThimble].transform.position;

            _thimbleComponents[firstThimble].transform.DOMoveX(_thimbleComponents[secondThimble].transform.position.x, _shufflingSpeed);
            _thimbleComponents[secondThimble].transform.DOMoveX(firstThimblePosition.x, _shufflingSpeed);
        }

        for (int i = 0; i < _thimbleComponents.Length; i++)
        {
            _thimbleComponents[i].EnableCollider(true);
        }

        yield return new WaitForSeconds(_shufflingSpeed);
        _ballTransform.parent = gameObject.transform;

    }
}
