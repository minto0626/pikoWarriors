using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game
{
    public abstract class EnemyBase : GameCharacter, ICircleCollison
    {
        [SerializeField]
        protected float moveSpeed;
        protected Vector3 movePos = Vector3.zero;
        protected GameMaster gm = null;

        public virtual Vector2 Center => transform.position;
        public virtual float Radius => 0;

        /// <summary>
        /// 初期化
        /// </summary>
        protected void Setup()
        {
            gm = GameMaster.Instance;
        }

        /// <summary>
        /// 弾を一発出す
        /// </summary>
        /// <param name="shotPoint">出す座標</param>
        protected void OneShot(Vector3 shotPoint)
        {
            var bullet = ObjectPooler.Instance.GetObject();
            var chara = bullet.GetComponent<GameCharacter>();
            chara.Initialize(GameMaster.Instance.GameCharaLayerDic[(int)CollisionManager.ObjectType.E_Bullet]);
            GameMaster.Instance.managedGameCharacterList.Add(chara);
            CollisionManager.Instance.AddList(bullet);
            bullet.transform.position = transform.localPosition + shotPoint;
        }

        /// <summary>
        /// 画面外なら非表示
        /// </summary>
        protected bool Disappear()
        {
            // +1して画面外で消えるようにする
            if ((gm.GetCameraTopLeft().y + 1f) < transform.position.y)
            {
                base.DestroyCharacter();
                return true;
            }

            return false;
        }
    }
}
