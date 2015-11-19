using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    public class PortalGenerator : RandomPlacerForRoom
    {
        int portalCount;
        public int maxPortalCount;
        bool checkStarted;
        public float minPortalDistance;
        Floor previousRoom;


        public override void Draw(TileDrawer tileDrawer = null, TilePool tilePool = null)
        {

            previousRoom = room;
            if (Vector2.Distance(room.size.center, Vector2.zero) > minPortalDistance && portalCount < maxPortalCount)
            {

                tilePool = tilePool ?? this.tilePool;
                tileDrawer = tileDrawer ?? this.tileDrawer;
                foreach (var point in Floor.Rectangle(room.size, step, height))
                {
                    var random = Random.value;
                    var xz = Mathf.Sqrt(xDensity.Evaluate(Mathf.InverseLerp(room.size.xMin, room.size.xMax, point.position.x))
                        * zDensity.Evaluate(Mathf.InverseLerp(room.size.yMin, room.size.yMax, point.position.z)));
                    if (random < xz)
                    {
                        tileDrawer.DrawTerrain(prefab: tilePool.decorative[Random.Range(0, tilePool.decorative.Length - 1)], at: point.position, overwriteIfExists: true);
                        portalCount += 1;
                        return;//up to 1 per room;
                    }
                }
            }
            if (!checkStarted)
            {
                checkStarted = true;
                //guarentees at least 1 portal if map is somehow small.
                StartCoroutine(checkRoomSpawn());
            }

        }

        private IEnumerator checkRoomSpawn()
        {
            yield return new WaitForEndOfFrame();
            if (portalCount == 0)
            {
                Vector3 target = new Vector3(previousRoom.size.center.x, height, previousRoom.size.center.y);
                tileDrawer.DrawTerrain(prefab: tilePool.decorative[Random.Range(0, tilePool.decorative.Length - 1)], at: target);
            }
            checkStarted = false; //resets so that this works over multiple worlds.
            portalCount = 0;
        }

    }
}