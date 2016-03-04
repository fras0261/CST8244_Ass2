using UnityEngine;
using System.Collections;

//http://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055
public class ScreenWarp : MonoBehaviour {

    private Camera _mainCamera; 
    private Renderer[] _renderers;
    private Vector3 _wrappedPosition;
    private bool _isWrappingOnX = false;
    private bool _isWrappingOnY = false;

	// Use this for initialization
	void Start () {
        _renderers = GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        ScreenWrap();
	}

    /// <summary>
    /// Checks to see if the renderer of the GameObject is visible
    /// </summary>
    /// <returns>True if the renderer is visible, false if not</returns>
    bool CheckRenderers()
    {
        foreach (var renderer in _renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Wraps the gameObject on the opposite side of axis of that it exited on
    /// </summary>
    void ScreenWrap()
    {
        if (CheckRenderers() == true)
        {
            _isWrappingOnX = false;
            _isWrappingOnY = false;
            return;
        }

        if (_isWrappingOnX && _isWrappingOnY)
            return;

        _wrappedPosition = transform.position;
        var viewport = Camera.main.WorldToViewportPoint(transform.position);

        if (!_isWrappingOnX && (viewport.x > 1 || viewport.x < 0))
        {
            _wrappedPosition.x = -_wrappedPosition.x;

            _isWrappingOnX = true;
        }

        if (!_isWrappingOnY && (viewport.y > 1 || viewport.y < 0))
        {
            _wrappedPosition.y = -_wrappedPosition.y;
            _isWrappingOnY = true;
        }

        transform.position = _wrappedPosition;
    }
}
