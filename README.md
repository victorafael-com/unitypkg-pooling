# Random List Item Package

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/your-username/your-repo/blob/main/LICENSE)

A pooling system to optimize performance when needing to spawn and despawn a lot of items (like bullets, coins, etc)

## Installation

To install this project use [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html), following those steps:

1. Open Unity Package on `Window/Package Manager`;
2. Click the + button;
3. Choose `Add package from git URL...`;
4. Add the url `https://github.com/victorafael-com/unitypkg-pooling.git`

## Using the Pooled Object

To use the Pooled Object, you must make a few changes when using the project, such as:

### Creating a Pooled Object Data

The first thing to do is to create a Pooled Object Data by right-clicking a Project Folder and going to Create/Pooled Object Data

Then you must fill some required fields:
field | description
-------|-------------
Prefab | The prefab of the pooled object. Must have a PooledObject script attached (Or a child class)
Pool Size | Initial Pool Size for this object. that ammount of elements will be spawned when the first object is taken.
Increment Size | How many items are added to the pool when there's no more items available on the pool

### Referencing Prefabs

Instead of referencing a prefab, you must reference the Pooled Object Data:

```cpp
// public GameObject missilePrefab;
public PooledObjectData missilePool;

{...}

//Missile extends PooledObject (see below)
public Missile SpawnMissile(){
    // var go = Instantiate<GameObject>(missilePrefab);
    // return go.GetComponent<Missile>();
    return missilePool.Take<Missile>(transform.position, transform.forward);
}
```

### Implementing a Pooled Object

Instead of relying a Start / OnDestroy cycle of the object, please rely on overriding those specific methods:

```cpp
using UnityEngine;
using com.victorafael.pool;
public class Missile : PooledObject
{
    private Rigidbody m_rigidBody;
    public float speed = 5;
    public PooledObjectData explosionPool;

    public override void Setup(PooledObjectData data)
    {
        base.Setup(data);
        m_rigidBody = GetComponent<Rigidbody>();
    }
    public override void OnTake()
    {
        base.OnTake();
        m_rigidBody.velocity = Vector3.forward * speed;
    }
    public override void OnReturn()
    {
        base.OnReturn();
        //reset what is needed here.
    }

    void OnColliderEnter(Collider other)
    {
        //creates a explosion at missile position
        explosionPool.Take<GameObject>(transform.position, Quaternion.identity);
        //returns the missle to the pool. Similar to Destroy(gameObject);
        PoolManager.Return(this);
    }
}
```
