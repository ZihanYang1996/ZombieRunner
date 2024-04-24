using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    CharacterController characterController;
    FirstPersonController firstPersonController;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponBob();
    }

    private void UpdateWeaponBob()
    {
        float characterMovementFactor = Mathf.Clamp01(characterController.velocity.sqrMagnitude / (firstPersonController.SprintSpeed * firstPersonController.SprintSpeed));

        weaponBobFactor = Mathf.Lerp(weaponBobFactor, characterMovementFactor, Time.deltaTime * BobSharpness);

        float bobAmount = DefaultBobAmount;
        float frequency = BobFrequency;
        float hBobValue = Mathf.Sin(Time.time * frequency) * bobAmount * weaponBobFactor;
        float vBobValue = ((Mathf.Sin(Time.time * frequency * 2) * 0.5f) + 0.5f) * bobAmount * weaponBobFactor;

        weaponBobLocalPosition.x = hBobValue;
        weaponBobLocalPosition.y = Mathf.Abs(vBobValue);

        transform.localPosition = Vector3.Lerp(transform.localPosition, weaponBobLocalPosition, Time.deltaTime * BobSharpness);
    }
}
