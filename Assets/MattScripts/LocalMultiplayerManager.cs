using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if(PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        }
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnPlayerJoined(PlayerInput _input)
    {
        int idx = _input.playerIndex;

        if(spawnPoints != null && spawnPoints.Length > 0)
        {
            _input.transform.position = spawnPoints[idx % spawnPoints.Length].position;
        }

        var cam = _input.GetComponentInChildren<Camera>();

        if(cam != null)
        {
            cam.rect = idx switch
            {
                0 => new Rect(0f, 0.5f, 0.5f, 0.5f),
                1 => new Rect(0.5f, 0.5f, 0.5f, 0.5f),
                2 => new Rect(0f, 0f, 0.5f, 0.5f),
                3 => new Rect(0.5f, 0f, 0.5f, 0.5f),
                _ => new Rect(0f, 0f, 1f, 1f),

            };
        }


    }


}
