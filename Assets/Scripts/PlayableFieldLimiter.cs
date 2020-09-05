using UnityEngine;

public class PlayableFieldLimiter : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;
    private static Camera _mainCamera;

    private static PlayableFieldLimiter _i;

    private void Awake()
    {
        _i = this;
        _mainCamera = Camera.main;
    }

    public static Vector2 GetPlayableField(bool withOffset)
    {
        float height = Screen.height;
        float width = Screen.width;

        Vector3 screenResolution = new Vector3(width, height, 0);
        Vector2 worldResolution = _mainCamera.ScreenToWorldPoint(screenResolution);
        Vector2 limitedResolution = worldResolution;

        if (withOffset)
        {
            limitedResolution -= _i._offset;
        }
        
        return limitedResolution;
    }
}
