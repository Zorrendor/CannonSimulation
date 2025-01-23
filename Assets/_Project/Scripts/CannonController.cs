using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Transform bulletSpawnerPosition;
    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private CannonConfigSO cannonConfig;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        playerInput.Player.Attack.performed += OnAttack;
    }
    
    private void OnDisable()
    {
        playerInput.Player.Attack.performed -= OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        cannonConfig.TriggerFire(bulletSpawnerPosition.position, bulletSpawnerPosition.forward * (cannonConfig.power * cannonConfig.powerMultiplier));
    }
    
    private void Update()
    {
        Vector2 look = playerInput.Player.Look.ReadValue<Vector2>() * (Time.deltaTime * rotationSpeed);

        Vector3 rotation = this.transform.eulerAngles;
        rotation.x = ClampEulerAngle(rotation.x - look.y, -60, 30);
        rotation.y += look.x; 
        
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        Vector3 rot = cameraLookAt.rotation.eulerAngles;
        cameraLookAt.rotation = Quaternion.Euler(10, rot.y, 0);
        
        if (playerInput.Player.AddPower.IsPressed())
        {
            cannonConfig.power.Value = Mathf.Clamp(cannonConfig.power.Value + 50f * Time.deltaTime, 1, 100);
        }

        if (playerInput.Player.DecreasePower.IsPressed())
        {
            cannonConfig.power.Value = Mathf.Clamp(cannonConfig.power.Value - 50f * Time.deltaTime, 1, 100);
        }

        cannonConfig.currentVelocity =
            bulletSpawnerPosition.forward * (cannonConfig.power * cannonConfig.powerMultiplier);
    }
    
    private float ClampEulerAngle(float angle, float min, float max)
    {
        angle = (angle + 180) % 360;
        if (angle < 0)
            angle += 360;
        angle -= 180;
        
        return Mathf.Clamp(angle, min, max);
    }
}
