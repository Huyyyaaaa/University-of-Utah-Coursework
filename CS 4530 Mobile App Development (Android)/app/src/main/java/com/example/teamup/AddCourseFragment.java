package com.example.teamup;

import android.app.TimePickerDialog;
import android.os.Build;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.fragment.app.DialogFragment;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.TimePicker;
import android.widget.Toast;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.DocumentReference;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.FieldValue;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class AddCourseFragment extends Fragment {
    private String TAG = "AddCourseFragment";
    private FirebaseFirestore db;
    private EditText courseName, startTime, endTime;
    private CheckBox monday, tuesday, wednesday, thursday, friday;
    private Spinner semester;
    private ProgressBar addCourseProgress;
    private String uid;
    private NetworkManager networkManager;

    public AddCourseFragment() {
        // Required empty public constructor
    }

    // TODO: Rename and change types and number of parameters
    public static AddCourseFragment newInstance(String param1, String param2) {
        AddCourseFragment fragment = new AddCourseFragment();;
        return fragment;
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        db = FirebaseFirestore.getInstance();
        networkManager = new NetworkManager(getContext());
        networkManager.isOnline();
    }

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_add_course, container, false);
        courseName = view.findViewById(R.id.courseNameText);
        startTime = view.findViewById(R.id.editTextStart);
        startTime.setShowSoftInputOnFocus(false);
        endTime = view.findViewById(R.id.editTextEnd);
        endTime.setShowSoftInputOnFocus(false);
        monday = view.findViewById(R.id.checkBoxMonday);
        tuesday = view.findViewById(R.id.checkBoxTuesday);
        wednesday= view.findViewById(R.id.checkBoxWednesday);
        thursday = view.findViewById(R.id.checkBoxThursday);
        friday = view.findViewById(R.id.checkBoxFriday);
        semester = view.findViewById(R.id.spinnerSemester);
        addCourseProgress = view.findViewById(R.id.progressBar2);
        addCourseProgress.setVisibility(View.GONE);
        Button addClassButton = view.findViewById(R.id.buttonCourseSubmit);
        addClassButton.setOnClickListener(new View.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.M)
            @Override
            public void onClick(View v) {
                attemptAddCourse();
            }
        });

        //click listeners for times
        startTime.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                TimePickerDialog startPicker = new TimePickerDialog(getActivity(), new TimePickerDialog.OnTimeSetListener() {
                    @Override
                    public void onTimeSet(TimePicker view, int hourOfDay, int minute) {
                        String ampm;
                        int hour = hourOfDay;
                        if(hourOfDay >= 12){
                            ampm = "PM";
                            hour = hourOfDay - 12;
                        }else{
                            ampm = "AM";
                        }
                        startTime.setText(String.format("%02d:%02d", hour, minute) + " " + ampm);
                    }
                },0,0,false);
                startPicker.show();
            }
        });

        endTime.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                TimePickerDialog endPicker = new TimePickerDialog(getActivity(), new TimePickerDialog.OnTimeSetListener() {
                    @Override
                    public void onTimeSet(TimePicker view, int hourOfDay, int minute) {
                        String ampm;
                        int hour = hourOfDay;
                        if(hourOfDay >= 12){
                            ampm = "PM";
                            hour -= 12;
                        }else{
                            ampm = "AM";
                        }
                        endTime.setText(String.format("%02d:%02d", hour, minute) + " " + ampm);
                    }
                },0,0,false);
                endPicker.show();
            }
        });
        return view;
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    public void attemptAddCourse(){
        addCourseProgress.setVisibility(View.VISIBLE);
        String course = courseName.getText().toString();
        String start = startTime.getText().toString();
        String end = endTime.getText().toString();
        String sem = semester.getSelectedItem().toString();
        ArrayList<String> days = new ArrayList<>();


        if(!course.isEmpty() && !start.isEmpty() && !end.isEmpty() && !sem.isEmpty() && validateCourseName(course)){
            if(monday.isChecked()){
                days.add("Mon");
            }
            if(tuesday.isChecked()){
                days.add("Tues");
            }
            if(wednesday.isChecked()){
                days.add("Wed");
            }
            if(thursday.isChecked()){
                days.add("Thurs");
            }
            if(friday.isChecked()){
                days.add("Fri");
            }

            AddCourse(course, start, end, sem, days);
        } else{
            addCourseProgress.setVisibility(View.GONE);
            Toast.makeText(getActivity(), "Some fields are incorrect or missing", Toast.LENGTH_LONG).show();
        }
    }

    public Boolean validateCourseName(String name){
        if(name == null){
            return false;
        }
        String regex = "[A-Z]{1,5} [0-9]{1,6}";
        Pattern pattern = Pattern.compile(regex);
        Matcher m = pattern.matcher(name);
        while(m.find()){
            return true;
        }

        return false;
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    private void AddCourse(final String course, final String start, final String end,
                           final String semester, final ArrayList<String> days){

        if(!networkManager.isOnline()){
            return;
        }
        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
        if(user != null){
            uid = user.getUid();
        }


        DocumentReference userDoc = db.collection("users").document(uid);
        userDoc.get().addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
            @Override
            public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                if(task.isSuccessful()){
                    DocumentSnapshot documentSnapshot = task.getResult();
                    if(documentSnapshot.exists()){
                        final String schoolid = documentSnapshot.getString("schoolId");

                        //add the course
                        final CollectionReference courseRef = db.collection("courses");
                        courseRef.whereEqualTo("name", course)
                                .whereEqualTo("start", start)
                                .whereEqualTo("end", end)
                                .whereEqualTo("semester", semester)
                                .whereEqualTo("days", days)
                                .whereEqualTo("schoolId", schoolid)
                                .get()
                                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                                    @Override
                                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                                        if(task.isSuccessful()){
                                            if(!task.getResult().isEmpty()){
                                                for(QueryDocumentSnapshot snapshot: task.getResult()){
                                                    final String cId = snapshot.getId();
                                                    courseRef.document(cId).update("users", FieldValue.arrayUnion(uid));

                                                }
                                                addCourseSuccessful();
                                            }else{
                                                Map<String, Object> courseData = new HashMap<>();
                                                courseData.put("name", course);
                                                courseData.put("start", start);
                                                courseData.put("end", end);
                                                courseData.put("semester", semester);
                                                courseData.put("days", days);
                                                courseData.put("schoolId", schoolid);
                                                courseData.put("users", Arrays.asList(uid));

                                                courseRef.add(courseData)
                                                        .addOnSuccessListener(new OnSuccessListener<DocumentReference>() {
                                                            @Override
                                                            public void onSuccess(DocumentReference documentReference) {
                                                                addCourseSuccessful();
                                                            }
                                                        });
                                            }
                                        }else{
                                            Log.d(TAG, "Error: "+task.getException());
                                        }
                                    }
                                });


                    }
                }else{
                    Log.d(TAG, "Error: "+task.getException());
                }
            }
        });

    }

    private  void addCourseSuccessful(){
        addCourseProgress.setVisibility(View.GONE);
        NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
        navController.navigate(R.id.coursesFragment);
        Toast.makeText(getActivity(), "Sucessfully added course!", Toast.LENGTH_LONG).show();
    }

    public void showTimePickerDialogStart(View v) {
        DialogFragment newFragment = new TimePickerFragment();
        newFragment.show(getFragmentManager(), "timePicker");
    }
}
