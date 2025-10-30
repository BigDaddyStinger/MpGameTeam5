using NUnit.Framework;
using UnityEngine;
using UnityEngine.Timeline;
using System.Collections.Generic;

public class ObstacleManagerCollective : MonoBehaviour
{
    [SerializeField] public ObstacleManagerScript[] managers;
    [SerializeField] public GameObject[] players = new GameObject[4];
    public int playerIndex = 0;

    [Header("Matt: Spawn Bias Toward Leaders")]
    [SerializeField] private float pointsToWeight = 1f;
    [SerializeField] private float minWeight;

    private void Start()
    {
        playerIndex = 0;
    }
    public void AddPlayer(GameObject newPlayer)
    {
        Debug.Log("Current Player Index: " + playerIndex);

        if (newPlayer == null)
        {
            Debug.Log("New Player Is Null: " + newPlayer);
            return;
        }
        if (playerIndex <= 3)
        {
            players[playerIndex] = newPlayer;
            Debug.Log("Player Has Been Added to Array");
            playerIndex++;
            Debug.Log("New Current PLayer Index: " + playerIndex);
        }
        else
        {
            Debug.Log("Max Players Has Been Reached");
        }
    }



    // Matt Added stuff for Director Ranking to spawn more obstacles

    private static PlayerMovementV3 GetPM(GameObject go) => go ? go.GetComponent<PlayerMovementV3>() : null;

    public Transform PickPlayerByScoreWeight()
    {
        var plist = new List<(GameObject go, PlayerMovementV3 pm)>();
        foreach (var p in players)
        {
            var pm = GetPM(p);
            if (pm != null) plist.Add((p, pm));
        }

        if (plist.Count == 0) return null;

        int minScore = int.MaxValue;

        foreach (var tup in plist) minScore = Mathf.Min(minScore, tup.pm.playerScore);

        float total = 0f;

        var weights = new float [plist.Count];

        for (int i = 0; i < plist.Count; i++)
        {
            int lead = Mathf.Max(0, plist[i].pm.playerScore - minScore);

            float w = minWeight + lead * pointsToWeight;

            weights[i] = w;
            total += w;
        }

        float r = Random.value * total;
        
        for (int i = 0;i < plist.Count;i++)
        {
            if((r -= weights[i]) <= 0)
            {
                return plist[i].go.transform;
            }
        }

        return plist[plist.Count - 1].go.transform;
    }
}
