using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudioListenerGate : MonoBehaviour
{
    void Start()
    {
        var al = GetComponentInChildren<AudioListener>(true);
        if (!al) return;
        bool isFirst = PlayerInput.all.Count > 0 && PlayerInput.all[0].gameObject == gameObject;
        al.enabled = isFirst;
    }
}
