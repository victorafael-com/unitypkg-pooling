using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.victorafael.pool
{
	public class PooledObject : MonoBehaviour
	{
		/// <summary>
		/// Event that is triggered when the object is taken from the pool.
		/// </summary>
		public event UnityAction<PooledObject> onTakenFromPool;

		/// <summary>
		/// Event that is triggered when the object is returned to the pool.
		/// </summary>
		public event UnityAction<PooledObject> onReturnToPool;

		/// <summary>
		/// Gets the Polled Object Data related to this object
		/// </summary>
		public PooledObjectData PoolData { get; private set; }

		/// <summary>
		/// Called once to setup the pooled object. Similar to the Awake / Start method.
		/// </summary>
		/// <param name="data">The source pool data</param>
		public virtual void Setup(PooledObjectData data)
		{
			PoolData = data;
		}

		/// <summary>
		/// Called when the object is returned to the pool.
		/// </summary>
		public virtual void OnReturn()
		{
			if (onReturnToPool != null)
				onReturnToPool(this);
		}

		/// <summary>
		/// Called when the object is taken from the pool.
		/// </summary>
		public virtual void OnTake()
		{
			if (onTakenFromPool != null)
				onTakenFromPool(this);
		}
	}
}