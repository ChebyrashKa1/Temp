using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Tween/Tween Waypoint Follow UI")]
public class TweenWaypointFollowUI : UITweener {
    private RectTransform   mTrans;
    public RectTransform    cachedTransform {
        get {
            if (mTrans == null)
                mTrans = gameObject.GetComponent<RectTransform> ();
            return mTrans;
        }
    }
	public Vector2          transformPosition  {
		get {
			return cachedTransform.anchoredPosition;
		}
		set {
            cachedTransform.anchoredPosition = value;
		}
	}

    private float           totalPathDistance {
        get {
            float distance = 0f;
            if (Waypoints != null && Waypoints.Count > 0)
                for (int i = 1; i <  Waypoints.Count; i++)
                    distance += Vector2.Distance(Waypoints[i - 1], Waypoints[i]);

            return distance;
        }
    }

#if UNITY_EDITOR
    [Header("In Editor Paths Config")]
    public bool showGizmos      = true;
    public bool scaleWithParent = false;
    private Vector2 oldDelta    = Vector2.zero;
    private void OnDrawGizmos() {
        var parentT = transform.parent.GetComponentInParent<RectTransform>();

        if (oldDelta == Vector2.zero) {
            oldDelta = new Vector2(parentT.rect.width, parentT.rect.height);
        }

        if (parentT.hasChanged && scaleWithParent) {
            var parentSize = new Vector2(parentT.rect.width, parentT.rect.height);
            ScaleWaypoints(parentSize / oldDelta);
            oldDelta = parentSize;
        }

        if (!showGizmos)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < Waypoints.Count; i++) {
            var position = parentT.TransformPoint(Waypoints[i]);

            Gizmos.DrawWireSphere(position, 1f);
            Handles.Label(position, " i: " + i.ToString());

            if (i > 0) {
                var o_position = parentT.TransformPoint(Waypoints[i - 1]);
                Gizmos.DrawLine(o_position, position);
            }
        }
    }
    void OnValidate() {
        RebuildPathValues();
    }
#endif

    [Space(10f), Header("Runtime Paths Config")]
    public bool             RotateToTarget  = false;
    [SerializeField]
    private List<Vector2>   Waypoints       = new List<Vector2>();
    private List<float>     WayParts        = new List<float>();

    public List<Vector2>    path {
        get { return Waypoints; }
        set {
            if (value == null || value.Count < 2)
                return;
            Waypoints = value;
            RebuildPathValues();
        }
    }

    private void Awake () {
        if (Waypoints.Count != WayParts.Count)
            RebuildPathValues();
    }

    // Tween Loop, factor = progress
	protected override void OnUpdate (float factor, bool isFinished) {
        for (int i = 0; i < WayParts.Count - 1; i++) {
            if (WayParts[i] <= factor && factor < WayParts[i + 1]) {
                var currentStepSize     = WayParts[i + 1] - WayParts[i];
                var myProgress          = (factor - WayParts[i]) / currentStepSize;
                transformPosition       = Vector2.Lerp(Waypoints[i], Waypoints[i + 1], myProgress);

                if (RotateToTarget) {
                    var dir         = (Waypoints[i + 1] - Waypoints[i]).normalized;
                    var angle       = Mathf.Atan2(dir.y, dir.x) * 180f / 3.14f - 90f;
                    mTrans.rotation = Quaternion.Euler(0, 0, angle);
                }
                break;
            }
        }
    }


    [ContextMenu("Add new Waypoint")]
	public void AddNewWaypointPosition () {
        Waypoints.Add(transformPosition);
    }

	[ContextMenu("Set last Waypoint to current position")]
	public override void SetStartToCurrentValue () {
        if (Waypoints == null || Waypoints.Count == 0)
            return;

        var last = Waypoints.Count - 1;
        Waypoints[last] = transformPosition;
    }

    [ContextMenu("Add Mirrored Waypoints")]
	public void AddMirroredWaypoints () {
        if (Waypoints == null || Waypoints.Count == 0)
            return;

        var size = Waypoints.Count;
        for (int i = size - 1; i >= 0; i--) {
            Waypoints.Add(Waypoints[i] * (-Vector2.right + Vector2.up));
        }

        RebuildPathValues();
    }



    [ContextMenu("Assume to start")]
    private void SetCurrentValueToStart () {
        if (Waypoints == null || Waypoints.Count == 0)
            return;

        transformPosition = Waypoints[0];
    }

    [ContextMenu("Assume to end")]
    private void SetCurrentValueToEnd () {
        if (Waypoints == null || Waypoints.Count == 0)
            return;

        var last = Waypoints.Count - 1;
        transformPosition = Waypoints[last];
    }


    private void RebuildPathValues() {
        WayParts.Clear();
        WayParts.Add(0f);

        var distance    = totalPathDistance;
        var i_dist      = 0f;

        for (int i = 1; i < Waypoints.Count; i++) {
            i_dist += Vector2.Distance(Waypoints[i - 1], Waypoints[i]);
            float clamped = i_dist / distance;
            WayParts.Add(clamped);
        }
    }

    private void ScaleWaypoints(Vector2 scale) {
        for (int i = 0; i < Waypoints.Count; i++) {
            Waypoints[i] *= scale;
        }
    }

}
