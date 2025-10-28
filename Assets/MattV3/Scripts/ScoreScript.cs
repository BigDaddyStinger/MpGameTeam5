using UnityEngine;
using TMPro;
using Unity.Collections;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] PlayerMovementV3 _playerScript;
    [SerializeField] GameManager _gameManager;

    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] TextMeshProUGUI rankTMP;




    private void Awake()
    {
        scoreTMP.text = "Current Score";
        rankTMP.text = "Current Position";
    }

    public void FixedUpdate()
    {
        if (_playerScript != null)
        {
            int currentScore = _playerScript.playerScore;
            scoreTMP.text = currentScore.ToString();
        }

        if (_gameManager != null)
        {
            int currentRank = _gameManager.indCurrentRank;
            rankTMP.text = currentRank.ToString();
        }
    }

}
