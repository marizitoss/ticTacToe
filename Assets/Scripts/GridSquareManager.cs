using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSquareManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _oText;
    [SerializeField] private TextMeshProUGUI _xText;
    private int _squareId;
    private GridSquareState _currentState = GridSquareState.empty;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager._.GridSquareClicked(_squareId);
    }
    public GridSquareState GetSquareState()
    {
        return _currentState;
    }
    public void SetSquare(GridSquareState _newState)
    {
        if (_newState == GridSquareState.empty)
        {
            _oText.enabled = false;
            _xText.enabled = false;
        }
        else if (_newState == GridSquareState.x)
        {
            _oText.enabled = false;
            _xText.enabled = true;
        }
        else if (_newState == GridSquareState.o)
        {
            _oText.enabled = true;
            _xText.enabled = false;
        }
        _currentState = _newState;
    }
    public void SetSquareId(int id)
    {
        _squareId = id;
    }
}

public enum GridSquareState { empty, x, o }