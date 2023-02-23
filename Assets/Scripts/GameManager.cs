using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> players;
    public List<GameObject> playersKnockedOut;

    [Header("UI")]
    [SerializeField] GameObject canvasKo;
    [SerializeField] TextMeshProUGUI winnerNameText;

    [SerializeField] List<Slider> playersHealthBars;

    [SerializeField] List<PlayerBehaviour> playersBehaviours;

    private void Update()
    {
        // HEALTHBARS
        /*if (players.Count > 0)
        {
            playersHealthBars[0].value = playersBehaviours[0].health;
        }
        if (players.Count > 1)
        {
            playersHealthBars[1].value = playersBehaviours[1].health;
        }*/
    }

    public void OnPlayerJoin()
    {
        Debug.Log("GM - Player Join");

        GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Player");

        // UPDATE PLAYER LIST
        foreach (GameObject gm in playersArray)
        {
            if (!players.Contains(gm))
            {
                players.Add(gm);
                playersBehaviours.Add(gm.GetComponent<PlayerBehaviour>());
            }
        }
        // UPDATE OTHERPLAYER LIST ON EACH PLAYERS
        foreach (GameObject gm in playersArray)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            p.UpdateOtherPlayers(players);
        }
    }

    public void UpdatePlayers()
    {
        Debug.Log("GM - Update Players");

        GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Player");

        // UPDATE PLAYER LIST
        foreach (GameObject gm in playersArray)
        {
            if (!players.Contains(gm))
            {
                players.Add(gm);
                playersBehaviours.Add(gm.GetComponent<PlayerBehaviour>());
            }
        }
        // UPDATE OTHERPLAYER LIST ON EACH PLAYERS
        foreach (GameObject gm in playersArray)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            p.UpdateOtherPlayers(players);
        }
        // UPDATE PLAYERS TRANSFORMS LIST ON CAMERA
        foreach (GameObject gm in playersArray)
        {
            //CameraBehaviour cam = Camera.main.GetComponent<CameraBehaviour>();
            Cinemachine.CinemachineTargetGroup targetGroup = GameObject.Find("TargetGroup1").GetComponent<Cinemachine.CinemachineTargetGroup>();
            Transform head = gm.transform.Find("Head").transform;
            if (targetGroup.FindMember(head) <= 0)
            {
                targetGroup.AddMember(head, 1.0f, 1.25f);
            }

            //cam.UpdatePlayersTransforms(players);
        }
    }

    public void UpdatePlayersKnockedOut()
    {
        foreach (GameObject gm in players)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            if (p.isKnockedOut)
            {
                playersKnockedOut.Add(gm);
            }
        }

        if (playersKnockedOut.Count == players.Count - 1)
        {
            string nameOfTheWinner = null;
            foreach (GameObject gm in players)
            {
                if (!playersKnockedOut.Contains(gm))
                {
                    nameOfTheWinner = gm.name;
                }
            }

            winnerNameText.text = nameOfTheWinner + " win";
            canvasKo.SetActive(true);
            Debug.Log("WIN DE : " + nameOfTheWinner);
        }
    }

    // PLAYERS CAN MOVE
    public void AllPlayerCanMoveOn()
    {
        foreach (GameObject gm in players)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            p.canMove = true;
        }
    }
    public void AllPlayerCanMoveOff()
    {
        foreach (GameObject gm in players)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            p.canMove = false;
        }
    }

    // PLAYERS CAN ATTACK
    public void AllPlayerCanAttackOn()
    {
        foreach (GameObject gm in players)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            p.canAttack = true;
        }
    }
    public void AllPlayerCanAttackOff()
    {
        foreach (GameObject gm in players)
        {
            PlayerBehaviour p = gm.GetComponent<PlayerBehaviour>();
            p.canAttack = false;
        }
    }
}