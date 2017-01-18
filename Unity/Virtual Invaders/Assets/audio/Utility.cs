using UnityEngine;
using System.Collections;
using UnityEngine.Internal;
using UnityEngine.Audio;

public class Utility {

	/**
	 * Obsolet.
	 */
	public static AudioSource PlayClipAt (
		Vector3 position,
		AudioClip clip,
		AudioMixerGroup mixer = null,
		AudioRolloffMode rolloffMode = AudioRolloffMode.Linear
	) {
		GameObject unsichtbaresObjekt = new GameObject ("Temporaeres Sound Objekt");
		AudioSource abspielQuelle = unsichtbaresObjekt.AddComponent <AudioSource>();

        if(mixer != null)
        {
            abspielQuelle.outputAudioMixerGroup = mixer;
        }

		abspielQuelle.spatialBlend = 1.0F;
		abspielQuelle.rolloffMode = rolloffMode;

		unsichtbaresObjekt.transform.position = position;
		abspielQuelle.clip = clip;

		// geraet nach clip vernichten
		GameObject.Destroy(unsichtbaresObjekt, clip.length);
		return abspielQuelle;
	}

	/**
	 * Erstellt ein neues GameObject mit einer AudioQuelle an einer bestimmten Position.
	 * @param position der Ort, an dem die AudioQuelle positioniert wird
	 * @param quelle eine AudioSource, deren Eigenschaften für die neue quelle uebernommen werden
	 * 					gegenwaertig werden nur die folgenden Eigenschaften uebernommen:
	 * 					Clip, Volume, RolloffMode, SpatialBlend, OutputAudioMixerGroup, Loop, Pitch
	 * @param automatischAbspielenUndZerstoeren - 
	 * @return die neu erzeugte AudioSource
	 */
	public static AudioSource PlayClipAt (
		Vector3 position,
		AudioSource quelle,
		bool automatischAbspielenUndZerstoeren = true
	) {
		// neues unsichtbares GameObject erstellen.
		GameObject unsichtbaresObjekt = new GameObject ("Temporaeres Sound Objekt");
		AudioSource abspielquelle = unsichtbaresObjekt.AddComponent <AudioSource> ();
	
		// eigenschaften kopieren
		abspielquelle.clip = quelle.clip;
		abspielquelle.volume = quelle.volume;
		abspielquelle.rolloffMode = quelle.rolloffMode;
		abspielquelle.spatialBlend = quelle.spatialBlend;
		abspielquelle.outputAudioMixerGroup = quelle.outputAudioMixerGroup;
		abspielquelle.loop = quelle.loop;
		abspielquelle.pitch = quelle.pitch;	

		if (automatischAbspielenUndZerstoeren) {
			GameObject.Destroy (unsichtbaresObjekt, abspielquelle.clip.length); // nach clip vernichten
			abspielquelle.Play();
		}

		return abspielquelle;
	}

}