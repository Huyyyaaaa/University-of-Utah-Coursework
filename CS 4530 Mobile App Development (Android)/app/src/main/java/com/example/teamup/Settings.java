package com.example.teamup;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.auth.UserProfileChangeRequest;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

public class Settings extends AppCompatActivity {

    private TextView backBtn;
    private EditText email, name, lastname;
    private Button btnChangePassword, btnUpdate;
    private FirebaseFirestore db;
    private  String uid, currentEmail;
    private NetworkManager networkManager;

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_settings);
        networkManager = new NetworkManager(getApplicationContext());

        db = FirebaseFirestore.getInstance();

        email = findViewById(R.id.editTextEmail);
        name = findViewById(R.id.editTextFirstname);
        lastname = findViewById(R.id.editTextLastname);
        btnUpdate = findViewById(R.id.buttonUpdateSettings);
        btnUpdate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                updateUserInformation();
            }
        });

        backBtn = findViewById(R.id.backArrow);
        backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        btnChangePassword = findViewById(R.id.buttonChangePassword);
        btnChangePassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Settings.this, ChangePassword.class);
                startActivity(intent);
            }
        });

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            getUserInformation();
        }

    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    public void getUserInformation(){
        if(!networkManager.isOnline()){
            return;
        }
        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
        if(user != null){
            email.setText(user.getEmail());
            currentEmail = user.getEmail();
            uid = user.getUid();

            db.collection("users").document(uid).get()
                    .addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
                        @Override
                        public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                            if(task.isSuccessful()){
                                DocumentSnapshot snapshot = task.getResult();
                                if(snapshot.exists()){
                                    name.setText(snapshot.get("firstName").toString());
                                    lastname.setText(snapshot.get("lastName").toString());
                                }else{
                                    Toast.makeText(Settings.this,
                                            "unable to get your information", Toast.LENGTH_SHORT).show();
                                }
                            }
                        }
                    });
        }
    }

    public void updateUserInformation(){

        if(email.getText().toString().isEmpty() || name.getText().toString().isEmpty() || lastname.getText().toString().isEmpty()){
            Toast.makeText(this, "All fields are required", Toast.LENGTH_SHORT).show();
            return;
        }

        final FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();

        user.updateEmail(email.getText().toString())
                .addOnCompleteListener(new OnCompleteListener<Void>() {
                    @Override
                    public void onComplete(@NonNull Task<Void> task) {
                        if(task.isSuccessful()){
                            db.collection("users").document(uid)
                                    .update("firstName", name.getText().toString(),
                                            "lastName", lastname.getText().toString(),
                                            "email", email.getText().toString())
                                            .addOnCompleteListener(new OnCompleteListener<Void>() {
                                                @Override
                                                public void onComplete(@NonNull Task<Void> task) {
                                                    if(task.isSuccessful()){
                                                        updateUserTeams();
                                                    }else{
                                                        errorUpdating();
                                                    }
                                                }
                                            });


                        }
                    }
                });




    }

    private void errorUpdating() {
        Toast.makeText(this, "Error updating information", Toast.LENGTH_SHORT).show();
    }

    private void updateUserTeams() {

        final CollectionReference utRef = db.collection("userTeams");
        utRef.whereEqualTo("email", currentEmail).get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @RequiresApi(api = Build.VERSION_CODES.M)
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if(task.isSuccessful()){
                            String docId="";
                            for(QueryDocumentSnapshot snapshot: task.getResult()) {
                                docId = snapshot.getId();
                                utRef.document(docId).update("email", email.getText().toString());
                            }
                            updateSuccess();
                        }else{
                            errorUpdating();
                        }
                    }
                });
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    private void updateSuccess() {
        Toast.makeText(this, "Successfully updated!", Toast.LENGTH_SHORT).show();
        getUserInformation();
    }
}
