using System.Collections.Generic;

namespace com.victorafael.pool
{

    public class PooledObjectList<T> where T : PooledObject
    {
        private PooledObjectData _pooledObjectData;
        private List<T> _list = new List<T>();

        public int Count
        {
            get => _list.Count;
            set
            {
                if (value < _list.Count)
                {
                    for (int i = _list.Count - 1; i >= value; i--)
                    {
                        var item = _list[i];
                        PoolManager.Return(item);
                        _list.RemoveAt(i);
                    }
                }
                else
                {
                    for (int i = _list.Count; i < value; i++)
                    {
                        var item = PoolManager.Take<T>(_pooledObjectData);
                        _list.Add(item);
                    }
                }
            }
        }

        public T this[int index] => _list[index];
        public PooledObjectList(PooledObjectData pooledObjectData)
        {
            _pooledObjectData = pooledObjectData;
            _list = new List<T>(pooledObjectData.poolSize);
        }

        public T Pop()
        {
            if (_list.Count == 0)
                return null;
            var item = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);
            return item;
        }
    }
}