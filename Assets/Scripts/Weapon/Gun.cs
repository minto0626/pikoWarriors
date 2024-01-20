using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;

namespace Game
{
    /// <summary>
    /// 単発銃
    /// </summary>
    public class Gun : Weapon
    {
        /// <summary>
        /// 弾の発射位置
        /// </summary>
        readonly Vector3 SHOT_POINT = new(0f, -0.3f, 0f);
        /// <summary>
        /// 弾の発射方向
        /// </summary>
        readonly Vector3 SHOT_DIR = new Vector3(0f, -1f, 0f);
        /// <summary>
        /// 弾の速度
        /// </summary>
        readonly float SHOT_SPEED = 10f;

        public override void Attack() => Shot();

        void Shot()
        {
            var bullet = GameMaster.Instance.CharacterManager.CreateChara(base.TeamObjType).GetComponent<BulletController>();
            bullet.transform.position = transform.position + SHOT_POINT;
            bullet.SetMoveDir(SHOT_DIR);
            bullet.SetMoveSpeed(SHOT_SPEED);
        }
    }
}
