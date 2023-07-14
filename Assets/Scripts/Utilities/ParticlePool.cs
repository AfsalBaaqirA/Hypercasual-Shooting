using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour
{
    private ObjectPool<ParticlePool> _particlePool;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;

    }

    public void SetPool(ObjectPool<ParticlePool> particlePool)
    {
        this._particlePool = particlePool;
    }

    private void OnParticleSystemStopped()
    {
        _particlePool.Release(this);
    }
}
