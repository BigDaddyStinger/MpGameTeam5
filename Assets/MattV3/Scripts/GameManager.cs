using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("Gameplay")]
    [SerializeField] float resultsScreenSeconds = 3f;
    [SerializeField] Transform[] spawnPoints;

    [Header("Results UI")]
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TMPro.TextMeshProUGUI resultsText;

    [SerializeField] public int indCurrentRank = 0;

    readonly List<PlayerMovementV3> players = new();
    readonly HashSet<PlayerMovementV3> deadPlayers = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void OnPlayerJoined(PlayerInput pi)
    {
        var p = pi.GetComponent<PlayerMovementV3>();
        if (p == null) { Debug.LogError("Joined player missing PlayerMovementV3"); return;   }

        players.Add(p);

        int idx = players.Count - 1;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            var sp = spawnPoints[Mathf.Min(idx, spawnPoints.Length - 1)];
            p.transform.SetPositionAndRotation(sp.position, sp.rotation);
        }

        p.gameObject.name = $"Player_{players.Count}";

        RecomputeRanks();
    }

    public void NotifyPlayerDied(PlayerMovementV3 p)
    {
        if (!deadPlayers.Contains(p))
            deadPlayers.Add(p);

        RecomputeRanks();

        if(deadPlayers.Count == players.Count && players.Count > 0)
        {
            ShowResults();
            StartCoroutine(RestartAfterDelay(resultsScreenSeconds));
        }
    }

    public void NotifyScoreChanged()
    {
        RecomputeRanks();
    }

    public void RecomputeRanks()
    {
        if(players.Count == 0) return;

        var sorted = players
            .OrderByDescending(p1 => p1.playerScore)
            .ThenBy(p1 => players.IndexOf(p1))
            .ToList();

        int[] ranks = new int[sorted.Count];
        int currentRank = 1;
        ranks[0] = currentRank;

        for (int i = 1; i < sorted.Count; i++)
        {
            if (sorted[i].playerScore == sorted[i - 1].playerScore)
            {
                ranks[i] = currentRank;
            }
            else
            {
                ranks[i] = currentRank = i + 1;
            }
        }

        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].SetRank(ranks[i]);
        }

        indCurrentRank = ranks[sorted.Count - 1];


        /* Commented out for new compute attempt.
         * 
         * 
        var sorted = players
            .OrderByDescending(p1 => p1.playerScore)
            .ThenBy(p1 => players.IndexOf(p1))
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            var rankComp = sorted[i].GetComponent<ScoreScript>();
            if (rankComp)
            {
                indCurrentRank = i + 1;
            }
            else
            {
                return;
            }
        }
        */
    }

    void ShowResults()
    {
        var sorted = players
            .OrderByDescending(p1 => p1.playerScore)
            .ThenBy(p1 => players.IndexOf (p1))
            .ToList();

        if (resultsPanel)
        {
            resultsPanel.SetActive(true);
        }    

        if (resultsText)
        {

            /* Commented out to avoid crash
             
            System.Text.StringBuilder sb = new();
            sb.AppendLine("FinalResults");
            int rank = 1;
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i > 0 && sorted[i].playerScore != sorted[i - 1].playerScore)
                {
                    rank = i + 1;
                }

                sb.AppendLine($"{rank}. {sorted[i].name} - {sorted[i].playerScore} pts");
            }
            */

            resultsText.text = "Game Over";
        }

        /* Commented to save for new attempt
        var sorted = players
            .OrderByDescending(p1 => p1.playerScore)
            .ThenBy(p1 => players.IndexOf(p1))
            .ToList();

        if (resultsPanel) resultsPanel.SetActive(true);

        if (resultsText)
        {
            System.Text.StringBuilder sb = new();
            sb.AppendLine("FINAL RESULTS");
            for (int i = 0; i < sorted.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {sorted[i].name} - {sorted[i].playerScore} pts");
            }
            resultsText.text = sb.ToString();
        } */
    }

    public int GetRankOf(PlayerMovementV3 player)
    {
        var sorted = players
            .OrderByDescending(p1 => p1.playerScore)
            .ThenBy(p1 => players.IndexOf(p1))
            .ToList();

        int idx = sorted.IndexOf(player);
        if (idx < 0) return -1;

        int rank = 1;
        for (int i = 0; i < sorted.Count; i++)
        {
            if (i > 0 && sorted[i].playerScore != sorted[i - 1].playerScore)
            {
                rank = i + 1;
            }

            if (sorted[i] == player)
            {
                return rank;
            }
        }

        return -1;
    }

    private IEnumerator RestartAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Instance = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
