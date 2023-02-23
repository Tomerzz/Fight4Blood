using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    [SerializeField] GameManager gameManager;

    public float moveSpeed = 10.0f;
    public float jumpForce = 50.0f;
    public float fallMultiplyer = 2.0f;
    public float runSpeed = 5.0f;
    public float dashForce = 50.0f;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerBehaviour>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        UpdateColor();
    }

    [SerializeField] SkinnedMeshRenderer skin;
    [SerializeField] Material[] materialsRed;
    [SerializeField] Material[] materialsBlue;
    void UpdateColor()
    {
        if (gameManager.players.Count > 1)
        {
            //skin rouge
            skin.materials = materialsRed;
        }
        else
        {
            //skin bleu
            skin.materials = materialsBlue;
        }
    }

    public void CanMoveOn() { player.canMove = true; }
    public void CanMoveOff() { player.canMove = false; }

    public void AllPlayersCanMoveOn() { gameManager.AllPlayerCanMoveOn(); }
    public void AllPlayersCanMoveOff() { gameManager.AllPlayerCanMoveOff(); }
}
