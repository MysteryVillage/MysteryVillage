using System;
using System.Linq;
using Mirror;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    [RequireComponent(typeof(Collider))]
    public class SpawnArea : NetworkBehaviour
    {
        public ItemData[] spawnableItems;

        public int maxItems = 10;
        public int spawnRate = 120;
        public int itemCount = 0;
        public float minimumPlayerDistanceToSpawn = 30;

        private float timeSinceSpawn;
        private new Collider collider;
        private PlayerController[] players;

        private void Start()
        {
            players = PlayerController.GetPlayers();
            collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (timeSinceSpawn > spawnRate && maxItems > itemCount && isServer)
            {
                SpawnItem();
            }

            timeSinceSpawn += Time.deltaTime;
        }

        private void SpawnItem()
        {
            if (spawnableItems.Length == 0) return;

            // Don't spawn items if a player is nearby
            if (GetDistanceToClosestPlayer() < minimumPlayerDistanceToSpawn) return;
            
            // Choose random item from spawnables
            var index = Random.Range(0, spawnableItems.Length);
            
            // Safety first
            if (index >= spawnableItems.Length) return;
            
            var item = spawnableItems[index];
            var itemObject = Instantiate(item.dropPrefab, GenerateSpawnPosition(), transform.rotation);
            itemObject.GetComponent<ItemObject>().spawnArea = this;
            NetworkServer.Spawn(itemObject);

            itemCount++;
            timeSinceSpawn = 0;
        }

        private Vector3 GenerateSpawnPosition()
        {
            var bounds = collider.bounds;
            var pos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
            var activeTerrain = GetClosestTerrain(pos);
            pos.y = activeTerrain.SampleHeight(pos) + activeTerrain.transform.position.y;
            return pos;
        }

        /**
         * https://stackoverflow.com/questions/52345522/unity-get-the-actual-current-terrain
         */
        private Terrain GetClosestTerrain(Vector3 position)
        {
            //Get all terrain
            Terrain[] terrains = Terrain.activeTerrains;
            
            //Make sure that terrains length is ok
            if (terrains.Length == 0)
                return null;
            
            //If just one, return that one terrain
            if (terrains.Length == 1)
                return terrains[0];

            var terrain = GetClosestInArray(terrains, position);
            
            return terrain.GetComponent<Terrain>();
        }

        private float GetDistanceToClosestPlayer()
        {
            var player = GetClosestInArray(players, transform.position);

            return (player.transform.position - transform.position).magnitude;
        }

        public static SpawnArea GetSpawnArea(Vector3 position, ItemData item)
        {
            var areas = FindObjectsOfType<SpawnArea>();

            var area = GetClosestInArray(areas, position).GetComponent<SpawnArea>();

            return area.spawnableItems.Contains(item) ? area : null;
        }

        private static GameObject GetClosestInArray(Behaviour[] list, Vector3 position)
        {
            float lowDist = (list[0].transform.position - position).sqrMagnitude;
            var index = 0;

            for (int i = 1; i < list.Length; i++)
            {
                var item = list[i];
                var itemPos = item.transform.position;

                var dist = (itemPos - position).sqrMagnitude;
                if (dist < lowDist)
                {
                    lowDist = dist;
                    index = i;
                }
            }

            return list[index].gameObject;
        }
    }
}