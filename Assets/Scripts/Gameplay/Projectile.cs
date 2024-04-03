using System;
using UnityEngine;

namespace Gameplay
{
    public class Projectile : MonoBehaviour
    {
        public GameObject target;
        public bool followTarget;
        public float speed = 15f;
        public float rotateSpeed = 200f;
        
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
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _targetRb = target.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.forward * speed;

            if (followTarget)
            {
                var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));
            
                PredictMovement(leadTimePercentage);
                AddDeviation(leadTimePercentage);
                RotateRocket();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }

        private void PredictMovement(float leadTimePercentage) {
            var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);
        
            _standardPrediction = _targetRb.position + _targetRb.velocity * predictionTime;
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
