using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    ///���̿� ���� Sprite ���� ���
    //-Back�� Front�� ���� ������ �༭ ����
    //-���� ���� ����ϹǷ�, GameObject�� ��ǥ�� Back or Front�� ��ȸ�ϴ� ��� ���� �߻�
    [SerializeField]
    private Transform Back;
    [SerializeField]
    private Transform Front;
    public int GetSortingOrder(GameObject go)
    {
        float objDist = Mathf.Abs(Back.position.y - go.transform.position.y);
        float totalDist = Mathf.Abs(Back.position.y - Front.position.y);

        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));
    }
}
