using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Clouds
{
	public class CloudSpawner : MonoBehaviour
	{
		public int count;
		public Rect xzRegion;
		public GameObject[] cloudPrefabs;
		// Use this for initialization
		void Start ()
		{
			// Subdivide region into `count` cells, and randomly place clouds
			// into cells such that two clouds don't occupy the same cell (Gaussian noise)
			var sides = Mathf.Ceil (Mathf.Sqrt (count));
			var cloudCells = new List<Rect> ((int)(sides * sides));
			var width = xzRegion.width / sides;
			var height = xzRegion.height / sides;

			for (var x = 0; x < sides; x++) {
				for (var y = 0; y < sides; y++) {
					cloudCells.Add (new Rect (xzRegion.xMin + x * width, xzRegion.yMin + y * height, width, height));
				}
			}

			var cloudIndex = 0;
			for (var i = 0; i < count; i++) {
				var randomIndex = Random.Range (0, cloudCells.Count - 1);
				var cell = cloudCells [randomIndex];
				cloudCells.RemoveAt (randomIndex);
				var randomCloud = cloudPrefabs [cloudIndex];
				cloudIndex = (cloudIndex + 1) % cloudPrefabs.Length;
				var cloud = Instantiate (randomCloud, new Vector3 (Random.Range (cell.xMin, cell.xMax), 0, Random.Range (cell.yMin, cell.yMax)) + randomCloud.transform.position, randomCloud.transform.rotation) as GameObject;
				cloud.transform.SetParent (transform, false);
			}
		}
	}
}