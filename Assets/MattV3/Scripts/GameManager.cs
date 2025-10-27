using System.Collections.Generic;
using NUnit.Framework;
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

    readonly List<PlayerMovementV3> players = new();
    readonly HashSet<PlayerMovementV3> deadPlayers = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
    }

    public void NotifyPlayerDied(PlayerMovementV3 p)
    {
        if (!deadPlayers.Contains(p))
            deadPlayers.Add(p);

        //DisablePlayerControl(p);

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
    /*
    private void DisablePlayerControl(PlayerMovementV3 p)
    {
        var input = p.GetComponent<PlayerInput>();
        if (input) input.DeactivateInput();
        var rb = p.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
    }
    */
    private void RecomputeRanks()
    {
        var sorted = players
            .OrderByDescending(p1 => p1.playerScore)
            .ThenBy(p1 => players.IndexOf(p1))
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            var rankComp = sorted[i].GetComponent<RankerScript>();
            if (rankComp) rankComp.currentRank = i + 1;
        }
    }

    private void ShowResults()
    {
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
        }
    }

    private IEnumerator RestartAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Instance = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
