using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityHFSM;

namespace FSM.Player.States
{
    public abstract class PlayerStateBase : State<PlayerState>
    {
        protected readonly Gameplay.Player.Player _player;
        protected readonly StateMachine<PlayerState> _playerFSM;
        
        protected PlayerStateBase(
            Gameplay.Player.Player player,
            StateMachine<PlayerState> playerFSM,
            Action<State<PlayerState, string>> onEnter = null,
            Action<State<PlayerState, string>> onLogic = null,
            Action<State<PlayerState, string>> onExit = null,
            Func<State<PlayerState, string>, bool> canExit = null,
            bool needsExitTime = false,
            bool isGhostState = false) : base(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
            _player = player;
            _playerFSM = playerFSM;
        }
    }
}