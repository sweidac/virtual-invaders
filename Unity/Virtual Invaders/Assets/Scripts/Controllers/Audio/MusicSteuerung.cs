using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts
{
	public class MusicSteuerung : MonoBehaviour
	{
		// SINGLETON BOILERPLATE
		private static MusicSteuerung _instance;

		private void Awake () {
			_instance = this;
		}

		public static MusicSteuerung Instance {
			get {
				return _instance;
			}
		}


		#region UNITY PARAMETERS
	
		public AudioMixerSnapshot inMenue;
		public AudioMixerSnapshot inSpiel;

		#endregion


		public void StarteSpiel () {
			inSpiel.TransitionTo (1);
		}

		public void BeendeSpiel () {
			inMenue.TransitionTo (1);
		}
	}
}

