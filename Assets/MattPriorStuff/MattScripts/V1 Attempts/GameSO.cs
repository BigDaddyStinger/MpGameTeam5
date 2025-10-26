using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GameSO", menuName = "Scriptable Objects/GameSO")]
public class GameSO : ScriptableObject
{

    [Header("Players / Scoring")]
//    [SerializeField] int _maxPlayers = 4;
//    [SerializeField] int _winnerScore = 10;

//    [SerializeField] float _boostModifier = 1f;
//    [SerializeField] float _slowModifier = 1f;


    [Header("Movement")]
    public float maxSpeed = 8f;
    public float accel = 10f;
    public float brakeDecel = 24f;
    public float coastDecel = 3f;
    public float turnRate = 720f;

    [SerializeField] PlayerControl _playerControl; 
}
