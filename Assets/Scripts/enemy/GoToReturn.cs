﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;

namespace Game
{
    /// <summary>
    /// 行って帰る
    /// </summary>
    public class GoToReturn : EnemyBase
    {
        enum Phase
        {
            Go,
            Wait,
            Return,
        }

        /// <summary>
        /// 移動方向のベース
        /// </summary>
        readonly Vector3 MOVE_BASE_DIR = new(0f, -1f, 0f);
        /// <summary>
        /// 移動速度のベース
        /// </summary>
        readonly float MOVE_BASE_SPEED = 3f;

        /// <summary>
        /// 止まる時の座標
        /// </summary>
        readonly float WAIT_POINT = 0f;
        /// <summary>
        /// 弾を撃つ時間
        /// </summary>
        readonly float SHOT_TIME = 1.5f;
        /// <summary>
        /// 帰る時の時間
        /// </summary>
        readonly float RETURN_TIME = 3f;
        /// <summary>
        /// 止まっている経過時間
        /// </summary>
        float waitTimer = 0f;
        /// <summary>
        /// 弾撃ちカウンタ
        /// </summary>
        int shotCount = 0;
        /// <summary>
        /// 現在のフェーズ
        /// </summary>
        Phase nowPhase;

        Weapon weapon;

        public override float Radius => 0.5f;

        public override void Initialize(int layer)
        {
            base.Initialize(layer);
            moveDir = MOVE_BASE_DIR;
            moveSpeed = MOVE_BASE_SPEED;
            nowPhase = Phase.Go;
            waitTimer = 0f;
            shotCount = 0;
            weapon = GameMaster.Instance.CharacterManager.CreateChara(ObjectType.Weapon_Gun).GetComponent<Weapon>();
            weapon.SetAttackAttribute(Weapon.TeamAttribute.Enemy);
            weapon.transform.SetParent(transform);
        }

        public override void OnUpdate() => PhaseUpdate();

        /// <summary>
        /// フェーズ更新
        /// </summary>
        void PhaseUpdate()
        {
            switch (nowPhase)
            {
                case Phase.Go:
                {
                    if (gameObject.transform.position.y <= WAIT_POINT)
                    {
                        nowPhase = Phase.Wait;
                        moveSpeed = 0f;
                        return;
                    }
                }
                break;
                case Phase.Wait:
                {
                    if (waitTimer >= RETURN_TIME)
                    {
                        nowPhase = Phase.Return;
                        moveSpeed = -MOVE_BASE_SPEED;
                        return;
                    }
                    else if (waitTimer >= SHOT_TIME)
                    {
                        if (shotCount == 0)
                        {
                            weapon.Attack();
                            shotCount++;
                        }
                    }

                    waitTimer += Time.deltaTime;
                }
                break;
                case Phase.Return:
                {
                    if (base.IsDestroy)
                    {
                        return;
                    }
                }
                break;

                default:
                return;
            }

            transform.position += moveDir * (moveSpeed * Time.deltaTime);
        }
    }
}
