using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.System
{
    /// <summary>
    /// ゲーム全体でリソースを管理するクラス
    /// </summary>
    public class ResourceStore : SingletonMonoBehaviour<ResourceStore>
    {
        readonly Dictionary<string, GameObject> objects = new();

        void Start()
        {
            if (CheckInstance())
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public async Task Load(string address)
        {
            if (objects.ContainsKey(address))
            {
                return;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            var obj = await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                return;
            }

            objects.Add(address, obj);
        }

        public void Unload(string address)
        {
            if (!objects.TryGetValue(address, out var obj))
            {
                return;
            }

            objects.Remove(address);
            Addressables.Release(obj);
        }

        public GameObject Get(string address)
        {
            if (!objects.TryGetValue(address, out var obj))
            {
                return null;
            }

            return obj;
        }
    }
}
