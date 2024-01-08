using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;

namespace Game
{
    /// <summary>
    /// ゲームの主体となるゲームオブジェクト。
    /// 基本的にゲームに関与するオブジェクトは、これを継承させる想定。
    /// </summary>
    public abstract class GameCharacter : MonoBehaviour, IObjectCollison
    {
        [SerializeField]
        CollisionManager.ObjectType layerType;
        int layer = -1;
        public int Layer
        {
            get
            {
                if (layer == -1)
                {
                    layer = LayerMask.NameToLayer(layerType.ToString());
                }
                return layer;
            }
        }

        public virtual void OnHit(GameCharacter target) {}

        public bool IsActive => gameObject.activeSelf;

        public bool IsDestroy { get; private set; } = false;
        public void DestroyCharacter()
        {
            IsDestroy = true;
            gameObject.SetActive(false);
        }

        public virtual void OnUpdate() {}
    }
}