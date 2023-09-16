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
        if(Input.touchCount>0)//��ġ�� �Ͼ���� Ȯ��
        {
            Touch touch = Input.GetTouch(0);//��ġ�� �Ͼ ������ ���� ù��° ��ġ����
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if(arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                //ù��° hit�� ������ �ޱ�
                Pose hitpose = hits[0].pose;
                //�Ѵ��� �������� ������    //���� ������Ʈ ���� ���� �� ��ġ�� �Ͼ ������ ȭ�鿡 ������
                if(!spawnObject)//���� ������Ʈ�� ���ٸ�
                {
                    spawnObject =  Instantiate(placeObject, hitpose.position, hitpose.rotation) ;
                }
                else// �̹� ���� �Ǿ��� �ֱ⶧���� ������Ʈ�� ����
                {
                    spawnObject.transform.position = hitpose.position;
                    spawnObject.transform.rotation = hitpose.rotation;
                }

            }
        }
    }

    private void UpdateCenterObject()//ȭ�� ���� �κп� ������Ʈ�� �� �����Ӹ��� ��ġ��Ű�� �Լ�(ī�޶� ����)
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); //viewport��ǥ���� screen��ǥ�� ��ȯ, ī�޶�ũ���� ���������� �޾ƿ��� �Լ�
        //ar raycast�� ������ ���� �ű� �浹�Ǵ� ��� ������Ʈ����

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes); // -> plane�̶�� ��ü�� ray�� ������ �� ������� ��ȯ�ްڴ�.
        //hitResults: ray�� ���� �浹�Ǵ� ��ü���� list ���·� ��ȯ
        //trackableType : � Ÿ���� ������Ʈ�鸸 ����Ұ���

        if(hits.Count > 0 )//���� ray�� �ε��� ��ü�� �ִٸ�
        {
            Pose placementPose = hits[0].pose;//������� �ε��� ��ü
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }
}
