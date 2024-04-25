using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{
    private StarterAssetsInputs _input;
    [SerializeField] Camera _camera;

    [SerializeField] int _damage = 10;
    [SerializeField] ParticleSystem _muzzleFlash;
    
    float rayCastDistance = Mathf.Infinity;
    IObjectPool<GameObject> pool;
    int layerMaske;

    void Awake()
    {
        _input = GetComponentInParent<StarterAssetsInputs>();
        pool = HitVFXPool.Instance.Pool;
        layerMaske = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (_input.shoot)
        {
            _muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, rayCastDistance))
            {
                Debug.Log(hit.transform.name);
                StartCoroutine(OnHit(hit.point, Quaternion.LookRotation(hit.normal), hit.transform));
                hit.transform.GetComponentInParent<EnemyHealth>()?.TakeDamage(_damage);
            }
            _input.shoot = false;
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


}
