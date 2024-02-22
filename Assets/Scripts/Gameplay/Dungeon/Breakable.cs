using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using UnityEngine;

namespace Gameplay.Dungeon
{
    public class Breakable : MonoBehaviour, IDamageable
    {
        [SerializeField] private int health;
        [SerializeField] private GameObject replacement;
        [SerializeField] private float shakeMultiplier = 6;
        [SerializeField] private bool broken = false;

        public void TakeDamage(int damage, Vector3 direction)
        {
            if (broken) { return; }

            health -= damage;
            if (health <= 0)
            {
                broken = true;
                var replacementObject = Instantiate(replacement, transform.position, transform.rotation);
                var rbs = replacementObject.GetComponentsInChildren<Rigidbody>();
                foreach (var rb in rbs)
                {
                    AnimateDestroy(rb.gameObject);
                }
                Destroy(gameObject);
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
