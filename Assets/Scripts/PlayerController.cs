using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private CharacterController _controller;
    public Transform[] brushes;

    public byte selectedBrush;

    public CinemachineFreeLook _camera;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCameraTarget(brushes[selectedBrush]);
    }

    void UpdateCameraTarget(Transform target)
    {
        _camera.LookAt = target;
        _camera.Follow = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            selectedBrush++;
            if (selectedBrush >= brushes.Length)
                selectedBrush = 0;
            UpdateCameraTarget(brushes[selectedBrush]);
        }
        var hor_axis = Input.GetAxis("Horizontal");
        var ver_axis = Input.GetAxis("Vertical");

        if (hor_axis != 0 || ver_axis != 0)
        {
            brushes[selectedBrush].Translate(new Vector3(hor_axis, 0, ver_axis) * speed * Time.deltaTime);
            var pos = brushes[selectedBrush].position;
            var newPos = new Vector3(Mathf.Clamp(pos.x,0,256),pos.y,Mathf.Clamp(pos.z,0,256));
            brushes[selectedBrush].position = newPos;
        }
    }
}