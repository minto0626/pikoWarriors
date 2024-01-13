using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 武器の基底クラス
    /// </summary>
    public abstract class Weapon : GameCharacter
    {
        public virtual void Attack() {}
    }
}
