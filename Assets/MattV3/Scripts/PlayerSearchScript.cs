using System.Collections;
using UnityEngine;

public class PlayerSearchScript : MonoBehaviour
{

    public static PlayerSearchScript Instance;

    [SerializeField] string playerTag = "Player";
    [SerializeField] float refetchPlayersEvery = 2f;

    GameObject[] players = new GameObject[0];
    float refetchTimer = 0f;



    public void PlayerFetcher()
    {
        refetchTimer += Time.deltaTime;

        if (refetchTimer >= refetchPlayersEvery)
        {
            FetchPlayers();
            refetchTimer = 0f;
        }

        if(players == null || players.Length == 0)
        {
            return;
        }
    }

    public void FetchPlayers()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag);
        StartCoroutine(PlayerCheck());
    }

    public IEnumerator PlayerCheck()
    {
        yield return new WaitForSeconds(5);
    }
}
