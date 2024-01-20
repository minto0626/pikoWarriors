using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game
{
    public class BulletController : GameCharacter, ICircleCollison
    {
        Vector3 moveDir;
        float moveSpeed;

        public Vector2 Center => transform.position;
        public float Radius => 0.25f;
        public override void OnHit(GameCharacter target)
        {
            base.DestroyCharacter();
        }

        public override void Initialize(int layer)
        {
            base.Initialize(layer);
            Setup();
        }

        public override void OnUpdate()
        {
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
        }

        void Setup()
        {
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }

        /// <summary>
        /// 弾の発射方向を設定。正規化されて使用されるので注意
        /// </summary>
        /// <param name="dir">方向</param>
        public void SetMoveDir(Vector3 dir)
        {
            moveDir = dir.normalized;
        }

        /// <summary>
        /// 弾の速度を設定
        /// </summary>
        /// <param name="speed">速度</param>
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }
    }
}
