using UnityEngine;
using Mirror;
using Player;
using TMPro;

public class PlayerDistance : MonoBehaviour
{
    private Network.NetworkManager _networkManager;
    public TextMeshProUGUI distanceText;
    public float yOffset = 2f;
    public float minRange = 10f;
    private PlayerController[] players;
    private GameObject otherPlayer;
    private Camera mainCamera;
    private bool isPlayerVisible;

    private void Start()
    {
        _networkManager = FindObjectOfType<Network.NetworkManager>();
        mainCamera = Camera.main;
        distanceText.enabled = false;
    }

    private void Update()
    {
        if (_networkManager != null)
        {
            // Berechne die Distanz zwischen den Spielern
            float distance = PlayerController.GetPlayerSeperation();
            players = PlayerController.GetPlayers();
            distanceText.text = "" + Mathf.FloorToInt(distance)+ "m";
            if (distance < minRange || !isPlayerVisible)
            {
                distanceText.enabled = false;
            }
            else
            {
                distanceText.enabled = true;
            }
            Debug.Log("Distance between players: " + distance);

            // Überprüfe, ob der zweite Spieler beigetreten ist
            if (players.Length == 2 && otherPlayer == null)
            {
                foreach (var player in players)
                {
                    if (player.gameObject != gameObject)
                    {
                        otherPlayer = player.gameObject;
                        Debug.Log("Der zweite Spieler ist beigetrete");
                        distanceText.enabled = true;
                        break;
                    }
                }
            }
        }

        if (otherPlayer != null && distanceText != null)
        {
            // ist der spieler im sichtfeld?
            Vector3 targetScreenPosition = mainCamera.WorldToScreenPoint(otherPlayer.transform.position);
            isPlayerVisible = targetScreenPosition.z > 0 && targetScreenPosition.x > 0 && targetScreenPosition.x < Screen.width && targetScreenPosition.y > 0 && targetScreenPosition.y < Screen.height;
            
            // y-offset f�r textfeld
            Vector3 targetPosition = otherPlayer.transform.position + new Vector3(0f, yOffset, 0f);

            // text position �ndern
            distanceText.transform.position = Camera.main.WorldToScreenPoint(targetPosition);
        }
    }
}
