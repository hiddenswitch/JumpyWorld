using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace JumpyWorld
{
	[ExecuteInEditMode()]
	public sealed class RecordTouches : BaseInput
	{
		static Touch[] emptyTouch = new Touch[0];
		public string hostName;
		public int port = 6001;
		TcpListener listener;
		SortedList<float, TouchRecord> currentTouchRecording = new SortedList<float, TouchRecord> (60 * 120);
		int lastLow;
		float timeOffset;

		public struct TouchRecord
		{
			public float time;
			public Touch touch0;
		}

		public override int touchCount {
			get {
				return touches.Count;
			}
		}

		public override IList<Touch> touches {
			get {
				// Save the last low point of a found touch
				var time = Time.time - Time.deltaTime - timeOffset;
				var index = FindFirstIndexGreaterThanOrEqualTo (sortedList: currentTouchRecording, key: time, low: lastLow, high: lastLow + 2);
				var record = currentTouchRecording [index];
				if (record.time < Time.time - timeOffset
					&& record.time > time) {
					lastLow = index;
					return new Touch[] {record.touch0};
				} else {
					return emptyTouch;
				}
			}
			private set {
				return;
			}
		}

		void OnApplicationQuit ()
		{
			if (listener == null) {
				return;
			}

			listener.Stop ();
		}

		// Use this for initialization
		void Start ()
		{
			listener = new System.Net.Sockets.TcpListener (System.Net.IPAddress.Any, port);
			timeOffset = Time.time;
			if (Application.isEditor) {
				// Create a listening socket
				DoBeginAcceptSocket (listener);
			}
		}
	
		public static ManualResetEvent clientConnected = 
			new ManualResetEvent (false);
		
		// Accept one client connection asynchronously.
		public static void DoBeginAcceptSocket (TcpListener listener)
		{
			// Set the event to nonsignaled state.
			clientConnected.Reset ();
			
			// Start to listen for connections from a client.
			Debug.Log ("Waiting for a connection...");
			
			// Accept the connection. 
			// BeginAcceptSocket() creates the accepted socket.
			listener.BeginAcceptSocket (
				new AsyncCallback (DoAcceptSocketCallback), listener);
			// Wait until a connection is made and processed before 
			// continuing.
			clientConnected.WaitOne ();
		}

		// Process the client connection.
		public static void DoAcceptSocketCallback (IAsyncResult ar)
		{
			// Get the listener that handles the client request.
			TcpListener listener = (TcpListener)ar.AsyncState;
			
			// End the operation and display the received data on the
			//console.
			Socket clientSocket = listener.EndAcceptSocket (ar);
			
			// Process the connection here. (Add the client to a 
			// server table, read data, etc.)
			Console.WriteLine ("Client connected completed");
			
			// Signal the calling thread to continue.
			clientConnected.Set ();
		}

		// Update is called once per frame
		void Update ()
		{
			if (!Application.isPlaying) {
				return;
			}

			if (Input.touchCount > 0) {
				currentTouchRecording.Add (Time.time, new TouchRecord () {
					time = Time.time,
					touch0 = Input.touches[0]
				});
			}
		}

		public void Reset ()
		{
			lastLow = 0;
			timeOffset = Time.time;
		}

		private static int BinarySearch<T> (IList<T> list, T value, int low=0, int high=-1)
		{
			if (high == -1) {
				high = list.Count - 1;
			}
			if (list == null)
				throw new System.ArgumentNullException ("list");
			var comp = Comparer<T>.Default;
			while (low < high) {
				int m = (high + low) / 2;  // this might overflow; be careful.
				if (comp.Compare (list [m], value) < 0)
					low = m + 1;
				else
					high = m - 1;
			}
			if (comp.Compare (list [low], value) < 0)
				low++;
			return low;
		}
		
		public static int FindFirstIndexGreaterThanOrEqualTo<T,U>
			(SortedList<T,U> sortedList, T key, int low=0, int high=-1)
		{
			return BinarySearch (sortedList.Keys, key, low: 0, high: high);
		}
	}
}