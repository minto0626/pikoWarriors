using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.System;

namespace Game
{
    public abstract class EnemyBase : GameCharacter, ICircleCollison
    {
        protected float moveSpeed;
        protected Vector3 moveDir = Vector3.zero;

        public virtual Vector2 Center => transform.position;
        public virtual float Radius => 0;
    }
}
