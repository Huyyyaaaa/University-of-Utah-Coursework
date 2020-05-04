package com.example.teamup;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.example.teamup.NetworkManager;

public class MainActivity extends AppCompatActivity {

    private EditText emailText, passwordText;
    private FirebaseAuth mAuth;
    private ProgressBar progressBar;
    private NetworkManager networkManager;
    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        emailText = findViewById(R.id.emailText);
        passwordText = findViewById(R.id.paswordLoginText);
        progressBar = findViewById(R.id.loginProgress);
        progressBar.setVisibility(View.GONE);
        mAuth = FirebaseAuth.getInstance();

        checkPermissions();

        networkManager = new NetworkManager(this);
        if(networkManager.isOnline()){
            //do nothing.
        }

        Intent intent = getIntent();
        intent.getExtras();
        if(intent.hasExtra("registration")){
            Toast.makeText(this, "Registration Successful!", Toast.LENGTH_LONG).show();
        }

    }

    private void checkPermissions() {

        if(ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
        != PackageManager.PERMISSION_GRANTED){
            ActivityCompat.requestPermissions(this,
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 1);
        }
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    public void goToRegister(View view) {
        if(!networkManager.isOnline()){
            return;
        }
        Intent intent = new Intent(this, RegisterActivity.class);
        startActivity(intent);
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    public void attemptLogin(View view) {
        if(!networkManager.isOnline()){
            return;
        }
        progressBar.setVisibility(View.VISIBLE);
        String email = emailText.getText().toString();
        String password = passwordText.getText().toString();

        if(!email.isEmpty() && !password.isEmpty()){

            mAuth.signInWithEmailAndPassword(email, password)
                    .addOnSuccessListener(this, new OnSuccessListener<AuthResult>() {
                        @Override
                        public void onSuccess(AuthResult authResult) {
                            Intent intent = new Intent(MainActivity.this, Dashboard.class);
                            startActivity(intent);
                            finish();
                        }
                    }).addOnFailureListener(this, new OnFailureListener() {
                @Override
                public void onFailure(@NonNull Exception e) {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(MainActivity.this, "Unable to Login. " +e.getMessage(), Toast.LENGTH_SHORT).show();
                }
            });
        }else{
            progressBar.setVisibility(View.GONE);
            Toast.makeText(this, "Email and Password required", Toast.LENGTH_SHORT).show();
        }
    }
}
