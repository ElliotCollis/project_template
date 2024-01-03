using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
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

        // New method for bulk loading
        public async Task<List<T>> LoadAssetsAsync<T>(List<string> assetNames) where T : Object
        {
            var loadTasks = new List<AsyncOperationHandle<T>>();
            foreach (var name in assetNames)
            {
                loadTasks.Add(Addressables.LoadAssetAsync<T>(name));
            }

            var loadedAssets = new List<T>();
            foreach (var task in loadTasks)
            {
                await task.Task;
                if (task.Status == AsyncOperationStatus.Succeeded)
                {
                    loadedAssets.Add(task.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load asset: {task.Result.name}");
                    // Handle the error appropriately
                }
            }

            return loadedAssets;
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

        // New method for bulk loading and instantiating GameObjects
        public async Task<List<GameObject>> LoadAndInstantiateAssetsAsync(List<string> assetNames, Transform parent = null)
        {
            var loadTasks = new List<AsyncOperationHandle<GameObject>>();
            foreach (var name in assetNames)
            {
                loadTasks.Add(Addressables.LoadAssetAsync<GameObject>(name));
            }

            var instantiatedObjects = new List<GameObject>();
            foreach (var task in loadTasks)
            {
                await task.Task;
                if (task.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject loadedObject = task.Result;
                    GameObject instantiatedObject = Object.Instantiate(loadedObject, parent);
                    instantiatedObjects.Add(instantiatedObject);
                }
                else
                {
                    Debug.LogError($"Failed to load and instantiate asset: {task.Result.name}");
                    // Handle the error appropriately
                }
            }

            return instantiatedObjects;
        }
    }
}
