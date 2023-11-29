using System;
using System.Collections;
using Code.Enums;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    public int[] Cost;
    public int MergeLevel;
    public int ResourceLevel;
    public int CouponPercent;
    public bool IsCollected;
    public bool IsCollector;

    [field: SerializeField, ReadOnly] public string KeyID { get; private set; }
    [ReadOnly] public string ResourceName;
    [ReadOnly] public string Key;
    [HideInInspector] public ResourceBlockComponent ResourceBlockComponent;

    [HideInInspector] public Sprite[] ResourceVisual;
    [HideInInspector] public Sprite[] ResourceGlow;

    [HideInInspector] public Sprite CouponVisual;
    [HideInInspector] public Sprite CouponGlow;

    [HideInInspector] public ParticleSystem CouponVFX;
    [HideInInspector] public ParticleSystem LostCouponVFX;
    [HideInInspector] public bool IsSuperMerge;


    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject[] _VFX;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private TextMeshPro _coolDownTimer;
    [SerializeField] private TextMeshPro _resourceLevel;

    private ResourceComponent _mergeResourceComponent;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Camera _mainCamera;

    private Vector2 _mousePosition;
    private Vector2 _offset;

    private bool _isDown;
    private float _countdown;

    private void Awake()
    {
        _countdown = 5;
        _coolDownTimer.text = $"{_countdown}";
        _coolDownTimer.GetComponent<MeshRenderer>().sortingOrder = 3;
        _mainCamera = Camera.main;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _timer.SetActive(false);
        _animator = GetComponent<Animator>();

        _resourceLevel.outlineWidth = 0.25f;
        _resourceLevel.outlineColor = new Color32(35, 27, 94, 255);
    }

    public void SetKey(string key, int level)
    {
        gameObject.transform.DOScale(1.5f, 0.1f);

        Key = key;
        MergeLevel = level;
        KeyID = Key + MergeLevel;
        _resourceLevel.gameObject.SetActive(false);

        if (level == 0)
        {
            _glow.sprite = null;
            _spriteRenderer.sprite = ResourceVisual[level];
        }

        else if (level > 0)
        {
            _resourceLevel.gameObject.SetActive(true);

            if (level == 2)
            {
                _resourceLevel.text = "Max.";
                AudioManager.Instance.PlayAudio(AudioTypes.MergeX3);
                OnboardingTaskType resourceType = (OnboardingTaskType)Enum.Parse(typeof(OnboardingTaskType), ResourceName);
                OnboardingTaskType[] taskType = new OnboardingTaskType[] { OnboardingTaskType.MaxMerge, resourceType };
                GameEvents.CheckTask_E(OnboardingTasks.MergeResources, taskType);
            }

            else
            {
                AudioManager.Instance.PlayAudio(AudioTypes.MergeX2);
                _resourceLevel.text = "x2";
            }
        }

        if (Key == ResourceKeys.TreasureKey)
        {
            _glow.sprite = ResourceGlow[0];
        }

        else if (Key == ResourceKeys.CouponKey)
        {
            _timer.SetActive(true);
            _spriteRenderer.sprite = CouponVisual;
            _glow.sprite = CouponGlow;
            StartCoroutine(CountdownToLost());
        }
    }

    public void OnMouseDown()
    {
        if (MergeLevel < 2 && Key != ResourceKeys.CouponKey && Key != ResourceKeys.TreasureKey)
        {
            IsCollected = true;
            _isDown = true;
        }
    }

    public void OnMouseUp()
    {
        if (_mergeResourceComponent != null)
        {
            MergeLevel = IsSuperMerge ? 2 : MergeLevel += 1;

            ResourceBlockComponent.IsResource = false;
            ResourceBlockComponent.ResourceComponent = null;
            ResourceBlockComponent = _mergeResourceComponent.ResourceBlockComponent;
            ResourceBlockComponent.SetResource(this, ResourceVisual, ResourceGlow, MergeLevel, ResourceName, ResourceLevel, Cost);

            Destroy(_mergeResourceComponent.gameObject);

            KeyID = Key + MergeLevel;

            _VFX[MergeLevel - 1].SetActive(true);
            _spriteRenderer.sprite = ResourceVisual[MergeLevel];
            _glow.sprite = ResourceGlow[MergeLevel];

            _mergeResourceComponent = null;

            OnboardingTaskType[] taskType = new OnboardingTaskType[] { OnboardingTaskType.None };
            GameEvents.SpawnResource_E?.Invoke(KeyID);
            GameEvents.CheckTask_E?.Invoke(OnboardingTasks.MergeResources, taskType);
        }

        else if (Key == ResourceKeys.CouponKey)
        {
            ParticleSystem vfx = Instantiate(CouponVFX);
            vfx.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -3);

            _countdown = 5;
            ResourceBlockComponent.IsResource = false;
            GameEvents.SpawnResource_E?.Invoke(Key);
            GameEvents.CoinsBoost_E?.Invoke(CouponPercent, true);
            Destroy(gameObject);
        }


        IsCollected = false;
        _isDown = false;
        gameObject.transform.position = ResourceBlockComponent.transform.position;
    }

    public void OnMouseDrag()
    {
        if (_isDown)
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = _mousePosition - _offset;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isDown && collision.TryGetComponent(out ResourceComponent resourceComponent) && resourceComponent.KeyID == KeyID)
        {
            _mergeResourceComponent = resourceComponent;
            Debug.Log("resource " + _mergeResourceComponent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isDown && collision.TryGetComponent(out ResourceComponent resourceComponent) && resourceComponent.KeyID == KeyID && resourceComponent == _mergeResourceComponent)
        {
            _mergeResourceComponent = null;
            Debug.Log("resource " + _mergeResourceComponent);
        }
    }

    public void AnimatorOff()
    {
        _animator.enabled = false;
    }

    private IEnumerator CountdownToLost()
    {
        while (_countdown > 0)
        {
            yield return new WaitForFixedUpdate();
            _countdown -= Time.deltaTime;
            _coolDownTimer.text = $"{Mathf.RoundToInt(_countdown)}";
        }

        ParticleSystem vfx = Instantiate(LostCouponVFX);
        vfx.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -3);

        _countdown = 5;
        IsCollected = false;
        ResourceBlockComponent.IsResource = false;
        GameEvents.SpawnResource_E?.Invoke(Key);
        Destroy(gameObject);
    }
}