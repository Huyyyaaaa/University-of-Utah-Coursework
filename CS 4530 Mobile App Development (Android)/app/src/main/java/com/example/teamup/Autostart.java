package com.example.teamup;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.util.Log;

public class Autostart extends BroadcastReceiver {
    public void onReceive(Context context, Intent arg1)
    {
        Log.d("Autostart", "started");
        Intent intent = new Intent(context, MessangerService.class);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            context.startForegroundService(intent);
        } else {
            context.startService(intent);
        }

    }
}
