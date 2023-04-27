using UnityEngine;

[RequireComponent(typeof(SentencesManager))]
public class MiniGame : MonoBehaviour
{
    [SerializeField] private GollumDialogComponent _gollumDialogComponent;
    private SentencesManager _sentencesManager;

    private void Awake()
    {
        _sentencesManager = GetComponent<SentencesManager>();
        _gollumDialogComponent.GollumTMP.text = _sentencesManager.Sentences[0];
    }

    public void Fail()
    {
        _gollumDialogComponent.gameObject.SetActive(true);
        _gollumDialogComponent.GollumTMP.text = _sentencesManager.Sentences[1];
    }

    public void Win()
    {
        _gollumDialogComponent.gameObject.SetActive(true);
        _gollumDialogComponent.GollumTMP.text = _sentencesManager.Sentences[2];
    }
}
