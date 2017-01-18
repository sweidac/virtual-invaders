using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers;
using UnityEngine.Audio;

namespace Assets.Scripts
{
	public class AudioSteuerung : MonoBehaviour
	{

		private static int KOPFHOERER_AKT_FRAME_SKIP = 60;
		// each second

		private IKopfhoererDetektor kopfhoererDetektor;
		private bool kopfhoererVerbunden;

		private MusicSteuerung musikSteuerung;

		#region UNITY PARAMETER

		// sounds für gemischte sachen (button klick ...)
		public AudioSource infoAudioSource;
		public AudioClip kopfhoererEingestecktSound;
		public AudioClip kopfhoererAusgestecktSound;

		public AudioSource laserAudioSource;

		// Sound, wenn Spieler getroffen wird
		public AudioSource spielerTreffer;

		// kopfhoerer snapshots
		public AudioMixerSnapshot viaKopfhoerer;
		public AudioMixerSnapshot viaLautsprecher;

		// menue / ingame snapshots
		public AudioMixerSnapshot inSpielMaster;
		public AudioMixerSnapshot inMenueMaster;

		// laser on hit Effekt
		public AudioMixerSnapshot gegnerLaserTreffer;
		public AudioMixerSnapshot keinLaserGegenerTreffer;

		// game over
		public AudioClip spielZuendeSound;

		#endregion

		private static AudioSteuerung _instance;

		private void Awake ()
		{
			_instance = this;
		}

		public static AudioSteuerung Instance {
			get {
				Debug.Assert (_instance != null, "Es gibt keine AudioSteuerung-Instanz in der Szene! Bitte eine hinzufügen.");
				return _instance;
			}
		}

		/**
	    * Init.
	    */
		void Start ()
		{
			Application.SetStackTraceLogType (LogType.Log, StackTraceLogType.None); // entfernt Stacktraces aus adb logcat Ausgabe

			kopfhoererDetektor = KopfhoererDetektorFabrik.fabriziere ();
			kopfhoererVerbunden = kopfhoererDetektor.istVerbunden ();

			Debug.Log (System.String.Format ("AudioSteuerung wurde Initialisiert. Kopfhoerer Detektor modul: {0}", kopfhoererDetektor.name ()));

			musikSteuerung = MusicSteuerung.Instance;
		}

		/**
		* Update.
		*/
		void Update ()
		{
			// gedrosselte Aufrufe auf den kopfhoererDetektor.
			if (kopfhoererDetektor != null && Time.frameCount % KOPFHOERER_AKT_FRAME_SKIP == 0) {
				UpdateKopfhoerer ();
			}
		}


		public void starteSpielerTrefferSound ()
		{
			this.spielerTreffer.Play ();
		}

		#region Laser Sounds

		public void stopLaserSound ()
		{
			laserAudioSource.volume = 0.0f;
		}

		public void startLaserSound ()
		{
			laserAudioSource.volume = 1.0f;
		}


		public void laserTrefferModus (bool hit)
		{
			if (hit) {
				this.gegnerLaserTreffer.TransitionTo (0.1f);
			} else {
				this.keinLaserGegenerTreffer.TransitionTo (0.3f);
			}
		}

		#endregion

		#region Menuemusik

		public void starteSpiel ()
		{
			Debug.Log ("[AudioSteuerung] starte Spiel");
			inSpielMaster.TransitionTo (1); // aktiviere Spiel Soundeinstellungen (laser, sfx aus etc)
			musikSteuerung.StarteSpiel();
			startLaserSound ();
		}

		public void beendeSpiel ()
		{
			Debug.Log ("[AudioSteuerung] beende Spiel");
			inMenueMaster.TransitionTo (1); // aktiviere Menue Soundeinstellungen (sfx an)
			musikSteuerung.BeendeSpiel();
			stopLaserSound ();

			if (spielZuendeSound != null) {
				infoAudioSource.clip = spielZuendeSound;
				infoAudioSource.Play ();
			}
		}

		#endregion

		#region Kopfhoerer Detektion

		void UpdateKopfhoerer ()
		{
			// Bitte nicht die Konsole vollspammen!
			//Debug.Log ("Kopfhoerer status wird aktualisiert...");
			bool gegenwaertigKopfhoererVerbunden = kopfhoererDetektor.istVerbunden ();

			if (kopfhoererVerbunden == gegenwaertigKopfhoererVerbunden) { // status unveraendert, tue nichts
				//Debug.Log ("\t...keine Aenderung");
				return;
			}

			Debug.Log (System.String.Format ("Update Kopfhoerer, war eingesteckt: {0}, ist nun eingesteckt {1}", kopfhoererVerbunden, gegenwaertigKopfhoererVerbunden));

			if (gegenwaertigKopfhoererVerbunden) {
				aktionKopfhoererEingesteckt ();
			} else {
				aktionKopfhoererAusgesteckt ();
			}
			kopfhoererVerbunden = gegenwaertigKopfhoererVerbunden;
		}

		private void aktionKopfhoererEingesteckt ()
		{
			viaKopfhoerer.TransitionTo (0.1f); // optimiere Audioeinstellungen fuer Kopfhoerer

			if (Debug.isDebugBuild) {
				infoAudioSource.clip = kopfhoererEingestecktSound;
				infoAudioSource.Play ();
				Debug.Log ("Kopfhoerer wurde eingesteckt -> Sound " + kopfhoererEingestecktSound + " abgespielt");
			}
		}

		private void aktionKopfhoererAusgesteckt ()
		{
			viaLautsprecher.TransitionTo (0.1f); // optimiere Audioeinstellungen fuer schlechte Lautsprecher

			if (Debug.isDebugBuild) {
				infoAudioSource.clip = kopfhoererAusgestecktSound;
				infoAudioSource.Play ();
				Debug.Log ("Kopfhoerer wurde herausgezogen -> Sound " + kopfhoererAusgestecktSound + " abgespielt");
			}
		}
		#endregion
	}
}
