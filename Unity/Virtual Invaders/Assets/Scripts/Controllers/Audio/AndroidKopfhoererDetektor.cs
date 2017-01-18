using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class AndroidKopfhoererDetektor : IKopfhoererDetektor
	{

		private static string _name = "Android impl";
		private static string INTENT_KOPFHOERER_AKTUALISIEREN_METHODE = "istKopfhoererEingesteckt";// TODO funktioniert aktuell nicht
		private static string KOPFHOERER_AKTUALISIEREN_METHODE = "checkIstKopfhoererEingesteckt";
		private static string KOPFHOERER_INFO_GEBER = "edu.hm.kopfhoererbib.KopfhoererEingestecktReceiver";

		private AndroidJavaClass kopfhoererDetektor;
		private AndroidJavaObject kopfhoererDetektorInstanz;

		public bool verbunden;

		public AndroidKopfhoererDetektor ()
		{
			kopfhoererDetektor = new AndroidJavaClass (KOPFHOERER_INFO_GEBER);
			kopfhoererDetektorInstanz = kopfhoererDetektor.CallStatic<AndroidJavaObject> ("instanzHolen", holeContext ()); // init singleton

			verbunden = kopfhoererDetektorInstanz.Call<bool> (KOPFHOERER_AKTUALISIEREN_METHODE);
		}

		public bool istVerbunden ()
		{
			UpdateKopfhoererInfo ();
			return verbunden;
		}

		public string name ()
		{
			return _name;
		}

		private void UpdateKopfhoererInfo (object o = null)
		{
			Debug.Log ("In Android detektor instanz");
			Debug.Log (kopfhoererDetektor);
			Debug.Log (kopfhoererDetektorInstanz);
			if (kopfhoererDetektorInstanz != null) {
				verbunden = kopfhoererDetektorInstanz.Call<bool> (KOPFHOERER_AKTUALISIEREN_METHODE);
			} else {
				Debug.Log ("Kopfhoerer detektor falsch initialisiert. Keine Detektor instanz...");
			}
		}

		private static AndroidJavaObject holeContext ()
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			return activity.Call<AndroidJavaObject> ("getApplicationContext");
		}
	}
}
