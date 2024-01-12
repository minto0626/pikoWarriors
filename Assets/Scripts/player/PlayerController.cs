using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game
{
    public class PlayerController : GameCharacter, ICircleCollison
    {
        /// <summary>
        /// 弾の発射位置
        /// </summary>
        /// <returns></returns>
        Vector3 shotPoint = new Vector3(0f, 0.3f, 0f);
        /// <summary>
        /// プレイヤーの移動速度
        /// </summary>
        float moveSpeed = 0.05f;

        /// <summary>
        /// 発射可能フラグ
        /// </summary>
        bool isShot = false;
        // 撃った数のカウント
        int shotCount = 0;
        // 撃ち始めのタイマー
        float shotTimer = 0f;
        /// <summary>
        /// 1ショットの発射回数
        /// </summary>
        float SHOT_LIMIT = 4;
        /// <summary>
        /// 弾の発射間隔
        /// </summary>
        float SHOT_INTERVAL_TIME = (1f / 60f) * 5f;

        public Vector2 Center => transform.position;
        public float Radius => 1;

        public override void OnUpdate()
        {
            MoveControl();
            BulletShot();
        }

        void MoveControl()
        {
            var move = InputManager.Instance.GetMoveValue();
            gameObject.transform.Translate(move.x * moveSpeed, move.y * moveSpeed, 0f);
        }

        void BulletShot()
        {
            // 弾を発射中でなければ撃てる
            if (!isShot)
            {
                if(InputManager.Instance.GetButtonTrigger(InputManager.ButtonType.Fire))
                {
                    isShot = true;
                }
            }
            else
            {
                OhuroShotUpdate();
            }
        }

        void OneShot()
        {
            var bullet = GameMaster.Instance.CharacterManager.CreateChara(CollisionManager.ObjectType.P_Bullet);
            bullet.transform.position = transform.localPosition + shotPoint;
        }

        /// <summary>
        /// オーバーフロッシャーのようなショット
        /// </summary>
        void OhuroShotUpdate()
        {
            if (!isShot) { return; }

            if (shotCount >= SHOT_LIMIT)
            {
                isShot = false;
                shotCount = 0;
                shotTimer = 0f;
                return;
            }

            if (shotTimer >= SHOT_INTERVAL_TIME * shotCount)
            {
                OneShot();
                shotCount++;
            }

            shotTimer += Time.deltaTime;
        }
    }
}
