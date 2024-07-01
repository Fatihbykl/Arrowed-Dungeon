using System;
using Gameplay.Interfaces;
using Gameplay.Managers;
using Gameplay.StatusEffectSystem;
using UnityEngine;

namespace Gameplay
{
    public class Projectile : MonoBehaviour
    {
        public GameObject target;
        public StatusEffectBase statusEffect;
        public bool followTarget;
        public float speed = 15f;
        public float rotateSpeed = 200f;
        public GameObject muzzlePrefab;
        public GameObject hitPrefab;

        [Header("Sound Effects")]
        public SoundClip[] arrowHitSoundEffects;
        public SoundClip[] arrowReleaseSoundEffects;
        
        [Header("PREDICTION")] 
        public float maxDistancePredict = 100;
        public float minDistancePredict = 5;
        public float maxTimePrediction = 5;
        private Vector3 _standardPrediction, _deviatedPrediction;

        [Header("Deviation")]
        public float deviationAmount =  25f;
        public float deviationSpeed = 2f;

        private Rigidbody _rb;
        private Rigidbody _targetRb;
        private Vector3 _position;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            if (target)
            {
                _targetRb = target.GetComponent<Rigidbody>();
            }
            
            AudioManager.Instance.PlayRandomSoundFXClip(arrowReleaseSoundEffects, transform);
            InstantiateMuzzleParticle();
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.forward * speed;

            if (followTarget)
            {
                var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, GetTargetPositionLockedY(1f)));
            
                PredictMovement(leadTimePercentage);
                AddDeviation(leadTimePercentage);
                RotateRocket();
            }
        }
        
        private Vector3 GetTargetPositionLockedY(float y)
        {
            _position = target.transform.position;
            _position.y = y;
            return _position;
        }

        private void OnCollisionEnter(Collision other)
        {
            InstantiateHitParticle(other);
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(10);
                AudioManager.Instance.PlayRandomSoundFXClip(arrowHitSoundEffects, other.gameObject.transform);
            }
            if (statusEffect)
            {
                statusEffect.ApplyStatus(other.gameObject);
            }
            Destroy(gameObject);
        }

        private void InstantiateMuzzleParticle()
        {
            if (muzzlePrefab != null) {
                var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
                muzzleVFX.transform.forward = gameObject.transform.forward;
                var ps = muzzleVFX.GetComponent<ParticleSystem>();
                if (ps != null)
                    Destroy (muzzleVFX, ps.main.duration);
                else {
                    var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy (muzzleVFX, psChild.main.duration);
                }
            }
        }

        private void InstantiateHitParticle(Collision co)
        {
            ContactPoint contact = co.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            
            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }
        }

        private void PredictMovement(float leadTimePercentage) {
            var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);
            _standardPrediction = GetTargetPositionLockedY(1f) + _targetRb.velocity * predictionTime;
        }
        
        private void AddDeviation(float leadTimePercentage) {
            var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);
            
            var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

            _deviatedPrediction = _standardPrediction + predictionOffset;
        }
        
        private void RotateRocket() {
            var heading = _deviatedPrediction - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
        }
    }
}
