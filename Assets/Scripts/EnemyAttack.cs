using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private Transform target;
    private PlayerHealth m_PlayerHealth;

    public void Awake()
    {
        m_PlayerHealth = target.GetComponent<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        Debug.Log("Attack Hit Event");
        m_PlayerHealth.TakeDamage(damage);
    }
}