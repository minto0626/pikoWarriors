using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game
{
    public class PlayerController : GameCharacter, ICapsuleCollison
    {
        /// <summary>
        /// プレイヤーの移動速度
        /// </summary>
        float moveSpeed = 10f;

        public float Radius => 0.5f;
        Vector3 size = new(0f, 0.5f);
        public Vector2 StartSegment => transform.position + size;
        public Vector2 EndSegment => transform.position - size;

        Weapon weapon;

        public override void Initialize(int layer)
        {
            base.Initialize(layer);
            weapon = GameMaster.Instance.CharacterManager.CreateChara(ObjectType.Weapon_OverBath).GetComponent<Weapon>();
            weapon.SetAttackAttribute(Weapon.TeamAttribute.Player);
            weapon.transform.SetParent(transform);
        }

        public override void OnUpdate()
        {
            MoveControl();
            BulletShot();
        }

        void MoveControl()
        {
            Vector3 move = InputManager.Instance.GetMoveValue();
            transform.position += move * (moveSpeed * Time.deltaTime);
        }

        void BulletShot()
        {
            if (InputManager.Instance.GetButtonTrigger(InputManager.ButtonType.Fire))
            {
                weapon.Attack();
            }
        }
    }
}
