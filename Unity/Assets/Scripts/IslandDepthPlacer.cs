using UnityEngine;
using System.Collections;

namespace JumpyWorld
{
    public class IslandDepthPlacer : Generator
    {
        public Floor room;
        public AnimationCurve xDensity;
        public AnimationCurve zDensity;
        public float maxDepth = 1.3f;
        public float step = 1f;
        public float height = 1;
        public float disturbance= 0.85f;
        public float heightDisturbance = 1.1f;

        public override void Draw (TileDrawer tileDrawer=null, TilePool tilePool=null)
        {
    
            Vector3 position;
            tilePool = tilePool ?? this.tilePool;
            tileDrawer = tileDrawer ?? this.tileDrawer;
            float random1x = Random.value;
            var random2x = Random.value;
            var random1z = Random.value;
            var random2z = Random.value;
       
            xDensity = new AnimationCurve (new Keyframe (0, 0), new Keyframe (room.BoundsGrid.size.x, 0));
            xDensity.AddKey (new Keyframe (random1x * room.BoundsGrid.size.x, random2x*room.BoundsGrid.size.x* maxDepth));
            zDensity = new AnimationCurve (new Keyframe (0, 0), new Keyframe (room.BoundsGrid.size.z, 0));
            zDensity.AddKey (new Keyframe (random1z * room.BoundsGrid.size.z, random2z *room.BoundsGrid.size.z* maxDepth));

            foreach (var point in Floor.Rectangle(room.size, step, height)) {         
                var depth = xDensity.Evaluate ((point.position.x - room.BoundsGrid.center.x + room.BoundsGrid.size.x / 2.0f)) + zDensity.Evaluate ((point.position.z - room.BoundsGrid.center.z + room.BoundsGrid.size.z / 2.0f));
                position = point.position;
                var randomDepth= Random.value;
                if (randomDepth>disturbance){
                    depth= randomDepth*(heightDisturbance*room.BoundsGrid.size.x);
                }
                else if( randomDepth< 1-disturbance){
                    depth= randomDepth* maxDepth;
                }
               
                for (int i=0; i>-1*depth; i--) {
                    position.y = i;
                    tileDrawer.DrawTerrain (prefab: tilePool.defaultGround, at: position);
                }
            }
        }
    }
}