using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;

namespace Game
{
    /// <summary>
    /// オーバーフロッシャーを模した武器
    /// </summary>
    public class OverBath : Weapon
    {
        /// <summary>
        /// 弾の発射位置
        /// </summary>
        readonly Vector3 shotPoint = new(0f, 0.3f, 0f);
        /// <summary>
        /// 1ショットの発射回数
        /// </summary>
        readonly float SHOT_LIMIT = 4;
        /// <summary>
        /// 弾の発射間隔
        /// </summary>
        readonly float SHOT_INTERVAL_TIME = (1f / 60f) * 5f;

        /// <summary>
        /// 発射可能フラグ
        /// </summary>
        bool canShot = true;
        // 撃った数のカウント
        int shotCount = 0;
        // 撃ち始めのタイマー
        float shotTimer = 0f;

        public override void Initialize(int layer)
        {
            base.Initialize(layer);
            InitStatus();
        }

        public override void OnUpdate()
        {
            if (canShot) { return; }

            if (shotCount >= SHOT_LIMIT)
            {
                InitStatus();
                return;
            }

            if (shotTimer >= SHOT_INTERVAL_TIME * shotCount)
            {
                Shot();
                shotCount++;
            }

            shotTimer += Time.deltaTime;
        }

        public override void Attack()
        {
            canShot = false;
        }

        void InitStatus()
        {
            canShot = true;
            shotCount = 0;
            shotTimer = 0f;
        }

        void Shot()
        {
            var bullet = GameMaster.Instance.CharacterManager.CreateChara(base.TeamObjType);
            bullet.transform.position = transform.position + shotPoint;
        }
    }
}
