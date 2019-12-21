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
    private Dictionary<MagicalObjectState, Action> _stateUpdates;
    private Action<MagicalObject> _onFadeOut;
    private Renderer _renderer;
    private AudioSource _source;

    // Start is called before the first frame update
    void Start()
    {   
        _renderer = gameObject.GetComponent<Renderer>();
        _source = gameObject.GetComponent<AudioSource>();
        _stateUpdates = new Dictionary<MagicalObjectState, Action>() {
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
        _stateUpdates[state]();
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
        _source.time = 0f;
        _source.pitch = 1f;
        _source.volume = 0.8f;
        _source.Play();
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
            if(_onFadeOut != null) _onFadeOut(this);
        }

    }

    public void SetOnFadeOutCallback(Action<MagicalObject> action) {
        _onFadeOut = action;
    }

    public void Show(Vector3 pos, Vector3 rot, float scaleFactor, Material material, AudioClip clip) {
        _renderer.material = material;
        _source.clip = clip;
        this.scaleFactorB = scaleFactor;
        gameObject.transform.position = pos;
        gameObject.transform.eulerAngles = rot;
        state = MagicalObjectState.FADE_IN;

        _source.time = 0.45f;
        _source.pitch = -1f;
        _source.volume = 0.3f;
        _source.Play();
    }
}

public enum MagicalObjectState
{
    NONE,
    FADE_IN,
    BE,
    FADE_OUT
}
