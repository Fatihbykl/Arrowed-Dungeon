using System;
using Cysharp.Threading.Tasks;
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
        public Material opaqueMaterial;
        public Material transparentMaterial;
        public Material transparentMaterialNoColor;

        private int _currentRoom = -1;
        private readonly Color _blackColor = Color.black;
        private readonly Color _transparentColor = new Color(0, 0, 0, 0);

        private void Start()
        {
            EnterRoom(startingRoomIndex);
        }

        public void EnterRoom(int roomIndex)
        {
            if (_currentRoom != -1) { return; }
            
            _currentRoom = roomIndex;
            var room = rooms[roomIndex];
            
            FadeBlackouts(room.insideBlackout.GetComponent<MeshRenderer>(), _transparentColor);
            foreach (var blackout in room.outsideBlackouts)
            {
                FadeBlackouts(blackout.GetComponent<MeshRenderer>(), _blackColor);
            }
        }

        public void LeaveRoom(int roomIndex)
        {
            if (_currentRoom == -1) { return; }

            _currentRoom = -1;
            var room = rooms[roomIndex];
            
            FadeBlackouts(room.insideBlackout.GetComponent<MeshRenderer>(), _blackColor);
            foreach (var blackout in room.outsideBlackouts)
            {
                FadeBlackouts(blackout.GetComponent<MeshRenderer>(), _transparentColor);
            }
        }

        private async void FadeBlackouts(MeshRenderer meshRenderer, Color color, float duration = 0.5f)
        {
            meshRenderer.material = color == _transparentColor ? transparentMaterial : transparentMaterialNoColor;
            
            await meshRenderer.material.DOColor(color, duration);

            if (color == _blackColor) { meshRenderer.material = opaqueMaterial; }
        }
    }
}
