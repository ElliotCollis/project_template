using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    public class ObjectPooler<T> where T : Component
    {
        private readonly T prefab;
        private readonly List<T> availableObjects = new List<T>();
        private readonly bool autoExpand;
        private readonly int maxSize;

        public ObjectPooler(T prefab, int initialSize = 10, int maxSize = 20, bool autoExpand = true)
        {
            this.prefab = prefab;
            this.maxSize = maxSize;
            this.autoExpand = autoExpand;

            for (int i = 0; i < initialSize; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActive = false)
        {
            var createdObject = Object.Instantiate(prefab);
            createdObject.gameObject.SetActive(isActive);
            availableObjects.Add(createdObject);
            return createdObject;
        }

        public T Get()
        {
            if (availableObjects.Count > 0)
            {
                var pooledObject = availableObjects[0];
                availableObjects.RemoveAt(0);
                pooledObject.gameObject.SetActive(true);
                return pooledObject;
            }
            else if (autoExpand && availableObjects.Count < maxSize)
            {
                return CreateObject(true);
            }

            return null;
        }

        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            availableObjects.Add(objectToReturn);
        }
    }
}
