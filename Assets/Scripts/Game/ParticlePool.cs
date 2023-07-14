using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour
{
    private ObjectPool<ParticlePool> particlePool;
    private ParticleSystem particlePoolSystem;

    private void Awake()
    {
        particlePoolSystem = GetComponent<ParticleSystem>();
        var main = particlePoolSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void SetPool(ObjectPool<ParticlePool> pool)
    {
        particlePool = pool;
    }

    private void OnParticleSystemStopped()
    {
        particlePool.Release(this);
    }
}
