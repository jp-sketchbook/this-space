using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isSpawning = false;
    [Range(0f, 1f)]
    public float materialAlpha = 0.8f;
    public List<Material> materials;
    public List<GameObject> prefabs;
    public int prefabCount = 8;
    public float range = 3f;
    public Vector3 origin;
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float minInterval = 0.5f;
    public float maxInterval = 2f;
    private float interval = 1f;

    private List<MagicalObjectPool> pools;

    // Start is called before the first frame update
    void Start()
    {
        pools = new List<MagicalObjectPool>();
        foreach(Material mat in materials) {
            mat.SetColor(
                "_Color",
                new Color(
                    mat.color.r,
                    mat.color.g,
                    mat.color.b,
                    materialAlpha
                )
            );
        }
        foreach(var obj in prefabs)
        {
            var poolObj = new GameObject();
            poolObj.transform.parent = transform;
            poolObj.name = obj.name + "_POOL";
            var pool = poolObj.AddComponent(typeof(MagicalObjectPool)) as MagicalObjectPool;
            pool.Populate(obj, prefabCount);
            pools.Add(pool);
        }
        StartCoroutine(SpawnRoutine());
    }

    void Update() 
    {
    }

    private void PlaceMagicalObject()
    {
        var scaleFactor = Random.Range(minScale, maxScale);
        var randomPosition = new Vector3 (
            origin.x + Random.Range(-range, range),
            origin.y + Random.Range(0, range),
            origin.y + Random.Range(-range, range)
        );
        var randomRotation = new Vector3(
            Random.Range(0, 180),
            Random.Range(0, 180),
            Random.Range(0, 180)
        );
        var obj = pools[Random.Range(0, pools.Count - 1)].GetFromPool() as MagicalObject;
        var material = materials[Random.Range(0, materials.Count - 1)];
        obj.Show(randomPosition, randomRotation, scaleFactor, material);
        Debug.Log("SHOW MAGICAL OBJECT");
        Debug.Log(obj);
        InvokeNext();
    }

    private void InvokeNext() {
        float delay = Random.Range(minInterval, maxInterval);
        Invoke("PlaceMagicalObject", delay);
    }

    private IEnumerator SpawnRoutine() {
        isSpawning = true;
        while(isSpawning) {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            PlaceMagicalObject();
        }
    }
}
