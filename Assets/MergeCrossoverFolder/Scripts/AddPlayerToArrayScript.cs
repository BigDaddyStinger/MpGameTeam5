using UnityEngine;
using UnityEngine.Events;

public class AddPlayerToArrayScript : MonoBehaviour
{
    [SerializeField] UnityEvent AddPlayerToArrayEvent;
    private void Awake()
    {
        Debug.Log("Player Spawned (Awake)");
        AddPlayerToArrayEvent?.Invoke();        
    }
    private void Start()
    {
        Debug.Log("Player Spawned (Start)");
        AddPlayerToArrayEvent?.Invoke();
    }
}
