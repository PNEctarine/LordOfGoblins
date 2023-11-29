using System.Collections;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GoblinMovementComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _coinsBoostVFX;
    [SerializeField] private GameObject _goblinBoostVFX;

    [Space(10)]
    [SerializeField] private SkeletonAnimation _front;
    [SerializeField] private SkeletonAnimation _frontBag;
    [SerializeField] private SkeletonAnimation _frontRight;
    [SerializeField] private SkeletonAnimation _frontLeft;

    [SerializeField] private SkeletonAnimation _back;
    [SerializeField] private SkeletonAnimation _backRight;
    [SerializeField] private SkeletonAnimation _backLeft;

    public bool IsGrab { get; private set; }
    public bool IsPath;

    private SkeletonAnimation _animation;
    private SkeletonAnimation _lastAnimation;
    private TrailRenderer _trailRenderer;

    private Vector3 _targetPosition;

    private float _speed;
    private float _lastSpeed;
    private bool _isStop;

    private const string _runName = "Run";
    private const string _idleName = "Idle";

    private void Awake()
    {
        GameEvents.CoinsBoost_E += CoinsBoost;
        GameEvents.CollectorBoost_E += CollectorBoost;

        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.enabled = false;

        _front.gameObject.SetActive(true);
        _front.AnimationName = _idleName;

        _frontBag.gameObject.SetActive(false);
        _frontRight.gameObject.SetActive(false);
        _frontLeft.gameObject.SetActive(false);
        _back.gameObject.SetActive(false);
        _backLeft.gameObject.SetActive(false);
        _backRight.gameObject.SetActive(false);

        _coinsBoostVFX.SetActive(false);
        _goblinBoostVFX.SetActive(false);

        _lastSpeed = _speed;

        _animation = _frontBag;
        _lastAnimation = _animation;
    }

    public void SetSkin(int skin)
    {
        _front.initialSkinName = skin.ToString();
        _frontBag.initialSkinName = skin.ToString();
        _frontRight.initialSkinName = skin.ToString();
        _frontLeft.initialSkinName = skin.ToString();

        _back.initialSkinName = skin.ToString();
        _backRight.initialSkinName = skin.ToString();
        _backLeft.initialSkinName = skin.ToString();

        _front.Initialize(true);
        _frontBag.Initialize(true);
        _frontRight.Initialize(true);
        _frontLeft.Initialize(true);

        _back.Initialize(true);
        _backRight.Initialize(true);
        _backLeft.Initialize(true);
    }

    public void MoveCollector(Vector3 targetPosition, float speed)
    {
        _speed = speed;
        _targetPosition = targetPosition;

        _front.gameObject.SetActive(false);
        _frontBag.gameObject.SetActive(false);
        _frontRight.gameObject.SetActive(false);
        _frontLeft.gameObject.SetActive(false);
        _back.gameObject.SetActive(false);
        _backLeft.gameObject.SetActive(false);
        _backRight.gameObject.SetActive(false);

        if (_targetPosition.y > 0)
        {
            if (gameObject.transform.position.x - _targetPosition.x < -1f)
            {
                _animation = _backRight;
                _backRight.gameObject.SetActive(true);
            }

            else if (gameObject.transform.position.x - _targetPosition.x > 1f)
            {
                _animation = _backLeft;
                _backLeft.gameObject.SetActive(true);
            }

            else
            {
                _animation = _back;
                _back.gameObject.SetActive(true);
            }
        }

        else
        {
            if (gameObject.transform.position.x - _targetPosition.x < -1f)
            {
                _animation = _frontRight;
                _frontRight.gameObject.SetActive(true);
            }

            else if (gameObject.transform.position.x - _targetPosition.x > 1f)
            {
                _animation = _frontLeft;
                _frontLeft.gameObject.SetActive(true);
            }

            else
            {
                _animation = _frontBag;
                _frontBag.gameObject.SetActive(true);
            }
        }


        float distance = Mathf.Abs(Vector2.Distance(transform.position, _targetPosition));
        _animation.AnimationName = _runName;
        _animation.timeScale = 1;
        _rigidbody.DOKill();
        _rigidbody.DOMove(_targetPosition, _speed).SetEase(Ease.Linear);
    }

    public void GrabSet(bool isGrab)
    {
        IsGrab = isGrab;

        if (IsGrab == false)
        {
            IsPath = false;
        }
    }

    //public void CollectorBoost(int clicks)
    //{
    //    _rigidbody.DOKill();

    //    if (clicks >= 3)
    //    {
    //        _isStop = true;
    //        StartCoroutine(WorkBreak());
    //    }

    //    else if (clicks < 3 && _isStop == false)
    //    {
    //        float boosted = _speed - (_speed * 20 / 100);
    //        _rigidbody.DOMove(_targetPosition, boosted).SetEase(Ease.Linear);
    //    }
    //}

    private void CollectorBoost(float percent)
    {
        if (percent > 0)
        {
            _goblinBoostVFX.SetActive(true);
            _trailRenderer.enabled = true;
        }

        else
        {
            _goblinBoostVFX.SetActive(false);
            _trailRenderer.enabled = false;
        }
    }

    private void CoinsBoost(float percent, bool _)
    {
        if (percent > 0)
        {
            _coinsBoostVFX.SetActive(true);
        }

        else
        {
            _coinsBoostVFX.SetActive(false);
        }
    }

    public void Stop(bool isStop)
    {
        if (isStop)
        {
            IdleAnimation();
            _rigidbody.DOKill();
            _animation.AnimationName = _idleName;

        }

        else
        {
            if (_speed > 0)
                _rigidbody.DOMove(_targetPosition, _speed).SetEase(Ease.Linear);

            _animation.gameObject.SetActive(false);
            _animation = _lastAnimation;
            _animation.gameObject.SetActive(true);
            _animation.AnimationName = _runName;
        }
    }

    private void IdleAnimation()
    {
        _lastAnimation = _animation;

        if (IsGrab)
        {
            _front.gameObject.SetActive(false);
            _frontBag.gameObject.SetActive(true);
            _animation = _frontBag;
        }

        else
        {
            _front.gameObject.SetActive(true);
            _frontBag.gameObject.SetActive(false);
            _animation = _front;
        }

        _frontRight.gameObject.SetActive(false);
        _frontLeft.gameObject.SetActive(false);
        _back.gameObject.SetActive(false);
        _backLeft.gameObject.SetActive(false);
        _backRight.gameObject.SetActive(false);
    }

    private IEnumerator WorkBreak()
    {
        yield return new WaitForSeconds(1f);

        _isStop = false;
        _rigidbody.DOMove(_targetPosition, _speed).SetEase(Ease.Linear);
    }
}