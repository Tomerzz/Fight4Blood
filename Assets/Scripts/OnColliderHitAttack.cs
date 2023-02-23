using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnColliderHitAttack : MonoBehaviour
{
    [SerializeField] CharacterAttack charAttack;
    [SerializeField] PlayerBehaviour player;
    [SerializeField] Collider playerColl;
    [SerializeField] BoxCollider thisColl;

    void Start()
    {
        charAttack = this.GetComponentInParent<CharacterAttack>();
        playerColl = this.transform.parent.GetComponent<Collider>();
        player = this.transform.parent.parent.GetComponent<PlayerBehaviour>();
        thisColl = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character" && other != playerColl)
        {
            PlayerBehaviour enemy = other.transform.parent.GetComponent<PlayerBehaviour>();

            float dmg = 0.0f;
            if (charAttack.isSuperHit) dmg = charAttack.damageSuperHit;
            else if (!charAttack.isSuperHit) dmg = charAttack.damageHit;
            else dmg = charAttack.damageSpecialAttack;

            player.Attacking(enemy, dmg);
        }
    }
}