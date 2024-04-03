using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VFX.AbilityIndicatorScripts;
using Random = UnityEngine.Random;

namespace Scenes
{
    public class RangedAOEtest : MonoBehaviour
    {
        [SerializeField] private GameObject throwableObject;
        [SerializeField] private Transform StartPoint;
        [SerializeField] private Transform EndPoint;
        [SerializeField] private ParticleSystem.MinMaxCurve Curve;
        [SerializeField] [Range(0, 100)] private float LerpSpeed = 1f;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject indicator;
        private IndicatorFill _fill;

        private void Start()
        {
            indicator = Instantiate(indicator);
            indicator.transform.position = EndPoint.transform.position;
            _fill = indicator.GetComponent<IndicatorFill>();
            SlerpRectFixedTime();
        }
        
        
        private async void SlerpRectFixedTime()
        {
            throwableObject.transform.rotation = Quaternion.identity;

            float time = 0;
            float random = Random.value;
            float startTime = Time.time;
            while (time < 1)
            {
                throwableObject.transform.position = Vector3.Slerp(
                    StartPoint.position,
                    EndPoint.position,
                    Curve.Evaluate(time, random)
                );
                _fill.fillProgress = time;
                time += Time.deltaTime * LerpSpeed;
                //yield return null;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
            float endTime = Time.time;
            Debug.Log(endTime - startTime);
            Instantiate(particle, throwableObject.transform.position, Quaternion.identity).Play();
            Destroy(indicator);
            Destroy(throwableObject);
        }
    }
}