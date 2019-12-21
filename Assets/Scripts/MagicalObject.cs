using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicalObject : MonoBehaviour
{
    public MagicalObjectState state;
    [Range(0f, 1f)]
    public float scaleFactorA;
    public float scaleFactorB = 1f;
    [Range(0.5f, 1f)]
    public float targetScaleFactor = 0.8f;
    public float fadeInSpeed = 1.4f;
    public float fadeOutSpeed = 0.8f;
    private Dictionary<MagicalObjectState, Action> StateUpdates;
    private Action<MagicalObject> onFadeOut;
    new private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {   
        renderer = gameObject.GetComponent<Renderer>();
        StateUpdates = new Dictionary<MagicalObjectState, Action>() {
            { MagicalObjectState.NONE, UpdateNone },
            { MagicalObjectState.FADE_IN, UpdateFadeIn },
            { MagicalObjectState.BE, UpdateBe },
            { MagicalObjectState.FADE_OUT, UpdateFadeOut }
        };
        scaleFactorA = 1f;
        state = MagicalObjectState.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdates[state]();
    }

    private void UpdateScale() {
        var scale = scaleFactorA * scaleFactorB;
        gameObject.transform.localScale = new Vector3(
            scale, scale, scale
        );
    }

    private void UpdateNone()
    {
        if(scaleFactorA > 0) {
            scaleFactorA = 0;
            UpdateScale();
        }
        return;
    }

    private void UpdateFadeIn()
    {
        if(scaleFactorA < targetScaleFactor) {
            scaleFactorA += fadeInSpeed * Time.deltaTime;
            UpdateScale();
        }
        else {
            scaleFactorA = targetScaleFactor;
            state = MagicalObjectState.BE;
        }
    }

    private void UpdateBe()
    {
        if(scaleFactorA < targetScaleFactor) {
            scaleFactorA = targetScaleFactor;
            UpdateScale();
        }
        state = MagicalObjectState.FADE_OUT;
    }

    private void UpdateFadeOut()
    {
        if(scaleFactorA > 0f) {
            scaleFactorA -= fadeOutSpeed * Time.deltaTime;
            UpdateScale();
        }
        else {
            scaleFactorA = 0f;
            state = MagicalObjectState.NONE;
            if(onFadeOut != null) onFadeOut(this);
        }

    }

    public void SetOnFadeOutCallback(Action<MagicalObject> action) {
        onFadeOut = action;
    }

    public void Show(Vector3 pos, Vector3 rot, float scaleFactor, Material material) {
        renderer.material = material;
        this.scaleFactorB = scaleFactor;
        gameObject.transform.position = pos;
        gameObject.transform.eulerAngles = rot;
        state = MagicalObjectState.FADE_IN;
    }
}

public enum MagicalObjectState
{
    NONE,
    FADE_IN,
    BE,
    FADE_OUT
}
