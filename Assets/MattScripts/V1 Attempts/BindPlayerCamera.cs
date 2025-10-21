using UnityEngine;
using UnityEngine.InputSystem;

public class BindPlayerCamera : MonoBehaviour
{
    void Awake()
    {
        var pi = GetComponent<PlayerInput>();
        if (pi.camera == null)
        {
            var cam = GetComponentInChildren<Camera>(true);
            if (cam != null) pi.camera = cam;
            else Debug.LogError("No Camera found under this player. Add a Camera + CinemachineBrain child.");
        }
    }
}
