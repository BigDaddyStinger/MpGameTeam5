using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinRelay : MonoBehaviour
{
    public PlayerInputManager pim;

    public void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        pim.playerJoinedEvent.AddListener(OnPlayerJoined);
    }

    public void OnDestroy()
    {
        if (pim) pim.playerJoinedEvent.RemoveListener(OnPlayerJoined);
    }

    public void OnPlayerJoined(PlayerInput pi)
    {
        if (GameManager.Instance) GameManager.Instance.OnPlayerJoined(pi);
    }
}
