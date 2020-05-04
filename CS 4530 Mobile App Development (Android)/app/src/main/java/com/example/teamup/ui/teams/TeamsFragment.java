package com.example.teamup.ui.teams;

import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;

import com.example.teamup.Dashboard;
import com.example.teamup.NetworkManager;
import com.example.teamup.R;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

import static com.android.volley.VolleyLog.TAG;

public class TeamsFragment extends Fragment {

    private TeamsViewModel teamsViewModel;
    private LinearLayout layout;
    private FirebaseAuth mAuth;
    private ArrayList teamIDs;
    private LinearLayout subLayout;
    private ProgressBar progressBar;
    private int childCount = 0;
    private NetworkManager networkManager;

    public View onCreateView(@NonNull LayoutInflater inflater,
            ViewGroup container, Bundle savedInstanceState) {
        teamsViewModel =
                ViewModelProviders.of(this).get(TeamsViewModel.class);
        final View root = inflater.inflate(R.layout.fragment_teams, container, false);
        networkManager = new NetworkManager(getContext());

        progressBar = root.findViewById(R.id.progressBarTeams);

        //The following lines of codes have the purpoese of Finding the logged in user's teams from the database.
        final List<Team> AllTeams = new ArrayList<>();
        final List<UserTeam> AllUserTeamRelationships = new ArrayList<>();
        mAuth = FirebaseAuth.getInstance();
        FirebaseUser user = mAuth.getCurrentUser();
        final String UserID = user.getEmail();
        final FirebaseFirestore db = FirebaseFirestore.getInstance();
        //This outermost query gets all teams from the database.
        db.collection("teams")
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            teamIDs = new ArrayList(task.getResult().size());
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                Map<String, Object> data = document.getData();
                                Team team = new Team();
                                team.name = (String) data.get("name");
                                team.teamId = document.getId();
                                AllTeams.add(team);
                            }
                            //This inner query gets all of the user/team relationships from the database.
                            GetAllUserTeamRelationships(AllUserTeamRelationships, AllTeams, UserID, root, db);
                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });


        return root;
    }

    public void GetAllUserTeamRelationships(final List<UserTeam> AllUserTeamRelationships,
                                            final List<Team> AllTeams, final String UserID,
                                            final View root, FirebaseFirestore db){
        db.collection("userTeams")
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                UserTeam Current = document.toObject(UserTeam.class);
                                Map<String, Object> data = document.getData();
                                boolean accepted = (Boolean) data.get("accepted");
                                if(accepted == true){ //Only show teams where invite has been accepted.
                                    AllUserTeamRelationships.add(Current);
                                }
                            }
                                MakeButtons(AllUserTeamRelationships, AllTeams, UserID, root);
                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });
    }

    public void showNoTeams(){
        HideProgressBar();
        getActivity().findViewById(R.id.noTeams).setVisibility(View.VISIBLE);
    }

    public void MakeButtons(List<UserTeam> UserTeams, List<Team> Teams, String UserID, View root){

        List<Team> TeamsWithUser = new ArrayList<Team>();
        int teamCount = 0;
        for(UserTeam Current : UserTeams){
            if(Current.email.equals(UserID)){
                teamCount = teamCount + 1;
                Team Team = new Team();
                Team.teamId = Current.teamId;
                Team.name = FindTeamName(Current.teamId, Teams);
                TeamsWithUser.add(Team);
            }
        }
        //Gets executed if the user has no teams.
        if(teamCount == 0){
            showNoTeams();
        }
        for(Team Current : TeamsWithUser){
            makeTeamButtonAndAddToView(Current.name, root, Current.teamId);
        }
    }

    public void makeTeamButtonAndAddToView(String Title, View root, String teamID){ //tempID refers to an index in the list of TeamIDs, which are strings.
        HideProgressBar();
        Button currentButton = new Button(getActivity());
        currentButton.setBackground(getResources().getDrawable(R.drawable.ic_team_panel_128dp));
        currentButton.setText(Title);
        currentButton.setGravity(Gravity.TOP|Gravity.CENTER);
        currentButton.setTextSize(24);
        currentButton.setLayoutParams(new LinearLayout.LayoutParams(512, 512));
        teamIDs.add(teamID);
        currentButton.setId(teamIDs.size() - 1);
        currentButton.setOnClickListener(new View.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.M)
            @Override
            public void onClick(View v) {
                if(networkManager.isOnline()) {
                    int tempID = v.getId();
                    String teamID = (String) teamIDs.get(tempID);
                    ((Dashboard) getActivity()).goToTeamDetails(teamID);
                }
            }
        });
        int action = childCount % 2;
        if(action == 0){
            subLayout = new LinearLayout(getContext());
            subLayout.setLayoutParams(new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT));
            subLayout.setOrientation(LinearLayout.HORIZONTAL);
            subLayout.addView(currentButton);
            layout = (LinearLayout) root.findViewById(R.id.teams_layout);
            layout.addView(subLayout);
            childCount = childCount + 1;
        } else{
            subLayout.addView(currentButton);
            childCount = childCount + 1;
        }

    }



    public String FindTeamName(String TeamID, List<Team> Teams){
        for(Team Current : Teams){
            if(Current.teamId.equals(TeamID)){
                return Current.name;
            }
        }
        return null;
    }

    private void HideProgressBar(){
        progressBar.setVisibility(View.GONE);
    }

}

class Team{
    public String teamId;
    public String name;
    public Team(){

    }
}

class UserTeam{
    public String email;
    public String teamId;
    public UserTeam(){

    }
}
