using UnityEngine;

public class ObstacleManagerCollective : MonoBehaviour
{
    [SerializeField] public ObstacleManagerScript[] managers;
    [SerializeField] public GameObject[] players = new GameObject[4];
    public int playerIndex = 0;

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
}
