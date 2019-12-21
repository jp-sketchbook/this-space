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

    public List<AudioClip> clips;
    private List<AudioClip> _shuffledClips;
    private int _currentShuffleIndex;
    private List<MagicalObjectPool> _pools;

    // Start is called before the first frame update
    void Start()
    {
        _pools = new List<MagicalObjectPool>();
        _shuffledClips = new List<AudioClip>();
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
            _pools.Add(pool);
        }
        shuffleClips();
    }

    void Update() 
    {
    }

    public void Begin()
    {
        StartCoroutine(SpawnRoutine());
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
        var obj = _pools[Random.Range(0, _pools.Count)].GetFromPool() as MagicalObject;
        var material = materials[Random.Range(0, materials.Count - 1)];
        obj.Show(randomPosition, randomRotation, scaleFactor, material, _shuffledClips[_currentShuffleIndex++]);
        if(_currentShuffleIndex >= _shuffledClips.Count - 1) shuffleClips();
    }

    private IEnumerator SpawnRoutine() {
        isSpawning = true;
        StartCoroutine(SpawnShadowRoutine());
        StartCoroutine(SpawnShadowShadowRoutine());
        while(isSpawning) {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            PlaceMagicalObject();
        }
    }

    // More interesting results with two spawning 'threads'
    private IEnumerator SpawnShadowRoutine() {
        while(isSpawning) {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            PlaceMagicalObject();
        }
    }

    // ... the more the merrier
    private IEnumerator SpawnShadowShadowRoutine() {
        while(isSpawning) {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            PlaceMagicalObject();
        }
    }

    private void shuffleClips() {
        var inputList = new List<AudioClip>(clips);
        _shuffledClips.Clear();
        while(inputList.Count > 0) {
            var randomIndex = Random.Range(0, inputList.Count);
            _shuffledClips.Add(inputList[randomIndex]);
            inputList.RemoveAt(randomIndex);
        }
        _currentShuffleIndex = 0;
    }
}
