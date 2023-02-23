using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    [SerializeField] GameManager gameManager;
    public Camera SpecialAttackCam;
    [SerializeField] Camera mainCam;
    Animator anim;

    public Transform fxTransform;
    public float damageHit = 10.0f;
    public float damageSuperHit = 15.0f;
    public float damageSpecialAttack = 30.0f;
    public Vector3 velocityHit = Vector3.zero;

    public bool isSuperHit = false;

    public VisualEffect vfxHit;
    public VisualEffect vfxSuperHit;
    public VisualEffect vfxGuard;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerBehaviour>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mainCam = Camera.main;
        SpecialAttackCam.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    public void CanAttackOn() { player.canAttack = true; }
    public void CanAttackOff() { player.canAttack = false; }
    public void AllPlayersCanAttackOn() { gameManager.AllPlayerCanAttackOn(); }
    public void AllPlayersCanAttackOff() { gameManager.AllPlayerCanAttackOff(); }

    public void SuperHitStart(AnimationEvent _event)
    {
        isSuperHit = true;

        if (_event.stringParameter == "x")
        {
            velocityHit = new Vector3(1, 0, 0);
        }
        else if (_event.stringParameter == "y")
        {
            velocityHit = new Vector3(0, 1, 0);
        }
        else if (_event.stringParameter == "xy")
        {
            velocityHit = new Vector3(1f, 1f, 0);
        }
    }
    public void SuperHitEnd() { isSuperHit = false; }

    public void SpecialAttack() { player.SpecialAttack(); }

    public void ComboStart() { anim.SetBool("InCombo", true); }
    public void ComboEnd() { anim.SetBool("InCombo", false); }

    public void ResetCamera()
    {
        SpecialAttackCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }
}
