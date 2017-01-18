package edu.hm.kopfhoererbib;

import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.media.AudioManager;
import android.media.MediaRouter;
import android.util.Log;

public class KopfhoererEingestecktReceiver extends BroadcastReceiver {

    private static KopfhoererEingestecktReceiver instanz;

    private final AudioManager audioManager;
    private final XConsumer<Boolean> action;

    private static boolean kopfhoererEingesteckt;

    public KopfhoererEingestecktReceiver(Context context, XConsumer<Boolean> onReceive) {
        audioManager = (AudioManager) context.getSystemService(Service.AUDIO_SERVICE);
        kopfhoererEingesteckt = checkIstKopfhoererEingesteckt();
        action = onReceive;
    }

    public boolean checkIstKopfhoererEingesteckt() {
        return !audioManager.isSpeakerphoneOn()
                && (audioManager.isBluetoothA2dpOn()
                || audioManager.isBluetoothScoOn()
                || audioManager.isWiredHeadsetOn());
    }

    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals(Intent.ACTION_HEADSET_PLUG)) {
            // status 0 => kein Kopfhoerer
            // status 1 => Kopfhoerer eingesteckt
            int status = intent.getIntExtra("state", 0);

            kopfhoererEingesteckt = status == 1;
            Log.i("KopfhoererReceiver","Kopfhoererstatus hat sich geaendert!");

            if (kopfhoererEingesteckt != istKopfhoererEingesteckt()) {
                // nur wenn BuildConfig.DEBUG
                Log.i("KopfhoererReceiver","Intent ACTION_HEADSET_PLUG liefert andere Daten als AudioManager.");
                throw new AssertionError("Intent ACTION_HEADSET_PLUG liefert andere Daten als AudioManager.");
            }

            if (this.action != null) {
                this.action.accept(kopfhoererEingesteckt);
            }
        }
    }

    public static boolean istKopfhoererEingesteckt() {
        return kopfhoererEingesteckt;
    }

    public static KopfhoererEingestecktReceiver instanzHolen () {
        return instanzHolen(null);
    }

    public static KopfhoererEingestecktReceiver instanzHolen (Context ctx) {
        if (instanz == null) {
            if (ctx == null) {
                throw new AssertionError("Application context must be supplied on initial call!");
            }
            instanz = new KopfhoererEingestecktReceiver(ctx, null);
        }

        return instanz;
    }
}
