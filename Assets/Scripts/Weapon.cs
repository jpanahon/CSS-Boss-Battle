using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using TMPro;
using Cinemachine;
using StarterAssets;

public abstract class Weapon : MonoBehaviour
{
     public abstract void Primary();

     public abstract void Secondary();

    [Header("Weapon Name")]
    [SerializeField] public string modelName;

    [Header("Arm Display Settings")]

    [SerializeField] public bool rightHandEnabled = false;
    [SerializeField] public bool leftHandEnabled = false;

    [SerializeField] public RigBuilder rigBuilder;
    [SerializeField] public Transform rightGrip;
    [SerializeField] public Transform leftGrip;
    [SerializeField] public TwoBoneIKConstraint leftHand;
    [SerializeField] public TwoBoneIKConstraint rightHand;


    [Header("View Settings")]
    [SerializeField] public CinemachineVirtualCamera cam;
    [SerializeField] public TextMeshProUGUI weaponStats;

    protected StarterAssetsInputs _input;
    
    protected virtual void Start()
    {
        _input = transform.root.GetComponent<StarterAssetsInputs>();
    }

    public virtual void UpdateDisplay()
    {
        rigBuilder.Clear();

        if (rightHandEnabled && rightGrip != null)
        {
            rightHand.data.target = rightGrip;
        }

        if (leftHandEnabled && leftGrip != null)
        {
            leftHand.data.target = leftGrip;
        }

        rigBuilder.Build();
    }

    protected virtual void OnEnable()
    {
        UpdateDisplay();
    }
}
