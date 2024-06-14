using System;
using InventorySystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Loot
{
    public class LootDrop : MonoBehaviour
    {
        private Vector3 _velocity = Vector3.up;
        private Rigidbody _rb;

        private void Start()
        {
            _velocity *= Random.Range(4f, 6f);
            _velocity += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            _rb = GetComponent<Rigidbody>();
            _rb.velocity = _velocity;
        }

        private void Update()
        {
            //_rb.position += _velocity * Time.deltaTime;
        }
    }
}
