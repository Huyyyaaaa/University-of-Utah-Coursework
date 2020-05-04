package com.example.teamup;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Build;
import android.os.Handler;
import android.os.IBinder;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.core.app.NotificationCompat;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.DocumentChange;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.EventListener;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.FirebaseFirestoreException;
import com.google.firebase.firestore.ListenerRegistration;
import com.google.firebase.firestore.QuerySnapshot;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Random;

public class MessangerService extends Service {
    private static final String ADMIN_CHANNEL_ID = "MessangerServiceChannelID0009123";

//    @RequiresApi(api = Build.VERSION_CODES.O)
//    private void setupChannels(){
//        CharSequence adminChannelName = getString(R.string.notifications_admin_channel_name);
//        String adminChannelDescription = getString(R.string.notifications_admin_channel_description);
//
//        NotificationChannel adminChannel;
//        adminChannel = new NotificationChannel(ADMIN_CHANNEL_ID, adminChannelName, NotificationManager.IMPORTANCE_LOW);
//        adminChannel.setDescription(adminChannelDescription);
//        adminChannel.enableLights(true);
//        adminChannel.setLightColor(Color.RED);
//        adminChannel.enableVibration(true);
//        if (notificationManager != null) {
//            notificationManager.createNotificationChannel(adminChannel);
//        }
//    }


    Handler mHandler;
    private FirebaseFirestore db;
    ListenerRegistration listener_registration;
    private static final String TAG = "MessangerService";
    String firebase_sdf_format = "yyyy-MM-dd HH:mm:ss.SSS";
    SimpleDateFormat firebase_sdf = new SimpleDateFormat(firebase_sdf_format);
    NotificationManager notificationManager;

    public MessangerService(){
        db = FirebaseFirestore.getInstance();
    }
    private String get_proper_name(String first, String last){
        if (first.length() > 1){
            first = first.substring(0, 1).toUpperCase() + first.substring(1).toLowerCase();
        }
        else{
            first = first.substring(0, 1).toUpperCase();
        }
        last = last.substring(0, 1).toUpperCase();
        return first + " " + last;
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
    @Override
    public void onDestroy() {
        //listener_registration.remove(); ONLY WAY TO KEEP IN BACKGROUND, shady I know -FW
        Log.d(TAG, "onDestroy");
    }

    private void service_fun(){
        try{
            listener_registration = db.collection("messagesTeams").addSnapshotListener(new EventListener<QuerySnapshot>() {
                @RequiresApi(api = Build.VERSION_CODES.M)
                @Override
                public void onEvent(@Nullable QuerySnapshot queryDocumentSnapshots, @Nullable FirebaseFirestoreException e) {
                    Log.d(TAG, "listen:event");
                    if (e != null) {
                        Log.e(TAG, "listen:error", e);
                        return;
                    }
                    FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
                    if (user == null){
                        return;
                    }

                    final String user_id = user.getUid();
                    // for calculating only notifying
                    Date time_thresh = Calendar.getInstance().getTime();
                    Calendar cal = Calendar.getInstance();
                    cal.setTime(time_thresh);
                    cal.add(Calendar.MINUTE, -1);
                    time_thresh = cal.getTime();
                    for (DocumentChange dc : queryDocumentSnapshots.getDocumentChanges()) {
                        final String sender = (String)dc.getDocument().get("sender");
                        if (sender.equals(user_id)) continue;
                        // Only if message sender is not the current user
                        try {
                            Date datetime = firebase_sdf.parse((String)dc.getDocument().get("datetime"));
                            if (dc.getType() == DocumentChange.Type.ADDED && datetime.after(time_thresh)){
                                final String message = (String)dc.getDocument().get("text");
                                db.collection("users").document(sender).get()
                                        .addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
                                            @Override
                                            public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                                                if (task.isSuccessful() && task.getResult().exists()) {
                                                    final String sender_name = get_proper_name((String) task.getResult().getData().get("firstName"),  (String) task.getResult().getData().get("lastName"));

                                                    Log.d(TAG, "Sending notification for message " + message + " from " + sender_name);
                                                    send_notification(sender_name + " on Teamup:",  message);

                                                }
                                                else{
                                                    Log.d(TAG, "Failure getting user");
                                                }
                                            }
                                        });
                            }

                        } catch (ParseException dateE) {
                            Log.e(TAG, "Failure parsing date " + dateE.getStackTrace());
                        }
                    }
                }
            });
        } catch (Exception gE){
            Log.e(TAG, "Error: " + gE.getStackTrace());
        }
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    private void setupChannels(){
        CharSequence adminChannelName = getString(R.string.notifications_admin_channel_name);
        String adminChannelDescription = getString(R.string.notifications_admin_channel_description);

        NotificationChannel adminChannel;
        adminChannel = new NotificationChannel(ADMIN_CHANNEL_ID, adminChannelName, NotificationManager.IMPORTANCE_HIGH);
        adminChannel.setDescription(adminChannelDescription);
        adminChannel.enableLights(true);
        adminChannel.setLightColor(Color.RED);
        adminChannel.enableVibration(true);
        if (notificationManager != null) {
            notificationManager.createNotificationChannel(adminChannel);
        }
    }

    private void send_notification(String title, String message){
        notificationManager =
                (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

        //Setting up Notification channels for android O and above
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.O) {
            setupChannels();
        }
        int notificationId = new Random().nextInt(60000);

        Uri defaultSoundUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);

        NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this, ADMIN_CHANNEL_ID)
                .setSmallIcon(R.drawable.ic_chat_black_24dp)
                .setContentTitle(title)
                .setContentText(message)
                .setAutoCancel(true)
                .setSound(defaultSoundUri);

        NotificationManager notificationManager =
                (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

        notificationManager.notify(notificationId, notificationBuilder.build());
    }

    private void scheduleNext() {
        mHandler.postDelayed(new Runnable() {
            public void run() { service_fun(); }
        }, 5000);
    }

    @Override
    public void onCreate()
    {
        //Intent intents = new Intent(getBaseContext(),hello.class);
        //intents.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
       // startActivity(intents);
        mHandler = new Handler();
        service_fun();

        //Toast.makeText(this, "My Service Started", Toast.LENGTH_LONG).show();
        Log.d(TAG, "onCreate");
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        super.onStartCommand(intent, flags, startId);
        Log.i(TAG, "Received start id " + startId + ": " + intent);
        return START_STICKY;
    }


}
