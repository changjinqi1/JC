using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureCollection : MonoBehaviour
{
    public GameObject homeBase; 
    public TextMeshProUGUI collectionText;
    public TextMeshProUGUI winText;

    private int collectedTreasures = 0;
    private int totalTreasuresToCollect = 1;

    private List<GameObject> attachedPlayers = new List<GameObject>();

    private void Start()
    {
        UpdateCollectionText();
        winText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure"))
        {
            CollectTreasure(other.gameObject);
        }
    }

    private void CollectTreasure(GameObject treasure)
    {
        collectedTreasures++;

        UpdateCollectionText();

        Destroy(treasure);

        DismissAttachedPlayers();

        if (collectedTreasures >= totalTreasuresToCollect)
        {
            ShowWinMessage();
        }
    }

    private void UpdateCollectionText()
    {
        collectionText.text = $"{collectedTreasures}/{totalTreasuresToCollect}";
    }

    private void DismissAttachedPlayers()
    {
        foreach (var player in attachedPlayers)
        {
            CharacterStates characterState = player.GetComponent<CharacterStates>();
            if (characterState != null)
            {
                characterState.isAttachedToTreasure = false;
            }
        }

        attachedPlayers.Clear();
    }

    private void ShowWinMessage()
    {
        winText.gameObject.SetActive(true);
        winText.text = "YOUWIN";
    }

    public void AttachPlayer(GameObject player)
    {
        if (!attachedPlayers.Contains(player))
        {
            attachedPlayers.Add(player);
            CharacterStates characterState = player.GetComponent<CharacterStates>();
            if (characterState != null)
            {
                characterState.isAttachedToTreasure = true;
            }
        }
    }
}
