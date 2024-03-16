using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.victorafael.pool
{
	[CreateAssetMenu(fileName = "Pooled Object", menuName = "Pooled Object Data")]
	public class PooledObjectData : ScriptableObject
	{
		[SerializeField] internal GameObject prefab;
		[SerializeField] internal int poolSize;
		[SerializeField] internal int incrementSize;


		public static PooledObjectData Create(GameObject prefab, int poolSize, int increment)
		{
			var data = PooledObjectData.CreateInstance<PooledObjectData>();

			data.prefab = prefab;
			data.poolSize = poolSize;
			data.incrementSize = increment;

			return data;
		}

		public PooledObjectData[] dependencies;

		public virtual void Setup()
		{
			if (poolSize > 0)
			{
				PoolManager.SetupPool(this);
			}
			if (dependencies != null && dependencies.Length > 0)
			{
				for (int i = 0; i < dependencies.Length; i++)
				{
					dependencies[i].Setup();
				}
			}
		}

		public virtual PooledObject Take()
		{
			return PoolManager.Take<PooledObject>(this);
		}
		public virtual T Take<T>() where T : PooledObject
		{
			return PoolManager.Take<T>(this);
		}
		public virtual T Take<T>(Vector3 position, Vector3 forward) where T : PooledObject
		{
			T t = Take<T>();
			t.transform.position = position;
			t.transform.forward = forward;
			return t;
		}
		public virtual T Take<T>(Transform parent, bool resetTransform = true) where T : PooledObject
		{
			T t = Take<T>();
			t.transform.SetParent(parent);
			if (resetTransform)
			{
				t.transform.localPosition = Vector3.zero;
				t.transform.localRotation = Quaternion.identity;
			}
			return t;
		}
		public virtual T Take<T>(Vector3 position, Quaternion rotation) where T : PooledObject
		{
			T t = Take<T>();
			t.transform.position = position;
			t.transform.rotation = rotation;
			return t;
		}
	}
}