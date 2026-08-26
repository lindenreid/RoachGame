using UnityEngine;

public class SignalRouter : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // timeline signal callback
    public void PlayMusicBackwards ()
    {
        AudioController._Instance.PlayMusicBackwards();
    }
}
