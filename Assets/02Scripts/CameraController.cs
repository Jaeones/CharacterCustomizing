using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject cameraParent;
    Vector3 defaultPosition;
    Quaternion defaultRotation;
    float defaultFOV;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraParent = GameObject.Find("CameraParent");
        defaultPosition = Camera.main.transform.position;
        defaultRotation = cameraParent.transform.rotation;
        defaultFOV = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        //휠 클릭 카메라 초기화 (가장 먼저 체크)
        if(Input.GetMouseButtonDown(2))
        {
            
            // 부모 오브젝트의 회전 초기화 (회전은 부모 오브젝트에서 관리)
            cameraParent.transform.rotation = defaultRotation;
            // 자식 카메라의 위치 초기화
            Camera.main.transform.position = defaultPosition;
            // FOV 초기화
            Camera.main.fieldOfView = defaultFOV;
            return; // 초기화 후 이번 프레임은 더 이상 처리하지 않음
        }
        
        //만약에 마우스 왼쪽버튼을 누른상태로 유지하면 카메라이동
        //마우스 오른쪽 버튼을 누른 상태로 유지한다면 카메라 회전
        if(Input.GetMouseButton(0))
        {
            Camera.main.transform.Translate(Input.GetAxisRaw("Mouse X")/ 10f, Input.GetAxisRaw("Mouse Y")/ 10f, 0 );
        }
        if(Input.GetMouseButton(1))
        {
            cameraParent.transform.Rotate(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0 );
        }
        //휠 : 줌인 줌아웃
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.fieldOfView -= 1;
            if(Camera.main.fieldOfView < 10)
            {
                Camera.main.fieldOfView = 10;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.fieldOfView += 1;
            if(Camera.main.fieldOfView > 60)
            {
                Camera.main.fieldOfView = 60;
            }
        }
    }
}
