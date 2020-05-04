package com.example.teamup;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.DocumentReference;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;
import java.util.TreeSet;

import static com.android.volley.VolleyLog.TAG;

public class CreateTeamActivity extends AppCompatActivity {
    final Map<String, List<String>> studentsInCourse = new TreeMap<String, List<String>>();
    final Set<Integer> invites = new TreeSet<Integer>();
    final List<String> inviteIDs = new ArrayList<>();
    final List<String> colleagueEmails = new ArrayList<>();
    private NetworkManager networkManager;

    @Override
    public void onCreate(Bundle bundle){
        super.onCreate(bundle);
        networkManager = new NetworkManager(getApplicationContext());
        setContentView(R.layout.activity_create_team);
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        FirebaseAuth mAuth = FirebaseAuth.getInstance();
        FirebaseUser user = mAuth.getCurrentUser();
        final String userID = user.getUid();
        final ArrayList<String> courses = new ArrayList<String>();

        //Get the classes from the db. These will be used for the drop down menu when the user types in a class.
        db.collection("courses")
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                Map<String, Object> data = document.getData();
                                List<String> users = (List<String>)data.get("users");
                                if(users.contains(userID)){
                                    courses.add((String)data.get("name"));
                                    users.remove(userID);
                                    studentsInCourse.put((String)data.get("name"), users);
                                }
                            }
                            //Do a second for loop to make sure we get all of the users in the same class(even if it's at a different time)
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                Map<String, Object> data = document.getData();
                                String className = (String)data.get("name");
                                List<String> users = (List<String>)data.get("users");
                                if(courses.contains(className)){
                                    users.remove(userID);
                                    Set<String> unionClassUsers = new TreeSet<>();
                                    unionClassUsers.addAll(users);
                                    unionClassUsers.addAll(studentsInCourse.get(className));
                                    List<String> allClassUsersList = new ArrayList<>();
                                    allClassUsersList.addAll(unionClassUsers);
                                    studentsInCourse.remove(className); //Clear out the list that was previously here so we can add larger list.
                                    studentsInCourse.put(className, allClassUsersList);
                                }
                            }
                            String[] temp = new String[courses.size()];
                            addAdapter(courses.toArray(temp));
                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });
    }

    //This function attaches the drop down suggestion list to the class text box, making it easier for the user to search for users in a class.
    public void addAdapter(String[] classes){
        AutoCompleteTextView classTextBox = findViewById(R.id.edit_class_name);
        classTextBox.setThreshold(0);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_dropdown_item_1line, classes);
        classTextBox.setAdapter(adapter);
    }

    //This is a handler for when the user clicks the button that says "Find teammates".
    @RequiresApi(api = Build.VERSION_CODES.M)
    public void doSearch(View button){
        if(!networkManager.isOnline()){
            return;
        }
        AutoCompleteTextView classTextBox = findViewById(R.id.edit_class_name);
        String course = classTextBox.getText().toString();
        final ListView listView = findViewById(R.id.people_list);
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        final List<String> colleagueIDs = studentsInCourse.get(course);
        inviteIDs.addAll(colleagueIDs); //This is used in onCreateTeamButtonClick and so it must be a class variable.
        final List<String> colleagueNames = new ArrayList<String>();
        for(int i = 0; i < colleagueIDs.size(); i++){ //Initialize the Lists.
            colleagueNames.add(null);
            colleagueEmails.add(null);
        }
            db.collection("users")
                    .get()
                    .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                        @Override
                        public void onComplete(@NonNull Task<QuerySnapshot> task) {
                            if (task.isSuccessful()) {
                                for (QueryDocumentSnapshot document : task.getResult()) {
                                    Map<String, Object> data = document.getData();
                                    String firstName = (String)data.get("firstName");
                                    String lastName = (String)data.get("lastName");
                                    String email = (String)data.get("email");
                                    String currentUserID = document.getId();
                                    if (colleagueIDs.contains(currentUserID)) {
                                        int idx = colleagueIDs.indexOf(currentUserID);
                                        colleagueNames.set(idx, lastName + ", " + firstName );
                                        colleagueEmails.set(idx, email);
                                    }
                                }
                                ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(getApplicationContext(),
                                        android.R.layout.simple_list_item_1,
                                        colleagueNames);
                                listView.setAdapter(arrayAdapter);
                                listView.setChoiceMode(ListView.CHOICE_MODE_MULTIPLE);
                                startListeningForItemClicks();
                            } else {
                                Log.w(TAG, "Error getting documents.", task.getException());
                            }
                        }
                    });
    }

    //This function gets called whenever the user clicks on a box in the search results list.
    public void startListeningForItemClicks(){

        final ListView listView = findViewById(R.id.people_list);
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                if(invites.contains(position)){
                    invites.remove(position);
                    view.setBackgroundColor(getResources().getColor(R.color.androidLightGray));
                }
                else{
                    invites.add(position);
                    view.setBackgroundColor(getResources().getColor(R.color.andriodDarkerGray));
                }
            }
        });
    }

    public void onCreateTeamButtonClick(View view){
        EditText teamNameTextBox = (EditText) findViewById(R.id.edit_team_name);
        String teamNameTextBoxContents = teamNameTextBox.getText().toString();
        if(teamNameTextBoxContents.equals("")){
            TextView alert = findViewById(R.id.at_least_one_invite_text_view);
            alert.setText("Please give this team a name.");
            alert.setVisibility(View.VISIBLE);
            return;
        }
        if(invites.size() == 0){
            TextView alert = findViewById(R.id.at_least_one_invite_text_view);
            alert.setText("You must invite at least one person to be on this team.");
            alert.setVisibility(View.VISIBLE);
        } else{
            //Make team in Firebase.
            EditText classNameView = findViewById(R.id.edit_team_name);
            final String teamName = classNameView.getText().toString();
            Map<String, Object> data = new HashMap<>();
            data.put("name", teamName);
            final FirebaseFirestore db = FirebaseFirestore.getInstance();
            db.collection("teams").add(data).addOnSuccessListener(new OnSuccessListener<DocumentReference>() {
                @Override
                public void onSuccess(DocumentReference documentReference) {
                    Log.d(TAG, "DocumentSnapshot written with ID: " + documentReference.getId());
                    //Toast.makeText(getApplicationContext(), "Successfully added document with id: " + documentReference.getId(), Toast.LENGTH_SHORT).show();
                    TextView alert = findViewById(R.id.at_least_one_invite_text_view);
                    alert.setVisibility(View.INVISIBLE);
                    getDocumentIDFromTeamName(teamName, db);
                }
            })
                    .addOnFailureListener(new OnFailureListener() {
                        @Override
                        public void onFailure(@NonNull Exception e) {
                            Log.w(TAG, "Error adding document", e);
                            Toast.makeText(getApplicationContext(), "Failed to create team " + e.toString(), Toast.LENGTH_SHORT).show();

                        }
                    });;

        }
    }

    //invites holds indeces for userIDs in the inviteIDs list. We need to send an invite to everyone represented in invites.
    public void sendInvites(Set<Integer> invites, List<String> inviteIDs){
        //Now return to dashboard.
        Intent intent = new Intent(this, Dashboard.class);
        startActivity(intent);
    }

    public void getDocumentIDFromTeamName(String teamName, FirebaseFirestore db){
        db.collection("teams")
                .whereEqualTo("name", teamName)
                .get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if (task.isSuccessful()) {
                            for (QueryDocumentSnapshot document : task.getResult()) {
                                addInviteesToTeam(document.getId());
                            }
                        } else {
                            Log.w(TAG, "Error getting documents.", task.getException());
                        }
                    }
                });
    }

    public void addInviteesToTeam(String documentID){
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        FirebaseAuth mAuth = FirebaseAuth.getInstance();
        FirebaseUser user = mAuth.getCurrentUser();
        final String userEmail = user.getEmail();
        //Send invites to potential team members. Tentatively add them to the team.
        List<Integer> invitesList = new ArrayList<>();
        invites.add(colleagueEmails.size()); //This makes sure the user gets added to the team they created.
        colleagueEmails.add(userEmail);
        for(int current : invites){
            invitesList.add(current);
        }
        addToTeam(invitesList, documentID);
    }

    public void addToTeam(final List<Integer> invitesList, final String teamID){
        FirebaseAuth mAuth = FirebaseAuth.getInstance();
        FirebaseUser user = mAuth.getCurrentUser();
        final String userEmail = user.getEmail();
        FirebaseFirestore db = FirebaseFirestore.getInstance();
        Map<String, Object> data = new HashMap<>();
        data.put("email", colleagueEmails.get(invitesList.get(0)));
        data.put("teamId", teamID);
        //Check if this is the logged in user. If so they accept new team invite by default.
        if(colleagueEmails.get(invitesList.get(0)).equals(userEmail)){
            data.put("accepted", true);
        } else{
            data.put("accepted", false);
        }
        db.collection("userTeams").add(data).addOnCompleteListener(new OnCompleteListener<DocumentReference>() {
            @Override
            public void onComplete(@NonNull Task<DocumentReference> task) {
                if(invitesList.size() != 1){
                    addToTeam(invitesList.subList(1, invitesList.size()), teamID);
                } else{
                    sendInvites(invites, inviteIDs);
                }
            }
        });
    }
}
