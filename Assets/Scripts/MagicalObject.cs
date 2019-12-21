using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicalObject : MonoBehaviour
{
    public MagicalObjectState state;
    public Color color;
    [Range(0f, 1f)]
    public float alpha;
    [Range(0.5f, 1f)]
    public float targetAlpha = 0.8f;
    public float fadeInSpeed = 1.4f;
    public float fadeOutSpeed = 0.8f;
    public float scaleFactor = 1f;
    private Dictionary<MagicalObjectState, Action> StateActions;
    private Material material;
    new private Renderer renderer;
    private Action<MagicalObject> onFadeOut;

    // Start is called before the first frame update
    void Start()
    {   
        StateActions = new Dictionary<MagicalObjectState, Action>() {
            { MagicalObjectState.NONE, UpdateNone },
            { MagicalObjectState.FADE_IN, UpdateFadeIn },
            { MagicalObjectState.BE, UpdateBe },
            { MagicalObjectState.FADE_OUT, UpdateFadeOut },
            { MagicalObjectState.MANUAL, UpdateManual }
        };
        renderer = gameObject.GetComponent<Renderer>();
        material = renderer.material;
        alpha = 1f;
        state = MagicalObjectState.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        StateActions[state]();
    }

    private void UpdateMaterialColor()
    {
        color.a = alpha;
        // material.color = color;
        material.SetColor(
            "_Color",
            color
        );
    }

    private void UpdateScale() {
        var scaleByAlpha = scaleFactor * alpha;
        gameObject.transform.localScale = new Vector3(
            scaleByAlpha, scaleByAlpha, scaleByAlpha
        );
    }

    private void UpdateNone()
    {
        if(alpha > 0) {
            alpha = 0;
            UpdateMaterialColor();
            UpdateScale();
        }
        return;
    }

    private void UpdateFadeIn()
    {
        if(alpha < targetAlpha) {
            alpha += fadeInSpeed * Time.deltaTime;
            UpdateMaterialColor();
            UpdateScale();
        }
        else {
            alpha = targetAlpha;
            state = MagicalObjectState.BE;
        }
    }

    private void UpdateBe()
    {
        if(alpha < targetAlpha) {
            alpha = targetAlpha;
            UpdateMaterialColor();
            UpdateScale();
        }
        state = MagicalObjectState.FADE_OUT;
    }

    private void UpdateFadeOut()
    {
        if(alpha > 0f) {
            alpha -= fadeOutSpeed * Time.deltaTime;
            UpdateMaterialColor();
            UpdateScale();
        }
        else {
            alpha = 0f;
            state = MagicalObjectState.NONE;
            if(onFadeOut != null) onFadeOut(this);
        }

    }

    private void UpdateManual()
    {
        UpdateMaterialColor();
        UpdateScale();
    }

    public void SetOnFadeOutCallback(Action<MagicalObject> action) {
        onFadeOut = action;
    }

    public void Show(Vector3 pos, Vector3 rot, float scaleFactor, Material material) {
        renderer.material = material;
        color = material.color;
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
    FADE_OUT,
    MANUAL
}
