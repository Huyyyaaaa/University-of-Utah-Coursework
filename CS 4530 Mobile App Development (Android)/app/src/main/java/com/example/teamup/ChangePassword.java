package com.example.teamup;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;

public class ChangePassword extends AppCompatActivity {

    private TextView backBtn;
    private EditText password, passwordConfirm;
    private Button saveBtn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_change_password);

        backBtn = findViewById(R.id.backArrow);
        backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        password = findViewById(R.id.editTextChangePass);
        passwordConfirm = findViewById(R.id.editTextChangePassConfirm);
        saveBtn = findViewById(R.id.buttonSavePassword);
        saveBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                updatePassword();
            }
        });

    }

    public void updatePassword(){
        final String newPassword = password.getText().toString();
        final String newPasswordConfirm = passwordConfirm.getText().toString();

        if(!newPassword.isEmpty() && !newPasswordConfirm.isEmpty()){
            if(newPassword.contentEquals(newPasswordConfirm)){

                FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
                if(user != null){
                    user.updatePassword(newPassword)
                            .addOnCompleteListener(new OnCompleteListener<Void>() {
                                @Override
                                public void onComplete(@NonNull Task<Void> task) {
                                    if(task.isSuccessful()){
                                        Toast.makeText(ChangePassword.this,
                                                "Updated Password Successfully!", Toast.LENGTH_SHORT).show();
                                    }else{
                                        Toast.makeText(ChangePassword.this,
                                                "Unable to Update Password", Toast.LENGTH_SHORT).show();
                                    }
                                }
                            });
                }
            }else{
                Toast.makeText(this, "Passwords do not match", Toast.LENGTH_SHORT).show();
            }
        }else{
            Toast.makeText(this, "All fields are required", Toast.LENGTH_SHORT).show();
        }

    }
}
