using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    public class ObjectPooler<TResource> where TResource : GameCharacter
    {
        readonly List<TResource> poolObjList;
        readonly TResource poolOrigin;

        public ObjectPooler(TResource origin, int capacity)
        {
            poolOrigin = origin;
            poolObjList = new(capacity);

            for(var i = 0; i < capacity; i++)
            {
                var newObj = Create();
                newObj.gameObject.SetActive(false);
                poolObjList.Add(newObj);
            }
        }

        public TResource Get()
        {
            // 使用中でないものを探して返す
            foreach (var obj in poolObjList)
            {
                if (!obj.IsActive)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            // 全て使用中だったら新しく作って返す
            var newObj = Create();
            newObj.gameObject.SetActive(true);
            poolObjList.Add(newObj);

            return newObj;
        }

        TResource Create()
        {
            var newObj = GameObject.Instantiate(poolOrigin);
            newObj.name = poolOrigin.name + (poolObjList.Count + 1);
            return newObj;
        }
    }
}
