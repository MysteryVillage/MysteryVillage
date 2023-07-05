using Cinemachine;
using UnityEngine;
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
    public Camera mainCamera;
    private bool isPlayerVisible;

    private void Start()
    {
        _networkManager = FindObjectOfType<Network.NetworkManager>();
        distanceText.enabled = false;
        CinemachineCore.CameraUpdatedEvent.AddListener(SetScreenPosition);
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

            // Überprüfe, ob der zweite Spieler beigetreten ist
            if (players.Length == 2 && otherPlayer == null)
            {
                foreach (var player in players)
                {
                    if (player.gameObject != gameObject)
                    {
                        otherPlayer = player.gameObject;
                        distanceText.enabled = true;
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
            // ist der spieler im sichtfeld?
            var otherPlayerPos = otherPlayer.transform.position;
            Vector3 targetScreenPosition = mainCamera.WorldToScreenPoint(otherPlayerPos);
            isPlayerVisible = targetScreenPosition.z > 0 && targetScreenPosition.x > 0 && targetScreenPosition.x < Screen.width && targetScreenPosition.y > 0 && targetScreenPosition.y < Screen.height;
            
            // y-offset für textfeld
            Vector3 targetPosition = otherPlayerPos + new Vector3(0f, yOffset, 0f);

            // text position ändern
            distanceText.transform.position = mainCamera.WorldToScreenPoint(targetPosition);
        }
    }
}
