using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UNIT9
{
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Amount of damage to inflict on the enemy.")]
        [SerializeField] private int m_Damage = 1;
        [Tooltip("How fast should this bullet travel.")]
        [SerializeField] private float m_Speed = 30f;

        [Header("References")]
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private GameObject m_OnHitParticle;

        private void Start()
        {
            // TODO: Pooling system to avoid unnecessary garbage collection
            Destroy(this.gameObject, 3f);
        }
        private void Update()
        {
            // move the bullet forward
            transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.TakeDamage(m_Damage);
                SpawnHitParticle(other.transform.position);
                Destroy(this.gameObject);
            }
        }

        private void SpawnHitParticle(Vector3 targetPos)
        {
            Quaternion swappedRotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
            Instantiate(m_OnHitParticle, transform.position, swappedRotation);
        }
    }

}
