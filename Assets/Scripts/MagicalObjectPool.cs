using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalObjectPool : MonoBehaviour
{   
    private Queue<MagicalObject> pool;
    
    public void Populate(GameObject prefab, int count) {
        pool = new Queue<MagicalObject>();
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(prefab);
            obj.transform.parent = transform;
            var magicalObj = obj.GetComponent<MagicalObject>();
            AddToPool(magicalObj);
            magicalObj.SetOnFadeOutCallback(AddToPool);
        }
    }

    public void AddToPool(MagicalObject obj) {
        pool.Enqueue(obj);
    }

    public MagicalObject GetFromPool() {
        return pool.Dequeue();
    }
}
