using UnityEngine;
using TMPro;
using Unity.Collections;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] PlayerMovementV3 _playerScript;
    [SerializeField] RankerScript _indScoreScript;

    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] TextMeshProUGUI rankTMP;




    private void Awake()
    {
        scoreTMP.text = "Current Score";
        rankTMP.text = "Current Position";
    }

    public void FixedUpdate()
    {
        int currentScore = _playerScript.playerScore;
        scoreTMP.text = currentScore.ToString();

        rankTMP.text = _indScoreScript.currentRank.ToString();
    }

}
