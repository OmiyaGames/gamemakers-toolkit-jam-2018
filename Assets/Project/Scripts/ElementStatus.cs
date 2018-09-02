using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OmiyaGames;
using OmiyaGames.Global;

namespace Project
{
    public class ElementStatus : IPooledObject
    {
        public event System.Action<ElementStatus> OnEnergyDepletion;
        public event System.Action<ElementStatus> OnMinTemperatureReached;
        public event System.Action<ElementStatus> OnCooldownFromMinTemperature;
        public event System.Action<ElementStatus> OnMaxTemperatureReached;
        public event System.Action<ElementStatus> OnCooldownFromMaxTemperature;

        [Header("Physics")]
        [SerializeField]
        Rigidbody body;

        [Header("Temperature")]
        [SerializeField]
        float maxTemperature = 100f;
        [SerializeField]
        float startingTemperature = 50f;
        [SerializeField]
        Image temperatureBar;

        [Header("Cooldown")]
        [SerializeField]
        float cooldownDuration = 10f;
        [SerializeField]
        float offsetFromLimits = 5f;

        [Header("Energy")]
        [SerializeField]
        float maxEnergy = 100f;
        [SerializeField]
        Image energyBar;
        [SerializeField]
        bool returnToPoolAfterEnergyDepletion = true;

        float currentTemperature;
        float currentEnergy;
        float lastAtExtremeTemperature = -1f;
        System.Action<float> cooldownMonitor = null;

        #region Properties
        public float CurrentTemperature
        {
            get
            {
                return currentTemperature;
            }
            set
            {
                currentTemperature = Mathf.Clamp(value, 0f, maxTemperature);
                if (Mathf.Approximately(currentTemperature, 0f) == true)
                {
                    OnMinTemperatureReached?.Invoke(this);

                    lastAtExtremeTemperature = Time.time;
                    cooldownMonitor = new System.Action<float>(Cooldown);
                    Singleton.Instance.OnUpdate += cooldownMonitor;
                }
                else if (Mathf.Approximately(currentTemperature, maxTemperature) == true)
                {
                    OnMaxTemperatureReached?.Invoke(this);

                    lastAtExtremeTemperature = Time.time;
                    cooldownMonitor = new System.Action<float>(Cooldown);
                    Singleton.Instance.OnUpdate += cooldownMonitor;
                }
                else if (cooldownMonitor != null)
                {
                    Reset(false, false);
                }

                // Update UI
                if (temperatureBar != null)
                {
                    temperatureBar.fillAmount = Mathf.InverseLerp(0f, maxTemperature, currentTemperature);
                }
            }
        }

        public float CurrentEnergy
        {
            get
            {
                return currentEnergy;
            }
            set
            {
                currentEnergy = Mathf.Clamp(value, 0f, maxEnergy);
                if (Mathf.Approximately(currentEnergy, 0f) == true)
                {
                    OnEnergyDepletion?.Invoke(this);
                    if (returnToPoolAfterEnergyDepletion == true)
                    {
                        Reset(false, false);
                        PoolingManager.ReturnToPool(this);
                    }
                }
                else if (energyBar != null)
                {
                    // Update UI
                    energyBar.fillAmount = Mathf.InverseLerp(0f, maxEnergy, currentEnergy);
                }
            }
        }

        public Rigidbody Body
        {
            get
            {
                return body;
            }
        }
        #endregion

        public override void Start()
        {
            base.Start();

            // Reset variables
            Reset();
        }

        public override void AfterDeactivate(PoolingManager manager)
        {
            base.AfterDeactivate(manager);

            Reset();
        }

        public void Reset(bool resetEnergy = true, bool resetTemperature = true)
        {
            // Reset variables
            if (resetEnergy == true)
            {
                CurrentEnergy = maxEnergy;
            }
            if (resetTemperature == true)
            {
                CurrentTemperature = startingTemperature;
            }

            // Reset events
            lastAtExtremeTemperature = -1f;
            if (cooldownMonitor != null)
            {
                Singleton.Instance.OnUpdate -= cooldownMonitor;
                cooldownMonitor = null;
            }
        }

        public void Cooldown(float delta)
        {
            if ((Time.time - lastAtExtremeTemperature) > cooldownDuration)
            {
                lastAtExtremeTemperature = -1f;

                if (Mathf.Approximately(currentTemperature, 0f) == true)
                {
                    OnCooldownFromMinTemperature?.Invoke(this);
                }
                else if (Mathf.Approximately(currentTemperature, maxTemperature) == true)
                {
                    OnCooldownFromMaxTemperature?.Invoke(this);
                }
            }

            if ((lastAtExtremeTemperature < 0f) && (cooldownMonitor != null))
            {
                Singleton.Instance.OnUpdate -= cooldownMonitor;
                cooldownMonitor = null;
            }
        }
    }
}
