namespace Assets.Scripts {
  class KopfhoererDetektorFabrik {

    /**
    * Gibt eine plattformabhängige Instanz eines Kopfhoererdetektors zurück.
    */
    public static IKopfhoererDetektor fabriziere() {
      #if (UNITY_EDITOR || UNITY_STANDALONE)
			return new MockKopfhoererDetektor();
			#elif UNITY_ANDROID
			return new AndroidKopfhoererDetektor ();
			#elif UNITY_IPHONE
			return new MockKopfhoererDetektor();
			#else
			return new MockKopfhoererDetektor();
			#endif
    }
  }


}
