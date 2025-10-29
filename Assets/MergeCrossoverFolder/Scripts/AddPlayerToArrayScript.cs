using UnityEngine;
using UnityEngine.Events;

public class AddPlayerToArrayScript : MonoBehaviour
{
    [SerializeField] ObstacleManagerCollective obstacleManagerCollective;
    [SerializeField] GameObject playerReference; 
    private void Awake()
    {
        obstacleManagerCollective = FindAnyObjectByType<ObstacleManagerCollective>();
        playerReference = gameObject;
    }
    private void Start()
    {
        Debug.Log("Player Spawned (Start)");
        SendPlayerReference();
    }
    void SendPlayerReference()
    {
        Debug.Log("Send Player Reference to Obstacle Manager Collective");
        obstacleManagerCollective.AddPlayer(playerReference);
    }
}
