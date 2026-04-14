
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.victorafael.pool
{
    [System.Serializable]
    public class PooledObjectRandomQueueEntry
    {
        public PooledObjectData pooledObjectData;
        public int ammount;
    }

    [CreateAssetMenu(fileName = "PooledObjectRandomQueue", menuName = "Assets/Create/Pooled Objects/Random Queue")]
    public class PooledObjectRandomQueue : ScriptableObject
    {
        public bool autoRefill = true;
        public List<PooledObjectRandomQueueEntry> entries;

        private List<PooledObjectData> queue = new List<PooledObjectData>();
        private bool firstTake = true;

        public void Refill(bool clearQueue = true)
        {
            if (clearQueue)
            {
                queue.Clear();
            }

            foreach (var entry in entries)
            {
                for (int i = 0; i < entry.ammount; i++)
                {
                    queue.Add(entry.pooledObjectData);
                }
            }
        }

        public PooledObject Take()
        {
            return Take<PooledObject>();
        }
        public virtual T Take<T>() where T : PooledObject
        {
            if (firstTake)
            {
                Refill();
                firstTake = false;
            }
            if (queue.Count == 0)
            {
                if (autoRefill)
                {
                    Refill();
                }
                else
                {
                    return null;
                }
            }
            int index = UnityEngine.Random.Range(0, queue.Count);
            PooledObjectData data = queue[index];
            queue.RemoveAt(index);
            return PoolManager.Take<T>(data);
        }
    }
}