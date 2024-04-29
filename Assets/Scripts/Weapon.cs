using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private StarterAssetsInputs _input;
    [SerializeField] Camera _camera;

    [SerializeField] int _damage = 10;
    [SerializeField] ParticleSystem _muzzleFlash;

    float rayCastDistance = Mathf.Infinity;
    IObjectPool<GameObject> pool;
    int layerMaske;

    private enum FireMode
    {
        Single,
        Burst,
        Auto
    }

    [SerializeField] float autoFireRate = 0.1f;
    [SerializeField] float burstFireRate = 0.05f;
    [SerializeField] float burstCooldown = 1f;

    private FireMode m_CurrentFireMode = FireMode.Auto;
    private float m_FireTimer = 0f;
    private bool isFiring = false;

    void Awake()
    {
        _input = GetComponentInParent<StarterAssetsInputs>();
        pool = HitVFXPool.Instance.Pool;
        layerMaske = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if (_input.shoot)
        {
            switch (m_CurrentFireMode)
            {
                case FireMode.Auto:
                    if (m_FireTimer < autoFireRate)
                    {
                        m_FireTimer += Time.deltaTime;
                    }
                    else
                    {
                        Shoot();
                        m_FireTimer = 0;
                    }

                    break;
                case FireMode.Single:
                    Shoot();
                    _input.shoot = false;
                    break;
                case FireMode.Burst:
                    if (!isFiring)
                    {
                        StartCoroutine(ShootBurst());
                    }

                    _input.shoot = false;
                    break;
            }
        }
        else
        {
            m_FireTimer = autoFireRate;
        }
    }

    void Shoot()
    {
        _muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, rayCastDistance))
        {
            Debug.Log(hit.transform.name);
            StartCoroutine(OnHit(hit.point, Quaternion.LookRotation(hit.normal), hit.transform));
            hit.transform.GetComponentInParent<EnemyHealth>()?.TakeDamage(_damage);
        }
    }

    IEnumerator OnHit(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject hitVFX = pool.Get();
        hitVFX.transform.position = position;
        hitVFX.transform.rotation = rotation;
        // hitVFX.transform.SetParent(parent);
        yield return new WaitForSeconds(1f);
        pool.Release(hitVFX);
    }
    
    IEnumerator ShootBurst()
    {
        isFiring = true;
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstFireRate);
        }

        yield return new WaitForSeconds(burstCooldown);
        isFiring = false;
    }

    public void OnFireModeSwitch(InputValue value)
    {
        if (value.Get<float>() > 0)
        {
            SwitchToNextFireMode();
        }
        else if (value.Get<float>() < 0)
        {
            SwitchToPreviousFireMode();
        }
    }
    
    private void SwitchToNextFireMode()
    {
        int nextFireMode = ((int)m_CurrentFireMode + 1) % Enum.GetValues(typeof(FireMode)).Length;
        m_CurrentFireMode = (FireMode)nextFireMode;
        Debug.Log("Switched to " + m_CurrentFireMode.ToString() + " fire mode");
    }
    
    private void SwitchToPreviousFireMode()
    {
        // Add the length of the enum to avoid negative values
        int previousFireMode = ((int)m_CurrentFireMode - 1 + Enum.GetValues(typeof(FireMode)).Length) % Enum.GetValues(typeof(FireMode)).Length;
        m_CurrentFireMode = (FireMode)previousFireMode;
        Debug.Log("Switched to " + m_CurrentFireMode.ToString() + " fire mode");
    }
}