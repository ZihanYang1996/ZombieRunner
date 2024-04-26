using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initHealth = 100;
    
    private int m_CurrentHealth;
    
    public void Awake()
    {
        m_CurrentHealth = initHealth;
    }

    public void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.RestartGame();
    }
    
    
}
