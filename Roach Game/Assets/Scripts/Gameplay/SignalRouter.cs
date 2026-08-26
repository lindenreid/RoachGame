using UnityEngine;

public class SignalRouter : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private Renderer _apartmentRenderer;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // timeline signal callback
    public void PlayMusicBackwards ()
    {
        AudioController._Instance.PlayMusicBackwards();
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartApartmentDissolve ()
    {
        PropEffectsController._Instance.StartApartmentDoorDissolve(_apartmentRenderer);
    }
}
