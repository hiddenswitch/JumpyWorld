using UnityEngine;
using System.Collections;


namespace JumpyWorld { 
    [RequireComponent (typeof(GridLocateable))]
    [RequireComponent(typeof(ICondition))]
    public class ConditionalBuildForwardPlatform : MonoBehaviour {
        public ICondition condition;
        public TileDrawer tileDrawer;
        public TilePool tilePool;
        public float platformHeight;
        public Collects woodCollector;

        GridLocateable gridLocatable;

        void Start()
        {
            gridLocatable = GetComponent<GridLocateable>();
        }

        void Update()
        {
            if (condition.evaluate()) {
                Vector3 pos = gridLocatable.PositionGrid;
                pos = new Vector3(pos.x, platformHeight, pos.z);
                if (!tileDrawer.Contains(pos))
                {
                    tileDrawer.DrawTerrain(prefab: tilePool.defaultGround,at: pos);
                    woodCollector.decrement(1);
                }
                
            }
        }


        
    }
}
