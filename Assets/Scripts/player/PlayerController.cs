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
        /// <summary>
        /// 1ショットの発射回数
        /// </summary>
        float SHOT_LIMIT = 4;

        public Vector2 Center => transform.position;
        public float Radius => 1;

        public override void OnHit(GameCharacter target)
        {
            Debug.Log("プレイヤーが何かに当たった");
        }

        private void Update()
        {
            MoveControl();
            BulletShot();
        }

        private void MoveControl()
        {
            var move = InputManager.Instance.GetMoveValue();
            gameObject.transform.Translate(move.x * moveSpeed, move.y * moveSpeed, 0f);
        }

        private void BulletShot()
        {
            if(InputManager.Instance.GetButtonTrigger(InputManager.ButtonType.Fire))
            {
                // 弾を発射していなかったら撃てる
                if (!isShot) 
                {
                    StartCoroutine(OhuroShot());
                }
            }
        }

        private void OneShot()
        {
            var bullet = ObjectPooler.Instance.GetObject();
            // bullet.GetComponent<ObjectCollision>().SetUp(CollisionManager.CollisionType.Circle,
            //                                                 CollisionManager.ObjectType.P_Bullet,
            //                                                 0.25f);
            bullet.transform.position = transform.localPosition + shotPoint;
        }

        /// <summary>
        /// オーバーフロッシャーのようなショット
        /// </summary>
        /// <returns></returns>
        private IEnumerator OhuroShot()
        {
            // 撃った数のカウント
            var shotCount = 0;
            // 撃ち始めのタイマー
            var shotTimer = 0f;
            
            isShot = true;

            while(true)
            {
                if(shotCount == SHOT_LIMIT)
                {
                    isShot = false;
                    yield break;
                }

                // 2fごとに弾を出す
                if(shotTimer >= 0.1f * shotCount)
                {
                    OneShot();
                    shotCount++;
                }

                shotTimer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
