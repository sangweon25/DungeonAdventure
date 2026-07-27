using UnityEngine;

public class DustParticle : MonoBehaviour
{
    [SerializeField] private bool _createDustOnWalk = true;
    [SerializeField] private ParticleSystem _dustParticleSystem;

    public void CreateDustParticles()
    {
        if (_createDustOnWalk)
        {
            _dustParticleSystem.Stop();
            _dustParticleSystem.Play();

        }
    }

}
