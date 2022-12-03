using System.Collections;
using UnityEngine;

namespace UNIT9
{
    public class Enemy : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("How many hits can this enemy take.")]
        [SerializeField] private int m_MaxHealth = 3;
        [Tooltip("How fast this enemy should rotate.")]
        [SerializeField] private float m_RotationSpeed = 30f;
        [Tooltip("How fast this enemy should move towards player.")]
        [SerializeField] private float m_MovementSpeed = 0.8f;
        [Tooltip("How fast should the spiral close.")]
        [SerializeField] private float m_MovementAmplitude = 1f;  // Speed of cos movement
        [Tooltip("Amount of damage to inflict on the player on explosion.")]
        [SerializeField] private float m_ExplosionDamage = 5f;
        [Tooltip("How long should this enemy light up upon taking damage.")]
        [SerializeField] private float m_HitColourDuration = 0.1f;
        private int m_Health;

        [Header("References")]
        [SerializeField] private MeshRenderer m_MeshRenderer;
        [SerializeField] private Material m_HitMaterial;
        [SerializeField] private GameObject m_ExplosionParticle;
        private Material m_DefaultMaterial;


        private Vector3 m_PlayerPos = Vector3.zero;
        private float m_ElapsedTime;
        private WaitForSeconds m_HitColourDurationWFS;

        private bool m_MovesOpposite = false;


        private void Start()
        {
            m_Health = m_MaxHealth;

            m_HitColourDurationWFS = new WaitForSeconds(m_HitColourDuration);

            m_DefaultMaterial = m_MeshRenderer.material;

            if (UnityEngine.Random.value < 0.5f)
            {
                m_MovesOpposite = true;
                m_RotationSpeed = -m_RotationSpeed;
            }
        }

        private void Update()
        {
            Vector3 dir = (m_PlayerPos - transform.position).normalized;
            Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;
            if (m_MovesOpposite)
            {
                left = -left;
            }

            m_ElapsedTime += Time.deltaTime;
            float cosineValue = m_MovementAmplitude* Mathf.Cos(Mathf.PI * m_ElapsedTime);

            transform.position += (dir + left) * m_MovementSpeed * Time.deltaTime * (1 - cosineValue);


            // rotate
            m_MeshRenderer.transform.Rotate(0, m_RotationSpeed * Time.deltaTime, 0);
        }

        public void TakeDamage(int amount)
        {
            m_Health-=amount;

            StartCoroutine(ChangeColour());

            if (m_Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Player.Instance.Score++;
            Instantiate(m_ExplosionParticle, m_MeshRenderer.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        IEnumerator ChangeColour()
        {
            m_MeshRenderer.material = m_HitMaterial;
            yield return m_HitColourDurationWFS;
            m_MeshRenderer.material = m_DefaultMaterial;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Explode();
            }
        }

        private void Explode()
        {
            StartCoroutine(Exploding());
        }

        IEnumerator Exploding()
        {
            float elapsedTime = 0f;
            float shakeDuration = 1.5f;
            float shakeFrequency = 35f;
            float shakeAmplitude = 0.1f;
            Vector3 startScale = m_MeshRenderer.transform.localScale;

            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;

                float sin = shakeAmplitude * Mathf.Sin(Time.time * shakeFrequency) ;

                m_MeshRenderer.transform.localScale = startScale + new Vector3(Mathf.Sin(sin), Mathf.Sin(sin), Mathf.Sin(sin));

                yield return null;

            }
            Player.Instance.TakeDamage(m_ExplosionDamage);
            Instantiate(m_ExplosionParticle, m_MeshRenderer.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
