package com.example.teamup;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.SetOptions;

import java.util.List;
import java.util.Map;
import java.util.TreeMap;

public class NotificationsAdapter  extends RecyclerView.Adapter<NotificationsAdapter.NotificationsViewHolder> {
    private static final String TAG = "NotificationsAdapter";
    private List<Notification> notifications;
    Context mContext;

    public NotificationsAdapter(Context mContext, List<Notification> notifications) {
        this.notifications = notifications;
        this.mContext = mContext;
    }

    @NonNull
    @Override
    public NotificationsViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View itemCourse = LayoutInflater.from(parent.getContext()).inflate(R.layout.notification_row, parent, false);
        NotificationsAdapter.NotificationsViewHolder notificationsViewHolder = new NotificationsAdapter.NotificationsViewHolder(itemCourse);
        notificationsViewHolder.title = itemCourse.findViewById(R.id.notificationTitle);
        notificationsViewHolder.description = itemCourse.findViewById(R.id.notificationDescription);
        notificationsViewHolder.acceptBtn = itemCourse.findViewById(R.id.notificationRowAcceptBtn);
        notificationsViewHolder.deleteBtn = itemCourse.findViewById(R.id.notificationRowDeclineBtn);

        return notificationsViewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull NotificationsViewHolder holder, final int position) {
        final Notification notification = notifications.get(position);
        holder.title.setText(notification.getTitle());
        holder.description.setText(notification.getDescription());
        if(notification.notificationSource.equals("joinTeam")) {
            holder.documentID = notification.getDocumentID();
            holder.acceptBtn.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    FirebaseFirestore db = FirebaseFirestore.getInstance();
                    Map<String, Object> userTeam = new TreeMap<>();
                    userTeam.put("accepted", true);
                    db.collection("userTeams").document(notification.getDocumentID())
                            .set(userTeam, SetOptions.merge());
                    notifications.remove(position);
                    updateView();//update array
                }
            });
            holder.deleteBtn.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    FirebaseFirestore db = FirebaseFirestore.getInstance();
                    db.collection("userTeams").document(notification.getDocumentID())
                            .delete()
                            .addOnSuccessListener(new OnSuccessListener<Void>() {
                                @Override
                                public void onSuccess(Void aVoid) {
                                    Log.d(TAG, "DocumentSnapshot successfully deleted!");
                                    notifications.remove(position);
                                    updateView();//update array
                                }
                            })
                            .addOnFailureListener(new OnFailureListener() {
                                @Override
                                public void onFailure(@NonNull Exception e) {
                                    Log.w(TAG, "Error deleting document", e);
                                }
                            });
                }
            });
        }
        if(notification.notificationSource.equals("location")){
            holder.acceptBtn.setVisibility(View.INVISIBLE);
            holder.deleteBtn.setText("X");
            holder.deleteBtn.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    FirebaseFirestore db = FirebaseFirestore.getInstance();
                    db.collection("locationNotifications").document(notification.getDocumentID())
                            .delete()
                            .addOnSuccessListener(new OnSuccessListener<Void>() {
                                @Override
                                public void onSuccess(Void aVoid) {
                                    Log.d(TAG, "DocumentSnapshot successfully deleted!");
                                    notifications.remove(position);
                                    updateView();//update array
                                }
                            })
                            .addOnFailureListener(new OnFailureListener() {
                                @Override
                                public void onFailure(@NonNull Exception e) {
                                    Log.w(TAG, "Error deleting document", e);
                                }
                            });
                }
            });
        }

    }

    private void updateView(){
        this.notifyDataSetChanged();
    }

    @Override
    public int getItemCount() {
        return notifications.size();
    }


    public static class NotificationsViewHolder extends RecyclerView.ViewHolder {
            View view;
            TextView title, description;
            Button acceptBtn;
            Button deleteBtn;
            String documentID;

            public NotificationsViewHolder(View v) {
                super(v);
                this.view = v;
            }
        }

}
