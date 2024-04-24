using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private StarterAssetsInputs _input;
    [SerializeField] Camera _camera;

    [SerializeField] int _damage = 10;

    void Awake()
    {
        _input = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (_input.shoot)
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit))
            {
                hit.transform.GetComponent<EnemyHealth>()?.TakeDamage(_damage);
            }
            _input.shoot = false;
        }
    }

}
