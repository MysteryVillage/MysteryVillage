using UnityEngine;
using Mirror;
using TMPro;

public class PlayerDistance : MonoBehaviour
{
    private Network.NetworkManager _networkManager;
    public TextMeshProUGUI distanceText;
    public float yOffset = 2f;
    public float minRange = 10f;
    private GameObject[] players;
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
            float distance = CalculatePlayersDistance();
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
                    if (player != gameObject)
                    {
                        otherPlayer = player;
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
            
            // y-offset für textfeld
            Vector3 targetPosition = otherPlayer.transform.position + new Vector3(0f, yOffset, 0f);

            // text position ändern
            distanceText.transform.position = Camera.main.WorldToScreenPoint(targetPosition);
        }
    }

    public float CalculatePlayersDistance()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 2)
        {
            Debug.LogWarning("Not enough players in the scene.");
            return 0f;
        }

        Vector3 player1Position = players[0].transform.position;
        Vector3 player2Position = players[1].transform.position;
        float distance = Vector3.Distance(player1Position, player2Position);
        return distance;
    }
}
