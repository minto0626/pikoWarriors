using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game
{
    public class BulletController : GameCharacter, ICircleCollison
    {
        float moveSpeed = 0.1f;
        GameMaster gm = null;

        public Vector2 Center => transform.position;
        public float Radius => 0.25f;
        public override void OnHit(GameCharacter target)
        {
            base.DestroyCharacter();
        }

        void Start()
        {
            Setup();
        }

        public override void OnUpdate()
        {
            gameObject.transform.Translate(0f, moveSpeed, 0f);
            Disappear();
        }

        void Setup()
        {
            gm = GameMaster.Instance;
        }

        /// <summary>
        /// 画面外なら非表示
        /// </summary>
        void Disappear()
        {
            // +1して画面外で消えるようにする
            if((gm.GetCameraTopLeft().y + 1f) < transform.position.y)
            {
                base.DestroyCharacter();
            }
        }
    }
}
