using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private ParticleSystem landingParticles;

    private void OnEnable()
    {
        player.Landed += PlayLandingParticles;
    }

    private void OnDisable()
    {
        player.Landed -= PlayLandingParticles;
    }

    private void PlayLandingParticles()
    {
        landingParticles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        
        landingParticles.Play();
    }
}