using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
    public class IslandDepthPlacer : Generator
    {
        public Floor room;
        public AnimationCurve xDensity;
        public AnimationCurve zDensity;
        public float maxDepth = 10f;
        public float step = 1f;
        public float height = 1;

        public override void Draw (TileDrawer tileDrawer=null, TilePool tilePool=null)
        {
            print ("wghe");
            Vector3 position;
            tilePool = tilePool ?? this.tilePool;
            tileDrawer = tileDrawer ?? this.tileDrawer;
            float random1x = Random.value / 2.0f + 0.25f;
            var random2x = Random.value;
            var random1z = Random.value / 2.0f + 0.25f;
            var random2z = Random.value;
            xDensity = new AnimationCurve (new Keyframe (0, 0), new Keyframe (room.size.x, 0));
            xDensity.AddKey (new Keyframe (random1x * room.size.x, random2x * maxDepth));
            zDensity = new AnimationCurve (new Keyframe (0, 0), new Keyframe (room.size.y, 0));
            zDensity.AddKey (new Keyframe (random1z * room.size.y, random2z * maxDepth));

            foreach (var point in Floor.Rectangle(room.size, step, height)) {         
                var depth = xDensity.Evaluate ((point.position.x - room.BoundsGrid.center.x + room.size.x / 2)) + zDensity.Evaluate ((point.position.z - room.BoundsGrid.center.z + room.size.y / 2));
                position = point.position;
                for (int i=0; i>-1*depth; i--) {
                    position.y = i;
                    tileDrawer.DrawTerrain (prefab: tilePool.defaultGround, at: position);
                }
            }
        }
    }
}