using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSequence : MonoBehaviour
{
    new public Light light;
    public GameObject idol01;
    public GameObject idol02;
    public GameObject idol03;
    public Spawner spawner;
    private double _targetLightIntensity;
    private float _pause = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _targetLightIntensity = light.intensity;
        light.intensity = 0f;
        idol01.SetActive(false);
        idol02.SetActive(false);
        idol03.SetActive(false);

        StartCoroutine(BeginSceneRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator BeginSceneRoutine() {
        while(light.intensity < _targetLightIntensity) {
            light.intensity += 0.001f;
            yield return true;
        }
        yield return new WaitForSeconds(_pause);
        idol01.SetActive(true);
        yield return new WaitForSeconds(_pause);
        idol02.SetActive(true);
        yield return new WaitForSeconds(_pause);
        idol03.SetActive(true);
        yield return new WaitForSeconds(_pause);
        spawner.Begin();

        StopAllCoroutines();
    }
}
