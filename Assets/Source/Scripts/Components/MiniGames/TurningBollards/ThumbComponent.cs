using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ThumbComponent : MonoBehaviour
{
    [SerializeField] private TextMeshPro _letter;
    [SerializeField] private TextMeshPro[] _thumbLetters;
    [SerializeField] private string[] _thumbChars;

    [SerializeField] private TurningThumbsMiniGame _turningThumbsMiniGame;
    [SerializeField] private int _tapsToWin;
    [SerializeField] private bool _isInteractable;

    private BoxCollider _boxCollider;
    [SerializeField] private int _taps;
    private const int _sidesТumber = 3;
    private bool _isDesiredPosition;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        GameEvents.ThumbColliderSwitch_E += ColliderSwitch;

        if (_isInteractable)
        {
            for (int i = 0; i < _thumbLetters.Length; i++)
            {
                _thumbLetters[i].text = _thumbChars[i];
            }
        }

        else
        {
            _boxCollider.enabled = false;
        }

        CheckWin();
    }

    private void OnMouseDown()
    {
        GameEvents.ThumbColliderSwitch_E?.Invoke(false);
        _taps++;

        if (_taps > _sidesТumber)
        {
            _taps = 0;
        }

        _letter.text = _thumbChars[_taps];
        CheckWin();

        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 90, 0), 1f).OnComplete(() =>
        {
            GameEvents.ThumbColliderSwitch_E?.Invoke(true);
            _boxCollider.enabled = true;
        });
    }

    private void ColliderSwitch(bool enabled)
    {
        if (_isInteractable == false)
        {
            _boxCollider.enabled = enabled;
        }
    }

    private void CheckWin()
    {
        if (_taps == _tapsToWin)
        {
            _turningThumbsMiniGame.CheckWin(1);
            _isDesiredPosition = true;
        }

        else if (_taps != _tapsToWin && _isDesiredPosition)
        {
            _isDesiredPosition = false;
            _turningThumbsMiniGame.CheckWin(-1);
        }
    }
}
