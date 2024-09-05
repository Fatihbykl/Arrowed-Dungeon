using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Dungeon
{
    [Serializable]
    public struct Room
    {
        public GameObject insideBlackout;
        public GameObject[] outsideBlackouts;
    }
    
    public class BlackOutManager : MonoBehaviour
    {
        public Room[] rooms;
        public int startingRoomIndex;

        private int _currentRoom = -1;

        private void Start()
        {
            EnterRoom(startingRoomIndex);
        }

        public void EnterRoom(int roomIndex)
        {
            if (_currentRoom != -1) { return; }
            
            _currentRoom = roomIndex;
            var room = rooms[roomIndex];
            
            //room.insideBlackout.SetActive(false);
            room.insideBlackout.GetComponent<MeshRenderer>().material.DOColor(new Color(0, 0, 0, 0), 0.5f);
            foreach (var blackout in room.outsideBlackouts)
            {
                blackout.GetComponent<MeshRenderer>().material.DOColor(new Color(0, 0, 0, 1), 0.5f);
            }
        }

        public void LeaveRoom(int roomIndex)
        {
            if (_currentRoom == -1) { return; }

            _currentRoom = -1;
            var room = rooms[roomIndex];
            
            room.insideBlackout.GetComponent<MeshRenderer>().material.DOColor(new Color(0, 0, 0, 1), 0.5f);
            foreach (var blackout in room.outsideBlackouts)
            {
                blackout.GetComponent<MeshRenderer>().material.DOColor(new Color(0, 0, 0, 0), 0.5f);
            }
        }
    }
}
