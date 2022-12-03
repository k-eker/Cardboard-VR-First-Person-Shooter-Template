using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UNIT9
{
    public class UIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image m_HealthFillImage;
        [SerializeField] private Text m_ScoreText;

        public static UIController Instance { get; private set; } // static singleton

        void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }

        public void SetHealthFill(float value)
        {
            m_HealthFillImage.fillAmount = value;
        }

        public void SetScoreText(string value)
        {
            m_ScoreText.text = "Score: " + value;
        }
    }

}
