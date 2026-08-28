using UnityEngine;

public class SignalRouter : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private Renderer[] _apartmentRenderers;

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
        PropEffectsController._Instance.StartApartmentDoorDissolve(_apartmentRenderers);
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void EndSequence ()
    {
        SequenceController._Instance.EndCurrentSequence();
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartFadeIn ()
    {
        PostEffectController._Instance.StartFadeIn();
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void StartMainLightFadeIn ()
    {
        LightingController._Instance.FadeInMainLight();
    }
}
