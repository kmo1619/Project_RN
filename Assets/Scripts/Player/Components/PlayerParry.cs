using UnityEngine;

public class PlayerParry : MonoBehaviour, IParryReceiver
{
    public bool IsWindowActive  { get; private set; }

    public bool WasParrySuccessful { get; private set; }

    public void SetWindowActive(bool value)
    {
        IsWindowActive = value;
    }

    public void ResetParryResult()
    {
        WasParrySuccessful = false;
    }

    public bool TryParry(int staggerPower)
    {
        if (!IsWindowActive)
            return false;

        WasParrySuccessful = true;
        SetWindowActive(false);
        return true;
    }
}
