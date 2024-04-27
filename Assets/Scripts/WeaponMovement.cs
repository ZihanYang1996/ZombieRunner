using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    CharacterController characterController;
    FirstPersonController firstPersonController;
    private StarterAssetsInputs _input;
    
    [SerializeField] Vector3 defaultWeaponPosition;
    [SerializeField] Vector3 defaultWeaponRotation;
    [SerializeField] Vector3 aimWeaponPosition;
    [SerializeField] Vector3 aimWeaponRotation;
    [SerializeField] private float aimingAnimationSpeed = 10;
    
    Vector3 m_WeaponPosition;
    Vector3 m_WeaponRotation;
    
    float weaponBobFactor;
    Vector3 weaponBobLocalPosition;
    public float DefaultBobAmount = 0.05f;
    // public float AimingBobAmount = 0.05f;

    [Tooltip("How fast the weapon bob is applied, the bigger value the fastest")]
    public float BobSharpness = 10f;

    [Tooltip("Frequency at which the weapon will move around in the screen when the player is in movement")]
    public float BobFrequency = 10f;

    void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        firstPersonController = GetComponentInParent<FirstPersonController>();
        _input = GetComponentInParent<StarterAssetsInputs>();
        m_WeaponPosition = defaultWeaponPosition;
        m_WeaponRotation = defaultWeaponRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponBob();
        Aim();
        
        transform.localPosition = m_WeaponPosition + weaponBobLocalPosition;
        transform.localRotation = Quaternion.Euler(m_WeaponRotation);
    }

    private void UpdateWeaponBob()
    {
        float characterMovementFactor = Mathf.Clamp01(characterController.velocity.sqrMagnitude / (firstPersonController.SprintSpeed * firstPersonController.SprintSpeed));

        weaponBobFactor = Mathf.Lerp(weaponBobFactor, characterMovementFactor, Time.deltaTime * BobSharpness);
        if (_input.aim)
        {
            weaponBobFactor = 0;
        }
        float bobAmount = DefaultBobAmount;
        float frequency = BobFrequency;
        float hBobValue = Mathf.Sin(Time.time * frequency) * bobAmount * weaponBobFactor;
        float vBobValue = ((Mathf.Sin(Time.time * frequency * 2) * 0.5f) + 0.5f) * bobAmount * weaponBobFactor;

        weaponBobLocalPosition.x = hBobValue;
        weaponBobLocalPosition.y = Mathf.Abs(vBobValue);
    }
    
    void Aim()
    {
        if (_input.aim)
        {
            Debug.Log("Aiming" + m_WeaponPosition);
            m_WeaponPosition = Vector3.Lerp(m_WeaponPosition, aimWeaponPosition, Time.deltaTime * aimingAnimationSpeed);
            m_WeaponRotation = Vector3.Slerp(m_WeaponRotation, aimWeaponRotation, Time.deltaTime * aimingAnimationSpeed);
        }
        else
        {
            m_WeaponPosition = Vector3.Lerp(m_WeaponPosition, defaultWeaponPosition, Time.deltaTime * aimingAnimationSpeed);
            m_WeaponRotation = Vector3.Slerp(m_WeaponRotation, defaultWeaponRotation, Time.deltaTime * aimingAnimationSpeed);
        }
    }
}
