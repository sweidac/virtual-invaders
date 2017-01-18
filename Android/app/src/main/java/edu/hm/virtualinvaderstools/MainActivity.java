package edu.hm.virtualinvaderstools;

import android.app.Activity;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.widget.TextView;

import edu.hm.kopfhoererbib.KopfhoererEingestecktReceiver;
import edu.hm.kopfhoererbib.XConsumer;

public class MainActivity extends Activity {

    TextView textView;

    KopfhoererEingestecktReceiver receiver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        initComponents();

        getApplicationContext().registerReceiver(receiver, new IntentFilter(Intent.ACTION_HEADSET_PLUG));

        if (receiver.istKopfhoererEingesteckt()) {
            textView.setText("initial eingesteckt");
        } else {
            textView.setText("Nicht");
        }
    }

    private void initComponents() {
        textView = (TextView) findViewById(R.id.fucker);
        receiver = new KopfhoererEingestecktReceiver(getApplicationContext(), new XConsumer<Boolean>() {
            @Override
            public void accept(Boolean eingesteckt) {
                update(eingesteckt);
            }
        });
    }

    private void update(Boolean eingesteckt) {
        textView.append(eingesteckt ? " Eingesteckt" : " Herausgezogen");
    }


}
