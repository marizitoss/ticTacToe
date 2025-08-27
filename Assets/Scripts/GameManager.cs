using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _;
    [SerializeField] private GridManager _gridManager;
    private Turn _currentTurn;
    private GridSquareState _playerSquareState;
    private GridSquareState _enemySquareState;
    private bool _awaitingInput = false;
    private GameResult _currentGameState;
    [SerializeField] private TextMeshProUGUI _playerCharacterText;
    [SerializeField] private TextMeshProUGUI _enemyCharacterText;
    [SerializeField] private TextMeshProUGUI _currentPlayerNumberText;
    [SerializeField] private TextMeshProUGUI _currentPlayerCharacterText;
    [SerializeField] private GameObject _gameResultUi;
    [SerializeField] private GameObject _currentTurnUi;
    [SerializeField] private TextMeshProUGUI _gameResultText;

    private void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than one GameManagers in this scene");
        }
        StartNewGame();
    }
    public void RestartGame()
    {
        StartNewGame();
    }
    private void StartNewGame()
    {
        _currentGameState = GameResult.ongoing;
        _gridManager.ResetGrid();

        int firstTurn = Random.Range(0, 2);
        _currentTurn = (Turn)firstTurn;

        if (firstTurn == 0)
        {
            _playerSquareState = GridSquareState.x;
            _enemySquareState = GridSquareState.o;
        }
        else
        {
            _playerSquareState = GridSquareState.o;
            _enemySquareState = GridSquareState.x;
        }

        _playerCharacterText.text = _playerSquareState.ToString();
        _enemyCharacterText.text = _enemySquareState.ToString();

        _gameResultUi.SetActive(false);
        _currentTurnUi.SetActive(true);
        SetCurrentTurnUi();
        _awaitingInput = true;

        if (_currentTurn == Turn.enemyTurn)
        {
            _awaitingInput = false;
            EnemyPlay();
        }
    }

    private IEnumerator EnemyPlayWithDelay()
    {
        yield return new WaitForSeconds(1f);
        EnemyPlay();
    }

    private void ProcessTurn(Turn turn, int selectedSquare)
    {
        _awaitingInput = false;
        GridSquareState state = GridSquareState.empty;
        if (turn == Turn.playerTurn)
        {
            state = _playerSquareState;
            _gridManager.SetSpecificSquare(state, selectedSquare);
            bool gameEnded = CheckIfGameEnded();
            if (!gameEnded)
            {
                ChangeTurn();
                // IA joga com delay
                StartCoroutine(EnemyPlayWithDelay());
            }
            else
            {
                _gameResultUi.SetActive(true);
                _currentTurnUi.SetActive(false);
            }
        }
        else
        {
            _gridManager.SetSpecificSquare(_enemySquareState, selectedSquare);
            bool gameEnded = CheckIfGameEnded();
            if (!gameEnded)
            {
                ChangeTurn();
                _awaitingInput = true;
            }
            else
            {
                _gameResultUi.SetActive(true);
                _currentTurnUi.SetActive(false);
            }
        }
    }
    private bool CheckIfGameEnded()
    {
        bool gridFull = _gridManager.CheckIfGridFull();

        GridSquareState winner = CheckForWin();

        if (winner != GridSquareState.empty)
        {
            if (winner == _playerSquareState)
            {
                _gameResultText.text = _playerSquareState.ToString() + " wins!";
                _currentGameState = GameResult.playerWin;
                return true;
            }
            else
            {
                _gameResultText.text = _enemySquareState.ToString() + " wins!";
                _currentGameState = GameResult.EnemyWin;
                return true;
            }
        }
        else
        {
            if (gridFull)
            {
                _gameResultText.text = "It's a draw!";
                _currentGameState = GameResult.draw;
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    private GridSquareState CheckForWin()
    {
        GridSquareState winner = GridSquareState.empty;

        winner = _gridManager.CheckForWin(0, 1, 2);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(3, 4, 5);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(6, 7, 8);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(0, 3, 6);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(1, 4, 7);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(2, 5, 8);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(0, 4, 8);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        winner = _gridManager.CheckForWin(2, 4, 6);
        if (winner != GridSquareState.empty)
        {
            return winner;
        }

        return winner;
    }
    private void SetCurrentTurnUi()
    {
        if (_currentTurn == Turn.playerTurn)
        {
            _currentPlayerNumberText.text = "Player";
            _currentPlayerCharacterText.text = _playerSquareState.ToString();
        }
        else
        {
            _currentPlayerNumberText.text = "Enemy";
            _currentPlayerCharacterText.text = _enemySquareState.ToString();
        }
    }
    public void ChangeTurn()
    {
        if (_currentTurn == Turn.playerTurn)
        {
            _currentTurn = Turn.enemyTurn;
        }
        else
        {
            _currentTurn = Turn.playerTurn;
        }
        SetCurrentTurnUi();
    }

    private void EnemyPlay()
    {
        int bestMove = GetBestMove();
        ProcessTurn(Turn.enemyTurn, bestMove);
    }

    private int GetBestMove()
    {
        int bestScore = int.MinValue;
        int move = -1;
        for (int i = 0; i < 9; i++)
        {
            if (_gridManager.GetSpecificSquareState(i) == GridSquareState.empty)
            {
                _gridManager.SetSpecificSquare(_enemySquareState, i);
                int score = Minimax(_gridManager, 0, false);
                _gridManager.SetSpecificSquare(GridSquareState.empty, i);
                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }
        return move;
    }

    private int Minimax(GridManager grid, int depth, bool isMaximizing)
    {
        GridSquareState winner = CheckForWin();
        if (winner == _enemySquareState)
            return 10 - depth;
        if (winner == _playerSquareState)
            return depth - 10;
        if (grid.CheckIfGridFull())
            return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (grid.GetSpecificSquareState(i) == GridSquareState.empty)
                {
                    grid.SetSpecificSquare(_enemySquareState, i);
                    int score = Minimax(grid, depth + 1, false);
                    grid.SetSpecificSquare(GridSquareState.empty, i);
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (grid.GetSpecificSquareState(i) == GridSquareState.empty)
                {
                    grid.SetSpecificSquare(_playerSquareState, i);
                    int score = Minimax(grid, depth + 1, true);
                    grid.SetSpecificSquare(GridSquareState.empty, i);
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    public void GridSquareClicked(int clickedSquare)
    {
        if (_awaitingInput == false || _currentTurn != Turn.playerTurn)
        {
            return;
        }
        if (_gridManager.GetSpecificSquareState(clickedSquare) != GridSquareState.empty)
        {
            return;
        }
        ProcessTurn(_currentTurn, clickedSquare);
    }
}

public enum Turn { playerTurn, enemyTurn }
public enum GameResult { ongoing, draw, playerWin, EnemyWin }