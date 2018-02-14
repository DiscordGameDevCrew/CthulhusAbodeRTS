using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 200f;

    public Vector4 mapBoundaries; // min x, max x, min y, max y
    public Vector2 scrollLimits; // Min, Max
    Vector3 translate;

    private Vector3 velocity = Vector3.zero;
    private float dampTime = .08f;

    void LateUpdate()
    {

        MoveCamera();
        //transform.position = Vector3.Lerp(transform.position, transform.position + translate, panSpeed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + translate, ref velocity, dampTime);
        LimitCamMovement();
    }

    void MoveCamera()
    {
        translate = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical"));

        translate.x = Mathf.Clamp(translate.x, mapBoundaries.x, mapBoundaries.y);
        translate.z = Mathf.Clamp(translate.z, mapBoundaries.z, mapBoundaries.w);
        //translate.y = Mathf.Clamp(translate.y, scrollLimits.x, scrollLimits.y);

        // this.transform.Translate(translate * panSpeed * Time.deltaTime, Space.World);
    }

    void LimitCamMovement()
    {
        // Limits the camera movement so we can't look outside of the map border.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, mapBoundaries.x, mapBoundaries.y);
        pos.z = Mathf.Clamp(pos.z, mapBoundaries.z, mapBoundaries.w);
        pos.y = Mathf.Clamp(pos.y, scrollLimits.x, scrollLimits.y);
        this.transform.position = pos;
    }
}
