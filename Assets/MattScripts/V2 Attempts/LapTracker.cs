using UnityEngine;

public class LapTracker : MonoBehaviour
{
    public int totalLaps = 3;
    int nextCheckpointIndex = 0;
    int currentLap = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Checkpoint")) return;

        // very simple: checkpoints must be named CP_0, CP_1, ...
        string n = other.name.Replace("CP_", "");
        if (int.TryParse(n, out int idx) && idx == nextCheckpointIndex)
        {
            nextCheckpointIndex++;
            // lap completed?
            if (other.CompareTag("FinishLine"))
            {
                currentLap++;
                nextCheckpointIndex = 0;
                if (currentLap > totalLaps)
                    Debug.Log("FINISH!");
            }
        }
    }
}
