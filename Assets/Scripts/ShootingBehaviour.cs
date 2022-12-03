using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UNIT9
{
    public class ShootingBehaviour : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The delay in seconds between each shot.")]
        [SerializeField] private float m_ShootInterval = 0.5f;

        [Header("References")]
        [SerializeField] private GameObject m_BulletPrefab;
        [SerializeField] private Camera m_MainCamera;
        private WaitForSeconds m_ShootIntervalSeconds;

        private bool m_CanShoot = true;


        private void Start()
        {
            m_ShootIntervalSeconds = new WaitForSeconds(m_ShootInterval);
        }

        private void Update()
        {
            if (m_CanShoot && Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Shoot();
                }
            }

            // DEBUG
#if UNITY_EDITOR
            if (m_CanShoot && Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
#endif
        }


        private void Shoot()
        {
            Instantiate(m_BulletPrefab, m_MainCamera.transform.position, m_MainCamera.transform.rotation);
            StartCoroutine(ShootInterval());
        }

        IEnumerator ShootInterval()
        {
            m_CanShoot = false;
            yield return m_ShootIntervalSeconds;
            m_CanShoot = true;
        }
    }

}
