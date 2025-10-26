using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class SplitScreenLayout : MonoBehaviour
{
    readonly List<Camera> _cams = new();

    void OnEnable()
    {
        var pim = GetComponent<PlayerInputManager>();
        pim.onPlayerJoined += OnPlayerJoined;
        pim.onPlayerLeft += OnPlayerLeft;
    }

    void OnDisable()
    {
        var pim = GetComponent<PlayerInputManager>();
        pim.onPlayerJoined -= OnPlayerJoined;
        pim.onPlayerLeft -= OnPlayerLeft;
    }

    void OnPlayerJoined(PlayerInput pi)
    {
        // Find the camera inside the spawned player prefab
        var cam = pi.GetComponentInChildren<Camera>(includeInactive: true);
        if (!cam)
        {
            Debug.LogWarning($"Player {pi.playerIndex} has no Camera child.");
            return;
        }
        cam.gameObject.SetActive(true);
        _cams.Add(cam);
        ApplyRects();
    }

    void OnPlayerLeft(PlayerInput pi)
    {
        var cam = pi.GetComponentInChildren<Camera>(includeInactive: true);
        if (cam) _cams.Remove(cam);
        ApplyRects();
    }

    void ApplyRects()
    {
        int n = _cams.Count;
        for (int i = 0; i < n; i++)
        {
            _cams[i].rect = GetViewport(i, n);
        }
    }

    // 1–4 players layouts (classic)
    static Rect GetViewport(int index, int count)
    {
        switch (count)
        {
            case 1: return new Rect(0, 0, 1, 1);
            case 2:
                return index == 0 ? new Rect(0, 0.5f, 1, 0.5f)   // top
                                  : new Rect(0, 0, 1, 0.5f);  // bottom
            case 3:
                if (index == 0) return new Rect(0, 0.5f, 1, 0.5f); // top
                if (index == 1) return new Rect(0, 0, 0.5f, 0.5f); // bottom-left
                return new Rect(0.5f, 0, 0.5f, 0.5f);              // bottom-right
            default: // 4
                return new Rect((index % 2) * 0.5f,
                                (index / 2) * 0.5f,
                                0.5f, 0.5f);
        }
    }
}
