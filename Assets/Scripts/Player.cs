using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UNIT9
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_MaxHealth = 30f;

        private float m_Health;
        private float Health
        {
            get
            {
                return m_Health;
            }
            set
            {
                m_Health = value;
                UIController.Instance.SetHealthFill(m_Health / m_MaxHealth);
            }
        }

        private int m_Score;
        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
                UIController.Instance.SetScoreText(m_Score.ToString());
            }
        }

        public static Player Instance { get; private set; } // static singleton

        void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            Health = m_MaxHealth;
            Score = 0;
        }

        public void TakeDamage(float amount)
        {
            Health -= Mathf.Min(amount, Health);

            if (Health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}