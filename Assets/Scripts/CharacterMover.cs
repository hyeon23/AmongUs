using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Bcpg;
using TMPro;

public class CharacterMover : NetworkBehaviour
{
    private Animator animator;

    private bool isMovable;

    public bool IsMovable
    {
        get { return isMovable; }
        set
        {
            if (!value)
            {
                animator.SetBool("isMove", false);
            }
            isMovable = value;
        }
    }

    [SyncVar]
    public float speed = 2f;

    [SerializeField]
    private float characterSize = 0.5f;

    [SerializeField]
    private float cameraSize = 2.5f;

    private SpriteRenderer spriteRenderer;

    [SyncVar(hook = nameof(SetPlayerColorHook))]
    public EPlayerColor playerColor;

    public void SetPlayerColorHook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    [SyncVar(hook = nameof(SetNickname_Hook))]
    public string nickname;
    [SerializeField]
    private TextMeshProUGUI nicknameTMP;
    public void SetNickname_Hook(string _, string value)
    {
        nicknameTMP.text = value;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));

        if (isOwned)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = cameraSize;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(isOwned && IsMovable)
        {
            bool isMove = false;

            if(PlayerSettings.controlType == EControlType.Mouse)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;

                    if (dir.x < 0f) transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                    else if (dir.x > 0f) transform.localScale = new Vector3(characterSize, characterSize, 1f);

                    transform.position += dir * speed * Time.deltaTime;

                    isMove = dir.magnitude != 0f;
                }
            }
            else if (PlayerSettings.controlType == EControlType.KeyboardMouse)
            {
                Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
                
                if (dir.x < 0f) transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                else if (dir.x > 0f) transform.localScale = new Vector3(characterSize, characterSize, 1f);
                
                transform.position += dir * speed * Time.deltaTime;

                isMove = dir.magnitude != 0f;
            }

            animator.SetBool("isMove", isMove);
        }
        if(transform.localScale.x < 0)
        {
            nicknameTMP.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (transform.localScale.x > 0)
        {
            nicknameTMP.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
