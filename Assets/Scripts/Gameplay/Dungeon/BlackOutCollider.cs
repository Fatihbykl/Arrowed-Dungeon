using System;
using UnityEngine;

namespace Gameplay.Dungeon
{
    public enum RoomDirection
    {
        Enter,
        Exit
    }
    public class BlackOutCollider : MonoBehaviour
    {
        public BlackOutManager manager;
        public int roomIndex;
        public RoomDirection direction; 
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            switch (direction)
            {
                case RoomDirection.Exit:
                    manager.LeaveRoom(roomIndex);
                    break;
                case RoomDirection.Enter:
                    manager.EnterRoom(roomIndex);
                    break;
            }
        }
    }
}
