using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public GameObject target;     
    
    //TP
    public float thirdPersonTargetHeight = 1.7f;          
    public float thirdPersonDistance = 12.0f;             
    public float thirdPersonOffsetFromWall = 0.1f;        
    public float thirdPersonMaxDistance = 20f;            
    public float thirdPersonMinDistance = 0.6f;           
    public float thirdPersonXSpeed = 200.0f;              
    public float thirdPersonYSpeed = 200.0f;              
    public float thirdPersonMinimumY = -80f;             
    public float thirdPersonMaximumY = 80f;              
    public float thirdPersonZoomRate = 40f;               
    public float thirdPersonRotationDampening = 3.0f;     
    public float thirdPersonZoomDampening = 5.0f;         
    private LayerMask _collisionLayers = -1;                                                    
    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private float _currentDistance;
    private float _desiredDistance;
    private float _correctedDistance;
    private bool _mouseSideButton = false;
    private float _pbuffer = 0.0f;              
    private float _coolDown = 0.5f;

    //FP
    public float firstPersonSensitivityX = 15F;
    public float firstPersonSensitivityY = 15F;
    public float firstPersonMinimumX = -360F;
    public float firstPersonMaximumX = 360F;
    public float firstPersonMinimumY = -60F;
    public float firstPersonMaximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    private List<float> _rotArrayX = new List<float>();
    float rotAverageX = 0F;
    private List<float> _rotArrayY = new List<float>();
    float rotAverageY = 0F;
    private float _frameCounter = 5;
    Quaternion originalRotation;
    public GameObject shootingPoint;
    public bool fpsOn = false;

    private Manager manager;

    void Start()
    {     
        Vector3 angles = transform.eulerAngles;
        _xDeg = angles.x;
        _yDeg = angles.y;
        _currentDistance = thirdPersonDistance;
        _desiredDistance = thirdPersonDistance;
        _correctedDistance = thirdPersonDistance;

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        originalRotation = transform.localRotation;

        manager = GameObject.Find("GameManager").GetComponent<Manager>();
    }

    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player") as GameObject;
        }
    }

    public GameObject MousePosition(float hitDistance)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject mouseObject = null;

        if (Physics.Raycast(ray, out hit, hitDistance))
        {
            mouseObject = hit.collider.gameObject;
        }

        return mouseObject;
    }

    void LateUpdate()
    {
        SwitchCameras();
    }

    private void SwitchCameras()
    {
        if (!manager.Paused)
        {
            if (Input.GetMouseButton(1))
            {
                FirstPersonCamera();
            }
            else
            {
                ThirdPersonCamera();
            }
        }
    }

    private void FirstPersonCamera()
    {
        fpsOn = true;
        shootingPoint.SetActive(true);
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1,  target.transform.position.z);

        rotAverageY = 0f;
        rotAverageX = 0f;

        rotationY += Input.GetAxis("Mouse Y") * firstPersonSensitivityY;
        rotationX += Input.GetAxis("Mouse X") * firstPersonSensitivityX;

        _rotArrayY.Add(rotationY);
        _rotArrayX.Add(rotationX);

            if (_rotArrayY.Count >= _frameCounter)
            {
                _rotArrayY.RemoveAt(0);
            }
            if (_rotArrayX.Count >= _frameCounter)
            {
                _rotArrayX.RemoveAt(0);
            }

            for (int j = 0; j < _rotArrayY.Count; j++)
            {
                rotAverageY += _rotArrayY[j];
            }
            for (int i = 0; i < _rotArrayX.Count; i++)
            {
                rotAverageX += _rotArrayX[i];
            }

            rotAverageY /= _rotArrayY.Count;
            rotAverageX /= _rotArrayX.Count;

            rotAverageY = ClampAngle(rotAverageY, firstPersonMinimumY, firstPersonMaximumY);
            rotAverageX = ClampAngle(rotAverageX, firstPersonMinimumX, firstPersonMaximumX);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;      
    }

    private void ThirdPersonCamera() {
        fpsOn = false;
        shootingPoint.SetActive(false);
        if (target == null)
            return;
        if (_pbuffer > 0)
            _pbuffer -= Time.deltaTime;
        if (_pbuffer < 0) _pbuffer = 0;

        if ((Input.GetAxis("Mouse ScrollWheel") != 0) && (_pbuffer == 0))
        {
            _pbuffer = _coolDown;
            _mouseSideButton = !_mouseSideButton;
        }
        if (_mouseSideButton && Input.GetAxis("Vertical") != 0)
            _mouseSideButton = false;

        Vector3 vTargetOffset;

        if (GUIUtility.hotControl == 0)
        {
            _xDeg += Input.GetAxis("Mouse X") * thirdPersonXSpeed * 0.02f;
            _yDeg -= Input.GetAxis("Mouse Y") * thirdPersonYSpeed * 0.02f;

        }
        _yDeg = ClampAngle(_yDeg, thirdPersonMinimumY, thirdPersonMaximumY);

        Quaternion rotation = Quaternion.Euler(_yDeg, _xDeg, 0);

        _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * thirdPersonZoomRate * Mathf.Abs(_desiredDistance);
        _desiredDistance = Mathf.Clamp(_desiredDistance, thirdPersonMinDistance, thirdPersonMaxDistance);
        _correctedDistance = _desiredDistance;

        vTargetOffset = new Vector3(0, -thirdPersonTargetHeight, 0);
        Vector3 position = target.transform.position - (rotation * Vector3.forward * _desiredDistance + vTargetOffset);

        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y + thirdPersonTargetHeight, target.transform.position.z);

        var isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, _collisionLayers))
        {
            _correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - thirdPersonOffsetFromWall;
            isCorrected = true;
        }

        _currentDistance = !isCorrected || _correctedDistance > _currentDistance ? Mathf.Lerp(_currentDistance, _correctedDistance, Time.deltaTime * thirdPersonZoomDampening) : _correctedDistance;

        _currentDistance = Mathf.Clamp(_currentDistance, thirdPersonMinDistance, thirdPersonMaxDistance);

        position = target.transform.position - (rotation * Vector3.forward * _currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void RotateBehindTarget()
    {
        float targetRotationAngle = target.transform.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;
        _xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, thirdPersonRotationDampening * Time.deltaTime);
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }   

}