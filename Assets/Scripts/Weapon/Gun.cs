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

        public override void Attack() => Shot();

        void Shot()
        {
            var bullet = GameMaster.Instance.CharacterManager.CreateChara(base.TeamObjType);
            bullet.transform.position = transform.position + SHOT_POINT;
        }
    }
}
