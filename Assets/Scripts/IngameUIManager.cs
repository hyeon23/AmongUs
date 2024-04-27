using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    public static IngameUIManager instance;

    [SerializeField]
    private IngameIntroUI ingameIntroUI;
    public IngameIntroUI IngameIntroUI { get { return ingameIntroUI; } }

    private void Awake()
    {
        instance = this;
    }
}
