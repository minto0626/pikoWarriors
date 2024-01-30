using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// オブジェクトのレイヤー（文字列をそのままUnityのlayerとして扱う）
    /// </summary>
    public enum LayerType
    {
        Player = 0,
        Enemy,
        P_Bullet,
        E_Bullet,
        Weapon,
        Wall,

        Length,
    }

    /// <summary>
    /// 使用するオブジェクト
    /// </summary>
    public enum ObjectType
    {
        None = -1,

        Player_Piko,
        Enemy_Mon,
        Player_Bullet,
        Enemy_Bullet,
        Weapon_OverBath,
        Weapon_Gun,

        Num,
    }

    public abstract class CharacterFactory
    {
        public abstract GameCharacter GetCharacter();
        public abstract void ReleseCharacter(GameCharacter gameCharacter);
    }

    public class SingleUnit : CharacterFactory
    {
        readonly GameCharacter origin;
        public SingleUnit(GameCharacter origin)
        {
            this.origin = origin;
        }
        public override GameCharacter GetCharacter() => GameObject.Instantiate(origin).GetComponent<GameCharacter>();
        public override void ReleseCharacter(GameCharacter gameCharacter) => GameObject.Destroy(gameCharacter);
    }

    public class Pool : CharacterFactory
    {
        readonly ObjectPooler<GameCharacter> pooler;
        public Pool(GameCharacter origin, int capacity)
        {
            pooler = new(origin, capacity);
        }
        public override GameCharacter GetCharacter() => pooler.Get();
        public override void ReleseCharacter(GameCharacter gameCharacter)
        {
            // todo: ObjectPoolerにResourceを追加する。
        }
    }

    public class CharacterManager
    {
        readonly List<GameCharacter> manageCharaList;
        readonly Dictionary<int, int> charaLayerDic;

        readonly CollisionManager collisionManager;

        readonly Dictionary<int, CharacterFactory> characterFactoryDic;
        readonly Dictionary<int, LayerType> objectLayerDic = new()
        {
            {(int)ObjectType.Player_Piko, LayerType.Player},
            {(int)ObjectType.Player_Bullet, LayerType.P_Bullet},
            {(int)ObjectType.Enemy_Mon, LayerType.Enemy},
            {(int)ObjectType.Enemy_Bullet, LayerType.E_Bullet},
            {(int)ObjectType.Weapon_OverBath, LayerType.Weapon},
            {(int)ObjectType.Weapon_Gun, LayerType.Weapon},
        };

        public CharacterManager()
        {
            charaLayerDic = new();
            for (var index = 0; index < (int)LayerType.Length; index++)
            {
                charaLayerDic.Add(index, LayerMask.NameToLayer(((LayerType)index).ToString()));
            }

            manageCharaList = new();
            collisionManager = new();
            characterFactoryDic = new();
        }

        public void OnUpdate()
        {
            var remove = manageCharaList.Where(chara => chara.IsDestroy);
            if (remove.Count() > 0)
            {
                var removeList = remove.ToArray();
                foreach (var obj in removeList)
                {
                    manageCharaList.Remove(obj);
                    collisionManager.Remove(obj);

                    // todo: オブジェクト(****)ごとで処分の仕方を変える
                    //characterFactoryDic[****].ReleseCharacter(obj);
                }
            }
            // todo: OnUpdate中に管理数が増減する処理があると、GetEnumeratorでエラーが発生する。
            //       現状インクリメント方式で対応しているが、いずれ削除＆追加は予約制を取る。
            // foreach (var chara in manageCharaList)
            for (var i = 0; i < manageCharaList.Count; i++)
            {
                //chara.OnUpdate();
                manageCharaList[i].OnUpdate();
            }

            collisionManager.OnUpdate();
        }

        /// <summary>
        /// ゲームキャラのファクトリーを追加する
        /// </summary>
        /// <param name="objID">ゲームキャラのID</param>
        /// <param name="factory">ファクトリーのオブジェクト</param>
        public void AddFactoryCharacter(int objID, CharacterFactory factory)
        {
            characterFactoryDic.Add(objID, factory);
        }

        /// <summary>
        /// 指定したリソースを実体化し、管理に回します。
        /// </summary>
        /// <param name="objectType">リソース</param>
        /// <returns>初期化済オブジェクト</returns>
        public GameCharacter CreateChara(ObjectType objectType)
        {
            GameCharacter obj = characterFactoryDic[(int)objectType].GetCharacter();
            var layerIndex = (int)objectLayerDic[(int)objectType];
            obj.Initialize(charaLayerDic[layerIndex]);
            manageCharaList.Add(obj);
            collisionManager.AddList(obj);

            return obj;
        }

        /// <summary>
        /// 実体化済のオブジェクトを管理に回します。
        /// </summary>
        /// <param name="layerType">オブジェクトのレイヤー</param>
        /// <param name="character">オブジェクトの参照</param>
        public void SetChara(LayerType layerType, GameCharacter character)
        {
            character.Initialize(charaLayerDic[(int)layerType]);
            manageCharaList.Add(character);
            collisionManager.AddList(character);
        }
    }
}
