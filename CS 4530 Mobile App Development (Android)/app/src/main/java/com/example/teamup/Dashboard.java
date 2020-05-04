package com.example.teamup;

import android.app.ActivityManager;
import android.content.Context;
import android.content.Intent;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.PopupMenu;
import android.widget.TextView;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.firebase.auth.FirebaseAuth;

public class Dashboard extends AppCompatActivity {

    private NetworkManager networkManager;
    Intent mServiceIntent;

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_dashboard);

        networkManager = new NetworkManager(this);

        // Messanger background service
        mServiceIntent = new Intent(this, MessangerService.class);
        if (!isMyServiceRunning(MessangerService.class)) {
            startService(mServiceIntent);
        }

        //Make our own custom action bar so that we can make it look how we want.
        Toolbar toolBar = (Toolbar) findViewById(R.id.custom_action_bar);
        setSupportActionBar(toolBar);
        getSupportActionBar().setDisplayShowTitleEnabled(false); //We are showing our own title so we can put it in the middle.
        TextView dotdotdot = (TextView) findViewById(R.id.dotdotdot);
        TextView plusButton = (TextView) findViewById(R.id.plusButton);
        dotdotdot.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(!networkManager.isOnline()){
                    return;
                }
                PopupMenu popupMenu = new PopupMenu(Dashboard.this, v);
                popupMenu.getMenuInflater().inflate(R.menu.user_menu, popupMenu.getMenu());
                popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
                    @Override
                    public boolean onMenuItemClick(MenuItem item) {
                        switch (item.getItemId()){
                            case R.id.account_settings_menu:
                                Intent intent = new Intent(Dashboard.this, Settings.class);
                                startActivity(intent);
                                return true;
                            case R.id.sign_out:
                                signOutUser();
                                return true;
                            default:
                                return false;
                        }
                    }
                });
                popupMenu.show();
            }


        });
        plusButton.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View v) {
                if(!networkManager.isOnline()){
                    return;
                }
                Intent createTeamIntent = new Intent(Dashboard.this, CreateTeamActivity.class);
                startActivity(createTeamIntent);
            }
        });

        BottomNavigationView navView = findViewById(R.id.nav_view);
        //This makes it so nav items don't disappear when clicked.
        navView.setItemTextColor(ColorStateList.valueOf(Color.BLACK));

    }

    private boolean isMyServiceRunning(Class<?> serviceClass) {
        ActivityManager manager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                Log.d ("Service status", "Running");
                return true;
            }
        }
        Log.i ("Service status", "Not running");
        return false;
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    private void signOutUser() {
        if(!networkManager.isOnline()){
            return;
        }
        FirebaseAuth.getInstance().signOut();
        Intent intent = new Intent(Dashboard.this, MainActivity.class);
        startActivity(intent);
        finish();
    }


    @RequiresApi(api = Build.VERSION_CODES.M)
    public void goToCourses(View view){
        if(!networkManager.isOnline()){
            return;
        }
        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
        navController.navigate(R.id.coursesFragment);

    }
    
    @RequiresApi(api = Build.VERSION_CODES.M)
    public void goToTeamDetails(String teamId){
        if(!networkManager.isOnline()){
            return;
        }
        Bundle bundle = new Bundle();
        bundle.putString("TEAM_ID", teamId);
        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
        navController.navigate(R.id.teamDetailsFragment, bundle);
    }

    //This function changes the view whenever a button is clicked on the bottom nav bar.
    @RequiresApi(api = Build.VERSION_CODES.M)
    public void NavMenuClickListener(MenuItem item){
        int id = item.getItemId();
        TextView plusButton = findViewById(R.id.plusButton);
        plusButton.setVisibility(View.INVISIBLE); //Plus button should only be visible when we are looking at the teamsFragment.
        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);

        if(networkManager.isOnline()) {
            if (id == R.id.navigation_teams) {
                TextView textView = (TextView) findViewById(R.id.toolbar_text);
                textView.setText("Teams");
                plusButton.setVisibility(View.VISIBLE);
                navController.navigate(R.id.navigation_teams);
            }
            if (id == R.id.navigation_notifications) {
                TextView textView = (TextView) findViewById(R.id.toolbar_text);
                textView.setText("Notifications");
                navController.navigate(R.id.navigation_notifications);
            }
            if (id == R.id.navigation_schedule) {
                TextView textView = (TextView) findViewById(R.id.toolbar_text);
                textView.setText("Schedule");
                navController.navigate(R.id.navigation_schedule);
            }
            if (id == R.id.navigation_messages) {
                TextView textView = (TextView) findViewById(R.id.toolbar_text);
                textView.setText("Messages");
                navController.navigate(R.id.navigation_messages);
            }
        }
    }

    public void goToAddCourse(View view) {
        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
        navController.navigate(R.id.addCourseFragment);
    }


   }
