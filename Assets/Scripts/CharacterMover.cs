using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Bcpg;

public class CharacterMover : NetworkBehaviour
{
    private Animator animator;

    public bool isMovable;

    [SyncVar]//��Ʈ��ũ ����ȭ
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (isOwned)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = 2.5f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(isOwned && isMovable)//���� O & �̵� ����
        {
            bool isMove = false;

            if(PlayerSettings.controlType == EControlType.Mouse)
            {
                if (Input.GetMouseButton(0))
                {
                    //���� ��Ŀ� ���� ���⺤��
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;

                    //��������Ʈ ����
                    if (dir.x < 0f) transform.localScale = new Vector3(-0.75f, 0.75f, 1f);
                    else if (dir.x > 0f) transform.localScale = new Vector3(0.75f, 0.75f, 1f);

                    //������ �� �̵�
                    transform.position += dir * speed * Time.deltaTime;

                    isMove = dir.magnitude != 0f;
                }
            }
            else if (PlayerSettings.controlType == EControlType.KeyboardMouse)
            {
                //���� ��Ŀ� ���� ���⺤��
                Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
                
                //��������Ʈ ����
                if (dir.x < 0f) transform.localScale = new Vector3(-0.75f, 0.75f, 1f);
                else if (dir.x > 0f) transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                
                //������ �� �̵�
                transform.position += dir * speed * Time.deltaTime;

                isMove = dir.magnitude != 0f;
            }

            animator.SetBool("isMove", isMove);
        }
    }
}
