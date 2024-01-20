using System.Collections;
using System.Collections.Generic;
using Game.System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 武器の基底クラス
    /// </summary>
    public abstract class Weapon : GameCharacter
    {
        public enum TeamAttribute
        {
            Player,
            Enemy,
        }

        TeamAttribute attackAttr;
        public void SetAttackAttribute(TeamAttribute attackAttribute) { attackAttr = attackAttribute; }
        protected ObjectType TeamObjType
        {
            get
            {
                return attackAttr switch
                {
                    TeamAttribute.Player => ObjectType.Player_Bullet,
                    TeamAttribute.Enemy => ObjectType.Enemy_Bullet,
                    _ => ObjectType.None,
                };
            }
        }

        public virtual void Attack() {}
    }
}
