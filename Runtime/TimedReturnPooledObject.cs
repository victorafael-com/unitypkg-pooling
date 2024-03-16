using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.victorafael.pool
{
	public class TimedReturnPooledObject : PooledObject
	{
		public float returnTime;
		void OnEnable()
		{
			StartCoroutine(Return());
		}
		IEnumerator Return()
		{
			yield return new WaitForSeconds(returnTime);
			PoolManager.Return(this);
		}
	}
}