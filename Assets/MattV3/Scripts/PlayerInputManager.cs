using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    //[SerializeField] CinemachineCamera p1Cam;
    //[SerializeField] CinemachineCamera p2Cam;
    //[SerializeField] CinemachineCamera p3Cam;
    //[SerializeField] CinemachineCamera p4Cam;

    private bool wasdJoined = false;
    private bool arrowsJoined = false;
    private bool gamepad1Joined = false;
    private bool gamepad2Joined = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (!wasdJoined && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "Keyboard1",
                pairWithDevice: Keyboard.current);

            if (spawnPoints.Length > 0)
            {
                player.transform.position = spawnPoints[0].position;
            }

            //p1Cam.Priority = 10;

            wasdJoined = true;
        }

        if (!arrowsJoined && Keyboard.current.rightCtrlKey.wasPressedThisFrame)
        {
            var player = PlayerInput.Instantiate(playerPrefab,
                controlScheme: "Keyboard2",
                pairWithDevice: Keyboard.current);

            if (spawnPoints.Length > 1)
            {
                player.transform.position = spawnPoints[1].position;
            }

            //p2Cam.Priority = 8;

            arrowsJoined = true;
        }

        foreach (var gamePad in Gamepad.all)
        {
            if (gamePad.buttonSouth.wasPressedThisFrame && !gamepad1Joined && !gamepad2Joined)
            {
                PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Gamepad",
                    pairWithDevice: gamePad);
            }

            //p3Cam.Priority = 6;

            gamepad1Joined = true;

            if (gamePad.buttonSouth.wasPressedThisFrame && gamepad1Joined && !gamepad2Joined)
            {
                PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Gamepad",
                    pairWithDevice: gamePad);
            }

            //p4Cam.Priority = 4;

            gamepad2Joined = true;
        }

    }
}
