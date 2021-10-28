//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System;
/// <summary>
/// Tween the object's position.
/// </summary>
[Serializable]
[AddComponentMenu("NGUI/Tween/Tween Rect UI")]
public class TweenRectUI: UITweener
{
	public Rect from;
	public Rect to;
	//public Rect temp;
	[HideInInspector]
	public bool worldSpace = false;

	RectTransform mTrans;
	RectTransform mRect;

	public RectTransform cachedTransform { get { if (mTrans == null) mTrans = gameObject.GetComponent<RectTransform> (); return mTrans; } }

	[System.Obsolete("Use 'value' instead")]
	public Rect rect { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Rect value
	{
		get
		{

      return new Rect( cachedTransform.anchoredPosition.x, cachedTransform.anchoredPosition.y, cachedTransform.sizeDelta.x, cachedTransform.sizeDelta.y );
		}
		set
		{
      cachedTransform.anchoredPosition = new Vector2( value.x, value.y );
      cachedTransform.sizeDelta = new Vector2( value.width, value.height );
				 //cachedTransform.rect.Set(value.x,value.y,value.width,value.height);
		}
	}

	void Awake () { mRect = GetComponent<RectTransform>(); }

  /// <summary>
  /// Tween the value.
  /// </summary>

  protected override void OnUpdate(float factor, bool isFinished)
  {
    Rect _rect = from;
    _rect.x =from.x+ ( to.x - from.x) * factor;
    _rect.y = from.y + ( to.y - from.y ) * factor;
    //( 1f - factor ) + to.y * factor;
    _rect.width = from.width + ( to.width - from.width ) * factor;
    //( 1f - factor ) + to.width * factor;
    _rect.height = from.height + ( to.height - from.height ) * factor;
    //( 1f - factor ) + to.height * factor;
    value = _rect;
  }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenRectUI Begin (GameObject go, float duration, Rect pos)
	{
		TweenRectUI comp = UITweener.Begin<TweenRectUI>(go, duration);
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

	static public TweenRectUI Begin (GameObject go, float duration, Rect pos, bool worldSpace)
	{
    TweenRectUI comp = Begin( go, duration, pos );
		comp.worldSpace = worldSpace;

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
