using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.victorafael.pool{
	public class PoolManager : MonoBehaviour {
		private static PoolManager instance;
		private Dictionary<PooledObjectData, Queue<PooledObject>> dictionary;
		private Dictionary<PooledObjectData, int> elementCount;
		private static Transform root;
		public PooledObjectData[] startupSetup;
        public bool delayedStart;

		public static void SetupPool(PooledObjectData data){
			if (instance == null) {
				instance = new GameObject ("PoolManager").AddComponent<PoolManager> ();
				Setup();
			}

			instance.DoSetupPool (data);
		}

		static void Setup() {
			root = new GameObject("Poolmanager root").transform;
			root.parent = instance.transform;
			root.gameObject.SetActive(false);

			instance.dictionary = new Dictionary<PooledObjectData, Queue<PooledObject>>();
			instance.elementCount = new Dictionary<PooledObjectData, int>();
		}

		private void DoSetupPool(PooledObjectData data){
			if (dictionary.ContainsKey (data) || data.poolSize == 0)
				return;

			var queue = new Queue<PooledObject> ();
			PoolObjects (data, queue, data.poolSize);
			dictionary.Add (data, queue);
			elementCount.Add(data, data.poolSize);
		}



		public static T Take<T>(PooledObjectData data) where T : PooledObject{
			return instance.DoTake<T> (data);
		}
			
		private T DoTake<T> (PooledObjectData data) where T : PooledObject{
			if (!dictionary.ContainsKey (data)) {
				data.Setup ();
			}
			var queue = dictionary [data];

			if (queue.Count == 0) {
				PoolObjects (data, queue, data.incrementSize);
				elementCount[data] += data.incrementSize;
			}
			var obj = queue.Dequeue ();
			obj.OnTake ();
			obj.transform.parent = null;
			return obj as T;
		}


		public static void Return(PooledObject obj){
			instance.DoReturn (obj);
		}

		private void DoReturn(PooledObject obj){
			obj.OnReturn ();
			obj.transform.parent = root;
			dictionary[obj.PoolData].Enqueue(obj);
		}

		private void PoolObjects(PooledObjectData data, Queue<PooledObject> list, int ammount){
			PooledObject po;
			for (int i = 0; i < ammount; i++) {
				po = Instantiate<GameObject> (data.prefab, root).GetComponent<PooledObject>();
				po.Setup (data);
				list.Enqueue (po);
			}
		}

		private void Awake() {
			if (instance == null) {
				instance = this;
				Setup();
			}
		}

		void Start(){
            if (!delayedStart)
            {
                DoStartSetup();
            }
            else
            {
                Invoke("DoStartSetup", 0.1f);
            }
		}

        void DoStartSetup()
        {
            for (int i = 0; i < startupSetup.Length; i++)
                startupSetup[i].Setup();
        }

		[ContextMenu("Output Element Count")]
		public void OutputElementCount(){
			foreach(var kvp in elementCount){
				Debug.LogFormat("{0}: {1}. {2} in use", kvp.Key.name, kvp.Value, kvp.Value - dictionary[kvp.Key].Count);
			}
		}

		// Update is called once per frame
		void OnDestroy () {
			instance = null;
		}
	}
}