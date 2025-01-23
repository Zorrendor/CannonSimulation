using System;
using UnityEngine;

public class CannonBarrelAnimation : MonoBehaviour
{
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 10.0f;
    public float resetSpeed = 5.0f; 
    
    [SerializeField] private CannonConfigSO config;
    
    private enum AnimationState
    {
        Recoil,
        Reset,
        Stop
    }
    private AnimationState animationState = AnimationState.Stop;
    
    private Vector3 originalPosition;
    private Vector3 recoilPosition;
    private float animationProgress = 0.0f;

    private void Start()
    {
        originalPosition = transform.localPosition;
        recoilPosition = originalPosition - transform.forward * recoilDistance;
    }
    
    private void OnEnable()
    {
        config.OnFire += OnFire;
    }

    private void OnDisable()
    {
        config.OnFire -= OnFire;
    }

    private void Update()
    {
        if (animationState == AnimationState.Stop) return;

        AnimateRecoil();
    }
    
    private void OnFire(Vector3 position, Vector3 velocity)
    {
        this.Fire();
    }

    [ContextMenu("Test Animation")]
    private void Fire()
    {
        if (animationState != AnimationState.Stop) return;

        animationState = AnimationState.Recoil;
        animationProgress = 0.0f;
    }

    private void AnimateRecoil()
    {
        if (animationState == AnimationState.Recoil)
        {
            animationProgress += Time.deltaTime * recoilSpeed;
            transform.localPosition = Vector3.Lerp(originalPosition, recoilPosition, animationProgress);
            if (animationProgress > 1.0f)
            {
                animationState = AnimationState.Reset;
            }
        }
        else if (animationState == AnimationState.Reset)
        {
            animationProgress -= Time.deltaTime * resetSpeed;
            transform.localPosition = Vector3.Lerp(originalPosition, recoilPosition, animationProgress);
            
            if (animationProgress <= 0.0f)
            {
                animationState = AnimationState.Stop;
                transform.localPosition = originalPosition;
            }
        }
    }
}