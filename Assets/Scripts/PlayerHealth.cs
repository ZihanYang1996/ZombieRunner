using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initHealth = 100;
    
    private int m_CurrentHealth;
    private GameManager m_GameManager;
    
    public void Awake()
    {
        m_CurrentHealth = initHealth;
        m_GameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;
        Debug.Log("Player took damage: " + damage);
        Debug.Log("Current Health: " + m_CurrentHealth);
        if (m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        m_GameManager.InvokeGameOverCanvas();
    }
    
    
}
