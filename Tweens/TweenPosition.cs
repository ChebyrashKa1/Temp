//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
    public Vector3 from;
    public Vector3 to;

    //[HideInInspector]
    public bool worldSpace = false;

    public bool IgnoreX = false;
    public bool IgnoreY = false;
    public bool IgnoreZ = false;

    Transform mTrans;

    public Transform cachedTransform {
        get {
            if (mTrans == null)
                mTrans = transform;
            return mTrans;
        }
    }

    [System.Obsolete("Use 'value' instead")]
    public Vector3 position { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>
    public Vector3 value {
        get {
            return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
        }
        set {
            var target  = value;
            if (IgnoreX || IgnoreY || IgnoreZ) {
                var pos = cachedTransform.position;

                if (!worldSpace)
                    pos = cachedTransform.localPosition ;

                target.x = IgnoreX ? pos.x : target.x;
                target.y = IgnoreY ? pos.y : target.y;
                target.z = IgnoreZ ? pos.z : target.z;
            }

            if (worldSpace)
                cachedTransform.position = target;
            else
                cachedTransform.localPosition = target;
        }
    }

    void Awake() { }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) {
        value = from * (1f - factor) + to * factor;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos) {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f) {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>
    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace) {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.worldSpace = worldSpace;
        comp.from       = comp.value;
        comp.to         = pos;

        if (duration <= 0f) {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }

}