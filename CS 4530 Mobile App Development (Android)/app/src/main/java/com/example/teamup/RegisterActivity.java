package com.example.teamup;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.app.DownloadManager;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.Toast;


import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;

import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.DocumentReference;
import com.google.firebase.firestore.FirebaseFirestore;

import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;


public class RegisterActivity extends AppCompatActivity {

    private static final String TAG = "RegisterActivity";
    private FirebaseAuth mAuth;
    private FirebaseFirestore db;
    private EditText firstNameText, lastNameText, passwordText, passwordConfirmText, emailText;
    Spinner schoolSpinner;
    private String schoolId;
    private ProgressBar progressBar;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

         firstNameText = findViewById(R.id.firstNameText);
         lastNameText = findViewById(R.id.lastNameText);
         emailText = findViewById(R.id.emailText);
         passwordText = findViewById(R.id.passwordText);
         passwordConfirmText = findViewById(R.id.passwordConfirmText);
         schoolSpinner = findViewById(R.id.spinnerSchool);
         progressBar = findViewById(R.id.progressBar);
         progressBar.setVisibility(View.GONE);

        mAuth = FirebaseAuth.getInstance();
        db = FirebaseFirestore.getInstance();
        ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(this, R.array.schools,
                android.R.layout.simple_spinner_dropdown_item);
        schoolSpinner.setAdapter(adapter);
    }

    public void Register(View view) {

        progressBar.setVisibility(View.VISIBLE);

        String firstName = firstNameText.getText().toString();
        String lastName = lastNameText.getText().toString();
        String email = emailText.getText().toString();
        String password = passwordText.getText().toString();
        String passwordConfirm = passwordConfirmText.getText().toString();
        String school = schoolSpinner.getSelectedItem().toString();

        if(!firstName.isEmpty() && !lastName.isEmpty() && !email.isEmpty() && !password.isEmpty()
            && !passwordConfirm.isEmpty() && !school.isEmpty()){
            if(!password.contentEquals(passwordConfirm)){
                Toast.makeText(this, "Passwords do not match", Toast.LENGTH_SHORT).show();
            }else{
                //getSchoolId(school);
                registerUser(firstName, lastName, email, password, school);
            }
        }else{
            progressBar.setVisibility(View.GONE);
            Toast.makeText(this, "All fields required", Toast.LENGTH_SHORT).show();
        }

    }

    private void registerUser(final String firstName, final String lastName, final String email, String password, final String school) {

        mAuth.createUserWithEmailAndPassword(email, password)
                .addOnSuccessListener(this, new OnSuccessListener<AuthResult>() {
                    @Override
                    public void onSuccess(@NonNull AuthResult authResult) {
                        final String uid = mAuth.getCurrentUser().getUid();

                        //get a school id
                        final CollectionReference schoolRef = db.collection("schools");
                         schoolRef.whereEqualTo("name", school).get()
                                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                                    @Override
                                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                                        if(task.isSuccessful()){
                                            if(!task.getResult().isEmpty()){
                                                for(QueryDocumentSnapshot snapshot: task.getResult()){
                                                    addUser(firstName, lastName, email, snapshot.getId(), uid);
                                                }
                                            }else{
                                                Map<String, Object> schoolData = new HashMap<>();
                                                schoolData.put("name", school);
                                                schoolRef.add(schoolData)
                                                        .addOnSuccessListener(new OnSuccessListener<DocumentReference>() {
                                                            @Override
                                                            public void onSuccess(DocumentReference documentReference) {
                                                                addUser(firstName, lastName, email, documentReference.getId(), uid);
                                                            }
                                                        });
                                            }
                                        }else{
                                            Log.d(TAG, "Error: "+task.getException());
                                        }
                                    }
                                }).addOnFailureListener(new OnFailureListener() {
                            @Override
                            public void onFailure(@NonNull Exception e) {
                                Log.d(TAG, "School error: " + e.getMessage());
                                Toast.makeText(RegisterActivity.this, "Unable to Register: "+ e.getMessage(), Toast.LENGTH_LONG).show();
                                progressBar.setVisibility(View.GONE);

                            }
                        });

                    }
                }).addOnFailureListener(this, new OnFailureListener() {
            @Override
            public void onFailure(@NonNull Exception e) {
                Log.d(TAG, "Registration error: " + e.getMessage());
                Toast.makeText(RegisterActivity.this, "Unable to Register: "+ e.getMessage(), Toast.LENGTH_LONG).show();
                progressBar.setVisibility(View.GONE);
            }
        });


    }

    private void addNewSchool(final String school){
        Map<String, Object> schoolData = new HashMap<>();
        schoolData.put("name", school);
        db.collection("schools").add(schoolData)
                .addOnSuccessListener(this, new OnSuccessListener<DocumentReference>() {
                    @Override
                    public void onSuccess(DocumentReference documentReference) {
                        schoolId = documentReference.getId();
                    }
                }).addOnFailureListener(this, new OnFailureListener() {
            @Override
            public void onFailure(@NonNull Exception e) {
                Toast.makeText(RegisterActivity.this, "Unable to Register", Toast.LENGTH_SHORT).show();
                progressBar.setVisibility(View.GONE);
            }
        });
    }

    private void addUser(final String firstName, final String lastName, final String email,
                         final String schoolId, final String uid){
        DocumentReference documentReference = db.collection("users").document(uid);
        Map<String, Object> user = new HashMap<>();
        user.put("firstName", firstName);
        user.put("lastName", lastName);
        user.put("email", email);
        user.put("schoolId", schoolId);
        Log.d(TAG, "user: "+ Arrays.asList(user));
        documentReference.set(user).addOnSuccessListener(new OnSuccessListener<Void>(){
            @Override
            public void onSuccess(Void aVoid){
                registrationSuccess();
            }
        });
    }

    private void registrationSuccess() {
        Intent intent = new Intent(this, MainActivity.class);
        String reg = "success";
        intent.putExtra("registration", reg);
        startActivity(intent);
    }

    //This method is a modified version of one found at: https://stackoverflow.com/questions/4238921/detect-whether-there-is-an-internet-connection-available-on-android on 4/14/20
    private boolean checkNetwork() {
        ConnectivityManager connectivityManager
                = (ConnectivityManager) getSystemService(getApplicationContext().CONNECTIVITY_SERVICE);
        NetworkInfo networkInfo = connectivityManager.getActiveNetworkInfo();
        return networkInfo != null && networkInfo.isConnected();
    }
}
