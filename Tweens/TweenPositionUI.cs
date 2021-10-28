//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Position UI")]
public class TweenPositionUI : UITweener
{
	public Vector2 from;
	public Vector2 to;
	public Vector2 temp;
	[HideInInspector]
	public bool worldSpace = false;

	RectTransform mTrans;
	RectTransform mRect;

	public RectTransform cachedTransform { get { if (mTrans == null) mTrans = gameObject.GetComponent<RectTransform> (); return mTrans; } }

	[System.Obsolete("Use 'value' instead")]
	public Vector3 position { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Vector2 value
	{
		get
		{
			return worldSpace ? cachedTransform.anchoredPosition : cachedTransform.anchoredPosition;
		}
		set
		{
				
				 cachedTransform.anchoredPosition = value;
		
		}
	}

	void Awake () { mRect = GetComponent<RectTransform>(); }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) {

		value = from * (1f - factor) + to * factor; }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenPositionUI Begin (GameObject go, float duration, Vector3 pos)
	{
		TweenPositionUI comp = UITweener.Begin<TweenPositionUI>(go, duration);
		comp.from = comp.value;
		comp.to = pos;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenPositionUI Begin (GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPositionUI comp = UITweener.Begin<TweenPositionUI>(go, duration);
		comp.worldSpace = worldSpace;
		comp.from = comp.value;
		comp.to = pos;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}
