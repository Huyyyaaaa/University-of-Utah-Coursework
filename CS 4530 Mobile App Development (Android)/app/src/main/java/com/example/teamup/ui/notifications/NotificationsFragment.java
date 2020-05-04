package com.example.teamup.ui.notifications;

import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ProgressBar;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.teamup.NetworkManager;
import com.example.teamup.Notification;
import com.example.teamup.NotificationsAdapter;
import com.example.teamup.R;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.android.volley.VolleyLog.TAG;

public class NotificationsFragment extends Fragment {

    private NotificationsViewModel notificationsViewModel;
    private RecyclerView recyclerView;
    private RecyclerView.Adapter mAdapter;
    private RecyclerView.LayoutManager layoutManager;
    private ProgressBar progressBar;
    private NetworkManager networkManager;

    @RequiresApi(api = Build.VERSION_CODES.M)
    public View onCreateView(@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {

        View root = inflater.inflate(R.layout.fragment_notifications, container, false);

        networkManager = new NetworkManager(getContext());
        networkManager.isOnline();
        progressBar = root.findViewById(R.id.progressBarNotifications);
        getAllNotifications();
        return root;
    }
        public void getAllNotifications(){
        //First get all notifications.
        FirebaseAuth auth = FirebaseAuth.getInstance();
        FirebaseUser user = auth.getCurrentUser();
        final List<Notification> notifications = new ArrayList<>();
        String email = user.getEmail();
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        db.collection("userTeams")
                .whereEqualTo("email", email)
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                Map<String, Object> data = document.getData();
                                if(data.get("accepted").equals(new Boolean(false))) {
                                    Notification current = new Notification();
                                    current.title = "New Team Request";
                                    current.documentID = document.getId();
                                    current.description = (String) data.get("teamId");
                                    current.notificationSource = "joinTeam";
                                    notifications.add(current);
                                }
                            }

                                attachTeamNames(notifications);

                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });

    }

    public void showNoNotificationsText(){
        progressBar.setVisibility(View.GONE);
        TextView noNotifications = (TextView) getActivity().findViewById(R.id.NoNotifications);
        if(noNotifications != null) {
            noNotifications.setVisibility(View.VISIBLE);
        }
    }

    public void attachTeamNames(final List<Notification> notifications){
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        db.collection("teams")
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                Map<String, Object> data = document.getData();
                                String teamName = (String)data.get("name");
                                String documentId = document.getId();
                                for(Notification n : notifications){
                                    if(n.description.equals(documentId)){
                                        n.description = "You have received a request to join the team called " + teamName + ".";
                                    }
                                }
                            }
                            getLocationNotifications(notifications);
                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });
    }

    private void getLocationNotifications(final List<Notification> notifications){
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        db.collection("locationNotifications")
                .whereEqualTo("userId", FirebaseAuth.getInstance().getCurrentUser().getUid())
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                Map<String, Object> data = document.getData();
                                String message = (String)data.get("message");
                                Notification notification = new Notification();
                                notification.description = message;
                                notification.title = "Shared Location";
                                notification.notificationSource = "location";
                                notification.documentID = document.getId();
                                notifications.add(notification);
                            }
                            if(notifications.size() == 0) {
                                showNoNotificationsText();
                            }
                            else {
                                makeList(notifications);
                            }
                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });
    }

    public void makeList(List<Notification> notifications){
        progressBar.setVisibility(View.GONE);
        recyclerView = getView().findViewById(R.id.NotificationsRecycler);
        layoutManager = new LinearLayoutManager(getActivity());
        recyclerView.setLayoutManager(layoutManager);
        // specify an adapter
        mAdapter = new NotificationsAdapter(getActivity(), notifications);
        recyclerView.setAdapter(mAdapter);
    }
}
