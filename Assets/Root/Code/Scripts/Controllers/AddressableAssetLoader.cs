using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace HowlingMan
{
    public class AddressableAssetLoader
    {
        public async Task<T> LoadAssetAsync<T>(string assetName) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);

            // Await the completion of the loading operation
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Asset loading status: Succeeded");
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Asset loading status: Failed");
                return null;
            }
        }

        public void UnloadAsset<T>(T asset) where T : Object
        {
            if (asset != null)
            {
                Addressables.Release(asset);
                Debug.Log($"Asset unloaded: {asset.name}");
            }
        }

        public async Task<T> LoadAssetAndInstantiateAsync<T>(string assetName, Transform parent = null) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Asset loading status: Succeeded for {assetName}");

                T result = null;

                if (typeof(T) == typeof(GameObject))
                {
                    GameObject loadedObject = handle.Result as GameObject;
                    result = Object.Instantiate(loadedObject, parent) as T;
                }
                else
                {
                    result = handle.Result;
                }

                // Unload the asset immediately after instantiation  
                Addressables.Release(handle);

                return result;
            }
            else
            {
                Debug.LogError($"Asset loading status: Failed for {assetName}");
                return null;
            }
        }
    }
}
