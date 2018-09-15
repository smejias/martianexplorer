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
    private LayerMask collisionLayers = -1;                                                    
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    private bool mouseSideButton = false;
    private float pbuffer = 0.0f;              
    private float coolDown = 0.5f;

    //FP
    public float firstPersonSensitivityX = 15F;
    public float firstPersonSensitivityY = 15F;
    public float firstPersonMinimumX = -360F;
    public float firstPersonMaximumX = 360F;
    public float firstPersonMinimumY = -60F;
    public float firstPersonMaximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;
    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;
    private float frameCounter = 5;
    Quaternion originalRotation;
    public GameObject shootingPoint;
    public bool fpsOn = false;

    void Start()
    {     
        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;
        currentDistance = thirdPersonDistance;
        desiredDistance = thirdPersonDistance;
        correctedDistance = thirdPersonDistance;

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        originalRotation = transform.localRotation;

        Cursor.visible = false;

    }
    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player") as GameObject;
        }

    }

    void LateUpdate()
    {
        SwitchCameras();
    }

    private void SwitchCameras()
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

    private void FirstPersonCamera()
    {
        fpsOn = true;
        shootingPoint.SetActive(true);
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1,  target.transform.position.z);

        rotAverageY = 0f;
        rotAverageX = 0f;

        rotationY += Input.GetAxis("Mouse Y") * firstPersonSensitivityY;
        rotationX += Input.GetAxis("Mouse X") * firstPersonSensitivityX;

        rotArrayY.Add(rotationY);
        rotArrayX.Add(rotationX);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }

            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }

            rotAverageY /= rotArrayY.Count;
            rotAverageX /= rotArrayX.Count;

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
        if (pbuffer > 0)
            pbuffer -= Time.deltaTime;
        if (pbuffer < 0) pbuffer = 0;

        if ((Input.GetAxis("Mouse ScrollWheel") != 0) && (pbuffer == 0))
        {
            pbuffer = coolDown;
            mouseSideButton = !mouseSideButton;
        }
        if (mouseSideButton && Input.GetAxis("Vertical") != 0)
            mouseSideButton = false;

        Vector3 vTargetOffset;

        if (GUIUtility.hotControl == 0)
        {
                xDeg += Input.GetAxis("Mouse X") * thirdPersonXSpeed * 0.02f;

                yDeg -= Input.GetAxis("Mouse Y") * thirdPersonYSpeed * 0.02f;

        }
        yDeg = ClampAngle(yDeg, thirdPersonMinimumY, thirdPersonMaximumY);

        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);

        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * thirdPersonZoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, thirdPersonMinDistance, thirdPersonMaxDistance);
        correctedDistance = desiredDistance;

        vTargetOffset = new Vector3(0, -thirdPersonTargetHeight, 0);
        Vector3 position = target.transform.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);

        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y + thirdPersonTargetHeight, target.transform.position.z);

        var isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
        {
            correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - thirdPersonOffsetFromWall;
            isCorrected = true;
        }

        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * thirdPersonZoomDampening) : correctedDistance;

        currentDistance = Mathf.Clamp(currentDistance, thirdPersonMinDistance, thirdPersonMaxDistance);

        position = target.transform.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void RotateBehindTarget()
    {
        float targetRotationAngle = target.transform.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;
        xDeg = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, thirdPersonRotationDampening * Time.deltaTime);

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