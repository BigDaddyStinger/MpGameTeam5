using UnityEngine;

[CreateAssetMenu(fileName = "GameSO", menuName = "Scriptable Objects/GameSO")]
public class GameSO : ScriptableObject
{

    [SerializeField] PlayerControl _playerControl; 
    
    [SerializeField] int _maxPlayers = 4;
    [SerializeField] int _score;
    [SerializeField] int _winnerScore;

    [SerializeField] float _boostModifier;
    [SerializeField] float _slowModifier;

}
