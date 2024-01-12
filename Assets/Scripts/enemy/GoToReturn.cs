using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /// 弾の発射位置
        /// </summary>
        readonly Vector3 SHOT_POINT = new Vector3(0f, -0.3f, 0f);
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

        public override float Radius => 0.5f;

        public override void Initialize(int layer)
        {
            base.Initialize(layer);
            base.Setup();
            movePos.y = base.moveSpeed;
            nowPhase = Phase.Go;
            waitTimer = 0f;
            shotCount = 0;
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
                        movePos.y = 0f;
                        return;
                    }
                }
                break;
                case Phase.Wait:
                {
                    if (waitTimer >= RETURN_TIME)
                    {
                        nowPhase = Phase.Return;
                        movePos.y = -base.moveSpeed;
                        return;
                    }
                    else if (waitTimer >= SHOT_TIME)
                    {
                        if (shotCount == 0)
                        {
                            base.OneShot(SHOT_POINT);
                            shotCount++;
                        }
                    }

                    waitTimer += Time.deltaTime;
                }
                break;
                case Phase.Return:
                {
                    if (base.Disappear())
                    {
                        return;
                    }
                }
                break;

                default:
                return;
            }

            transform.position += movePos;
        }
    }
}
