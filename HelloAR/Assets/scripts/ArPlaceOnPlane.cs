using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    GameObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCenterObject();
        PlaceObjectByTouch();
    }

    private void PlaceObjectByTouch()
    {
        if(Input.touchCount>0)//터치가 일어났는지 확인
        {
            Touch touch = Input.GetTouch(0);//터치가 일어난 순서를 저장 첫번째 터치기준
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                //첫번째 hit의 포지션 받기
                Pose hitpose = hits[0].pose;
                //한대의 포르세만 나오게    //스폰 오브젝트 지정 없을 시 터치가 일어날 때마다 화면에 나오게
                if(!spawnObject)//나온 오브젝트가 없다면
                {
                    spawnObject =  Instantiate(placeObject, hitpose.position, hitpose.rotation) ;
                }
                else// 이미 스폰 되어져 있기때문에 업데이트만 진행
                {
                    spawnObject.transform.position = hitpose.position;
                    spawnObject.transform.rotation = hitpose.rotation;
                }

            }
        }
    }

    private void UpdateCenterObject()//화면 센터 부분에 오브젝트를 매 프레임마다 위치시키는 함수(카메라 기준)
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); //viewport좌표에서 screen좌표로 변환, 카메라스크린의 센터지점을 받아오는 함수
        //ar raycast에 광선을 쏴서 거기 충돌되는 모든 오브젝트감지

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes); // -> plane이라는 객체에 ray가 닿으면 그 결과값을 반환받겠다.
        //hitResults: ray를 쏴서 충돌되는 객체들을 list 형태로 변환
        //trackableType : 어떤 타입의 오브젝트들만 허용할건지

        if(hits.Count > 0 )//뭔가 ray에 부딪힌 객체가 있다면
        {
            Pose placementPose = hits[0].pose;//가장먼저 부딪힌 객체
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }
}
