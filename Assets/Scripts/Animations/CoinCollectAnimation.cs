using System;
using Cysharp.Threading.Tasks;
using Gameplay.Managers;
using UnityEngine;

namespace Animations
{
    public class CoinCollectAnimation : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float waitTimeBeforeMove;
        
        [SerializeField] private float moveSpeed;
        
        private ParticleSystem particlesSystem;
        private ParticleSystem.Particle[] particles;
        private bool canMoveStart;

        private void Start()
        {
            player = GameManager.Instance.playerObject.transform;
            particlesSystem = GetComponent<ParticleSystem>();
            particles = new ParticleSystem.Particle[100];
            canMoveStart = false;
        }

        private void LateUpdate()
        {
            if (particlesSystem.isPlaying) { WaitForCoinsGrounded(); }
            if (canMoveStart)
            {
                int length = particlesSystem.GetParticles(particles);
                
                for (int i = 0; i < length; i++)
                {
                    var currentParticlePos = particles[i].position;
                    var distance = Vector3.Distance(player.position, particles[i].position);
                    
                    if (distance <= 0.1f)
                    {
                        particles[i].remainingLifetime = -1.0f;
                    }
                    else
                    {
                        particles[i].position = Vector3.MoveTowards(currentParticlePos, player.position, moveSpeed * Time.deltaTime);
                    }
                }
                if (length == 0) { canMoveStart = false; }
                particlesSystem.SetParticles(particles, length);
            }
        }

        private async void WaitForCoinsGrounded()
        {
            await UniTask.WaitForSeconds(waitTimeBeforeMove);
            canMoveStart = true;
        }
    }
}