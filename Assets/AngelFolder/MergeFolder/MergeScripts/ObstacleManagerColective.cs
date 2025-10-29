using UnityEngine;

public class ObstacleManagerCollective : MonoBehaviour
{
    [SerializeField] public ObstacleManagerScript[] managers;
    [SerializeField] public GameObject[] players = new GameObject[4];

    private void Awake()
    {
        FindPlayers();
    }

    public void FindPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}
