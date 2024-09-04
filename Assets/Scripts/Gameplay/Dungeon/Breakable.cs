using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Interfaces;
using Gameplay.Loot;
using UnityEngine;

namespace Gameplay.Dungeon
{
    public class Breakable : MonoBehaviour, IDamageable
    {
        [SerializeField] private int health;
        [SerializeField] private float shakeMultiplier = 6;
        [SerializeField] private GameObject particle;
        [SerializeField] private bool broken = false;

        private ParticleSystem _particle;
        private LootSpawner _lootSpawner;
        private MeshRenderer _meshRenderer;
        private BoxCollider _boxCollider;

        private void Start()
        {
            _particle = particle.GetComponent<ParticleSystem>();
            _lootSpawner = GetComponent<LootSpawner>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void TakeDamage(int damage)
        {
            if (broken) { return; }

            _particle.Play();
            health -= damage;
            if (health <= 0)
            {
                broken = true;
                _lootSpawner.SpawnItems();
                _meshRenderer.enabled = false;
                _boxCollider.enabled = false;
                Destroy(gameObject,1f);
            }
            else
            {
                gameObject.transform.DOPunchRotation(Vector3.forward * shakeMultiplier, 1f);
            }
        }

        private async void AnimateDestroy(GameObject cell)
        {
            await UniTask.WaitForSeconds(1f);
            await cell.GetComponent<MeshRenderer>().material.DOFade(0f, 2f);
            Destroy(cell, 1f);
        }
    }
}
