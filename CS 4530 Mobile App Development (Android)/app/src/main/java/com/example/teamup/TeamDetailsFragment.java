package com.example.teamup;

import android.Manifest;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.location.Location;
import android.os.Build;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.DocumentReference;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;


import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

import static com.android.volley.VolleyLog.TAG;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link TeamDetailsFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class TeamDetailsFragment extends Fragment {

    private String teamId, teamName;
    private FirebaseFirestore db;
    private List<User> members = new ArrayList<>();
    private List<String> teamEmails = new ArrayList<>();
    private TextView teamNameText;
    private LinearLayout teamLayout;
    private ProgressBar progressBar;
    private String userFirstName;
    private String userLastName;
    private NetworkManager networkManager;

    public TeamDetailsFragment() {
        // Required empty public constructor
    }


    public static TeamDetailsFragment newInstance(String param1, String param2) {
        TeamDetailsFragment fragment = new TeamDetailsFragment();
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if(savedInstanceState != null){
            teamId = savedInstanceState.getString("TEAM_ID");
        }

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_team_details, container, false);
        teamNameText = view.findViewById(R.id.textViewDetailsTeamName);
        teamLayout = view.findViewById(R.id.TeamDetailsLinearLayout);
        progressBar = view.findViewById(R.id.progressBarTeamDetails);
        progressBar.setVisibility(View.GONE);

        teamId = getArguments().getString("TEAM_ID");

        db = FirebaseFirestore.getInstance();
        networkManager = new NetworkManager(getContext());
        getTeamDetails();
        return view;
    }

    private void getTeamDetails() {
        progressBar.setVisibility(View.VISIBLE);
        db.collection("teams").document(teamId)
                .get().addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
            @Override
            public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                if(task.isSuccessful()){
                    teamName = task.getResult().get("name").toString();
                    getMembers();
                }else{
                    Toast.makeText(getActivity(), "Unable to get Team Details", Toast.LENGTH_SHORT).show();
                }
            }
        });
    }

    private void getMembers(){
        db.collection("userTeams").whereEqualTo("teamId", teamId)
                .get().addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
            @Override
            public void onComplete(@NonNull Task<QuerySnapshot> task) {
                if(task.isSuccessful()){
                    if(!task.getResult().isEmpty()){
                        for(QueryDocumentSnapshot snapshot: task.getResult()){
                            teamEmails.add(snapshot.get("email").toString());
                        }
                        getTeamMembersNames();
                    }
                }else{
                    Toast.makeText(getActivity(), "Unable to get Team Details", Toast.LENGTH_SHORT).show();
                }
            }
        });

    }

    private void getTeamMembersNames() {
        db.collection("users")
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if(task.isSuccessful()){
                            if(!task.getResult().isEmpty()){
                                for(QueryDocumentSnapshot snapshot : task.getResult()){
                                    if(teamEmails.contains(snapshot.get("email").toString())) {
                                        members.add(snapshot.toObject(User.class));
                                    }
                                }
                                putTeamInfoOnUI();
                            }
                        }else{
                            Toast.makeText(getActivity(), "Unable to get Team Details", Toast.LENGTH_SHORT).show();
                        }
                    }
                });
    }

    private void putTeamInfoOnUI() {
        progressBar.setVisibility(View.GONE);
        teamNameText.setText(teamName);
        for(User user : members){
            View mem = LayoutInflater.from(getContext()).inflate(R.layout.user_detail_row, null, false);
            ViewGroup v = (ViewGroup) mem;
            LinearLayout row = ((LinearLayout)v.getChildAt(0));
            TextView name = ((TextView)row.getChildAt(0));
            TextView email = ((TextView)row.getChildAt(1));
            name.setText(user.getFirstName() + " " +user.getLastName());
            email.setText(user.getEmail());

            if(user.email.equals(FirebaseAuth.getInstance().getCurrentUser().getEmail())){
                userFirstName = user.getFirstName();
                userLastName = user.getLastName();
            }

            teamLayout.addView(v);
        }

        //Share location button
        Button shareLocationBtn = new Button(getActivity());
        shareLocationBtn.setText("Notify teammates of your location.");
        shareLocationBtn.setBackgroundColor(Color.rgb(0xff, 0x91, 0x4d)); //Orange
        shareLocationBtn.setTextColor(Color.WHITE);
        teamLayout.addView(shareLocationBtn);
        shareLocationBtn.setOnClickListener(new View.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.M)
            @Override
            public void onClick(View v) {
                shareLocation();
            }
        });

        //leave team button
        Button leaveBtn = new Button(getActivity());
        leaveBtn.setText("Leave Team");
        leaveBtn.setBackgroundColor(Color.RED);
        leaveBtn.setTextColor(Color.WHITE);
        teamLayout.addView(leaveBtn);
        leaveBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                leaveTeam();
            }
        });
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    private void shareLocation(){
        if(!networkManager.isOnline()){
            return;
        }
        if(hasGPSPermission()) {
            GPSTracker tracker = new GPSTracker(getContext());
            Location location = tracker.getLocation();
            if (location != null) {
                double latitute = location.getLatitude();
                double longitude = location.getLongitude();
                FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
                List<String> rest = teamEmails.subList(0, teamEmails.size());
                rest.remove(user.getEmail());
                getTeamMemberUIds(latitute, longitude, rest);
                //tellTeamLocation(latitute, longitude, rest);
            }
        }
    }

    private boolean hasGPSPermission() {
        if(ContextCompat.checkSelfPermission(getActivity(), Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED){
            ActivityCompat.requestPermissions(getActivity(),
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 1);
            return false;
        }
        return  true;
    }

    private void getTeamMemberUIds(final double latitude, final double longitude, List<String> emails){
        final List<String> teamMemberIds = new ArrayList<String>();
        final CollectionReference usersRef = db.collection("users");
        usersRef.whereIn("email", emails).get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if(task.isSuccessful()){
                            for(QueryDocumentSnapshot snapshot : task.getResult()){
                                teamMemberIds.add(snapshot.getId());
                            }
                            tellTeamLocation(latitude, longitude, teamMemberIds);
                        }
                    }
                });
    }

    private void tellTeamLocation(final double latitude, final double longitude, final List<String> invitesLeftToSend){
        if(invitesLeftToSend.size() != 0) {
            FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
            Map<String, Object> data = new TreeMap<>();
            data.put("userId", invitesLeftToSend.get(0));
            data.put("message", userFirstName + " " + userLastName + " from " + teamName + " shared their location: " + latitude + ", " + longitude + ".");
            db.collection("locationNotifications").add(data).addOnSuccessListener(new OnSuccessListener<DocumentReference>() {
                @Override
                public void onSuccess(DocumentReference documentReference) {
                    tellTeamLocation(latitude, longitude, invitesLeftToSend.subList(1, invitesLeftToSend.size()));
                }
            })
                    .addOnFailureListener(new OnFailureListener() {
                        @Override
                        public void onFailure(@NonNull Exception e) {
                            Log.w(TAG, "Error adding document", e);
                            Toast.makeText(getContext(), "Failed to send notifications to team " + e.toString(), Toast.LENGTH_SHORT).show();

                        }
                    });
            ;
        } else{
            Toast.makeText(getContext(), "Location sent!", Toast.LENGTH_LONG).show();
        }
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    private void leaveTeam(){
        if (!networkManager.isOnline()) {
            return;
        }
        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
        if(user != null){
            String userEmail = user.getEmail();
            final CollectionReference utRef = db.collection("userTeams");
            utRef.whereEqualTo("teamId", teamId)
                    .whereEqualTo("email",  userEmail).get()
                    .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                        @Override
                        public void onComplete(@NonNull Task<QuerySnapshot> task) {
                            if(task.isSuccessful()){
                                String docId = "";
                                for(QueryDocumentSnapshot snapshot : task.getResult()){
                                    docId = snapshot.getId();
                                }
                                utRef.document(docId).delete()
                                        .addOnSuccessListener(new OnSuccessListener<Void>() {
                                            @Override
                                            public void onSuccess(Void aVoid) {
                                                leftTeamSuccess();
                                            }
                                        })
                                        .addOnFailureListener(new OnFailureListener() {
                                            @Override
                                            public void onFailure(@NonNull Exception e) {
                                                Toast.makeText(getContext(), "Unable to leave team", Toast.LENGTH_SHORT).show();
                                            }
                                        });
                            }
                        }
                    });
        }

    }

    private void leftTeamSuccess() {
        NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
        navController.popBackStack();
        Toast.makeText(getActivity(), "Successfully left team!", Toast.LENGTH_SHORT).show();
    }


}
