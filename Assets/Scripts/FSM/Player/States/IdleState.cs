using System;
using UnityEngine;
using UnityHFSM;
using FSM;

namespace FSM.Player.States
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(Gameplay.Player.Player player, StateMachine<PlayerState> _playerFSM) : base(player, _playerFSM) { }

        public override void OnLogic()
        {
            base.OnLogic();
        }
    }
}
