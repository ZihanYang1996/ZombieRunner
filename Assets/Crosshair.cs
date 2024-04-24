using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using StarterAssets;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] float crosshairSpreadMax = 1;
    [SerializeField] float crosshairSpreadSpeed = 5f;


    RectTransform crosshair;

    void Awake()
    {
        crosshair = GetComponent<RectTransform>();
    }


    void Update()
    {
        CrosshairSpread();
    }

    void CrosshairSpread()
    {
        float crosshairSpreadFactor = Mathf.Clamp01(characterController.velocity.sqrMagnitude / (firstPersonController.SprintSpeed * firstPersonController.SprintSpeed));
        float crosshairSpread = 1 + crosshairSpreadMax * crosshairSpreadFactor;
        crosshair.localScale = Vector3.Lerp(crosshair.localScale, new Vector3(crosshairSpread, crosshairSpread, 1), Time.deltaTime * crosshairSpreadSpeed);
    
    }
}
