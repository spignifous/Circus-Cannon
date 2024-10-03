using UnityEngine;


[CreateAssetMenu(fileName = "New Game Data", menuName = "Game/Data/Game Data")]
public class GameData : ScriptableObject
{
    #region GAMEPLAY VARIABLES

    private int _score;
    private int _bestScore;

    #endregion

    public void Score(int value) => _score += value;
    public int Score() => _score;
    public int BestScore() => _bestScore;

    public int ResetScore() => _score = 0;
    public int ResetBestScore() => _bestScore = 0;
}

