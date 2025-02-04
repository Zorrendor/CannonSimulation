using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CannonBulletsManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private CannonConfigSO config;

    private ObjectPool<Bullet> bulletPool;
    private readonly HashSet<Bullet> bulletsToRelease = new HashSet<Bullet>();
    
    private ObjectPool<ParticleSystem> particlesPool;

    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(() =>
        {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            return bullet.GetComponent<Bullet>();
        }, b => b.gameObject.SetActive(true), 
            b => b.gameObject.SetActive(false));
        
        particlesPool = new ObjectPool<ParticleSystem>(() =>
        {
            var expl = Instantiate(explosionPrefab, transform);
            expl.gameObject.SetActive(false);
            return expl;
        }, e => e.gameObject.SetActive(true), 
            e => e.gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        config.OnFire += OnFire;
    }

    private void OnDisable()
    {
        config.OnFire -= OnFire;
    }
    
    private void OnFire(Vector3 position, Vector3 velocity)
    {
        this.SpawnBullet(position, velocity);
    }

    private void SpawnBullet(Vector3 position, Vector3 velocity)
    {
        var bullet = bulletPool.Get();
        bullet.gameObject.SetActive(true);
        bullet.Init(position, velocity, config.bulletEnergyLossFactor, OnDestroyBullet);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        if (bullet.ShouldSpawnParticles)
        {
            var explosion = particlesPool.Get();
            explosion.transform.position = bullet.transform.position;
            explosion.transform.up = bullet.HitNormal;
            explosion.Play(true);

            this.StartCoroutine(ReturnToPoolExplosionRoutine(explosion));
        }
        bulletsToRelease.Add(bullet);
    }

    private IEnumerator ReturnToPoolExplosionRoutine(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration);
        
        particlesPool.Release(ps);
    }

    private void LateUpdate()
    {
        if (bulletsToRelease.Count == 0) return;
        
        foreach (var bullet in bulletsToRelease)
        {
            bulletPool.Release(bullet);    
        }
        bulletsToRelease.Clear();
    }
}