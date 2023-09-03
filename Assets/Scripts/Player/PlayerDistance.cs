using Cinemachine;
using Mirror;
using UnityEngine;
using Player;
using TMPro;
using UnityEngine.UI;

public class PlayerDistance : MonoBehaviour
{
    private Network.NetworkManager _networkManager;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI playerNameText;
    public float yOffset = 2f;
    public float minRange = 10f;
    private PlayerController[] players;
    private GameObject otherPlayer;
    private bool isPlayerVisible;
    public Image playerIcon;

    [Header("Icons")]
    public Sprite boyIcon;
    public Sprite girlIcon;

    private void Start()
    {
        _networkManager = FindObjectOfType<Network.NetworkManager>();
        distanceText.enabled = false;
        CinemachineCore.CameraUpdatedEvent.AddListener(SetScreenPosition);

        RoomPlayer roomPlayer;
        // set player name
        if (NetworkServer.active)
        {
            roomPlayer = _networkManager.GetRoomClient();
        }
        else
        {
            roomPlayer = _networkManager.GetRoomHost();
        }
        
        if (roomPlayer != null) playerNameText.text = roomPlayer.playerName;
    }

    private void Update()
    {
        if (_networkManager != null)
        {
            // Berechne die Distanz zwischen den Spielern
            float distance = PlayerController.GetPlayerSeperation();
            players = PlayerController.GetPlayers();
            distanceText.text = "" + Mathf.FloorToInt(distance)+ "m";
            if (distance < minRange)
            {
                distanceText.enabled = false;
                playerNameText.enabled = false;
                playerIcon.gameObject.SetActive(false);
            }
            else
            {
                distanceText.enabled = true;
                playerNameText.enabled = true;
                playerIcon.gameObject.SetActive(true);
            }

            // Überprüfe, ob der zweite Spieler beigetreten ist
            if (players.Length == 2 && otherPlayer == null)
            {
                foreach (var player in players)
                {
                    if (player.gameObject != gameObject)
                    {
                        otherPlayer = player.gameObject;
                        distanceText.enabled = true;
                        playerIcon.gameObject.SetActive(true);
                        if (otherPlayer.GetComponent<PlayerController>().isBoy)
                        {
                            playerIcon.sprite = boyIcon;
                        }
                        else
                        {
                            playerIcon.sprite = girlIcon;
                        }
                        break;
                    }
                }
            }
        }
    }

    public void SetScreenPosition(CinemachineBrain brain)
    {
        if (otherPlayer != null && distanceText != null)
        {
            var pos = GetComponent<PlayerController>().playerUi
                .GetScreenPosition(otherPlayer.transform, playerIcon, Vector3.up * yOffset, Camera.main);
            
            // text position ändern
            distanceText.transform.parent.position = pos;
        }
    }
}
