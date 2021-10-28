//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween AlphaUI")]
public class TweenAlphaUI : UITweener
{
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;
	public bool _childInclude;

	bool mCached = false;
	//	UIRect mRect;
	//	Material mMat;
	//	SpriteRenderer mSr;
	[SerializeField] float _currentAlpha = 0;
	[SerializeField]private Image _thisImage;
	[SerializeField]private Color _thisColor;
	[SerializeField]private Image[] _imageList;
	[SerializeField]private Color[] _imageColor;
	[SerializeField]private TextMeshProUGUI _thisText;
	[SerializeField]private Color _thisTextColor;
	[SerializeField]private TextMeshProUGUI[] _textList;
	[SerializeField]private Color[] _textColor;

	[System.Obsolete("Use 'value' instead")]
	public float alpha { get { return this.value; } set { this.value = value; } }

    public override void PlayForward()
    {
        //ResetCache();
        base.PlayForward();
    }

    public override void PlayReverse()
    {
        //ResetCache();
        base.PlayReverse();
    }

    void Cache ()
	{
		_thisImage = GetComponent<Image>();
		_thisText = GetComponent<TextMeshProUGUI>();

		if (_childInclude)
		{
			_imageList = GetComponentsInChildren<Image>(true);
			_imageColor = new Color[_imageList.Length];
			_textList = GetComponentsInChildren<TextMeshProUGUI>(true);
			_textColor = new Color[_textList.Length];
		}
		if (_thisImage != null)
		{
			_thisColor = _thisImage.color;
		}

		if (_thisText != null)
		{
			_thisTextColor = _thisText.color;
		}
		if (_childInclude)
		{
			for (int i = 0; i < _imageList.Length; i++)
			{
				_imageColor[i] = _imageList[i].color;
			}

			for (int i = 0; i < _textList.Length; i++)
			{
				_textColor[i] = _textList[i].color;
			}
		}
		mCached = true;
		//	mRect = GetComponent<UIRect>();
		//	mSr = GetComponent<SpriteRenderer>();

		//	if (mRect == null && mSr == null)
		//	{
		//		Renderer ren = GetComponent<Renderer>();
		//		if (ren != null) mMat = ren.material;
		//		if (mMat == null) mRect = GetComponentInChildren<UIRect>();
		//	}
	}

    public void ResetCache()
    {
        mCached = false;
        Cache();
    }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	private void SetAlpha(float setAlpha)
	{
		if (_thisImage != null)
		{
            _thisColor = _thisImage.color;
            _thisColor.a = setAlpha;
			_thisImage.color = _thisColor;
		}

		if (_thisText != null)
		{
			_thisTextColor.a = setAlpha;
			_thisText.color = _thisTextColor;
		}

		if (_childInclude)
		{
			for (int i = 0; i < _imageList.Length; i++)
			{
				_imageColor[i].a = setAlpha;
				_imageList[i].color = _imageColor[i];
			}

			for (int i = 0; i < _textList.Length; i++)
			{
				_textColor[i].a = setAlpha;
				_textList[i].color = _textColor[i];
			}
		}

		_currentAlpha = setAlpha;
	}


	public float value
	{
		get
		{

			return _currentAlpha;
		}
		set
		{
			_currentAlpha = value;

			if (!mCached) Cache();

			SetAlpha (value);
		}
	}

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Mathf.Lerp(from, to, factor);
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>



    static public TweenAlpha Begin (GameObject go, float duration, float alpha)
	{
		TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
	[ContextMenu("Cache")]
	void CacheSet()
	{
		mCached = false;
		Cache ();
	}
	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
