using UnityEngine;

public class DrawMiniGameComponent : MonoBehaviour
{
    [SerializeField] private GameObject[] _figures;
    [SerializeField] private DrawComponent _drawComponent;

    private FigureComponent _figureComponent;

    private int _step;

    private void Awake()
    {
        GameObject figure = Instantiate(_figures[0], gameObject.transform);
        _figureComponent = figure.GetComponent<FigureComponent>();

        SetData();
    }

    private void SetData()
    {
        _drawComponent.SetFiqure(_figureComponent.StartPoint, _figureComponent.Points);
    }

    public void FinishDraw()
    {
        _step++;

        if (_step < _figures.Length)
        {
            Destroy(_figureComponent.gameObject);

            GameObject figure = Instantiate(_figures[_step], gameObject.transform);
            _figureComponent = figure.GetComponent<FigureComponent>();

            SetData();
        }
    }
}
