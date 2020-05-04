package com.example.teamup;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;


/**
 * A simple {@link Fragment} subclass.
 * Use the {@link CoursesFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class CoursesFragment extends Fragment {

    private String uid;
    private String TAG = "CoursesFragment";
    private List<Course> courses = new ArrayList<>();
    private RecyclerView recyclerView;
    private RecyclerView.Adapter mAdapter;
    private RecyclerView.LayoutManager layoutManager;
    private TextView noCourses;
    private ProgressBar progress;

    public CoursesFragment() {
        // Required empty public constructor
    }
        private FirebaseFirestore db;
    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     * @return A new instance of fragment Courses.
     */
    // TODO: Rename and change types and number of parameters
    public static CoursesFragment newInstance(String param1, String param2) {
        CoursesFragment fragment = new CoursesFragment();
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        db = FirebaseFirestore.getInstance();


    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v = inflater.inflate(R.layout.fragment_courses, container, false);
        noCourses = v.findViewById(R.id.textViewNoCourses);
        progress = v.findViewById(R.id.progressBarCourses);
        progress.setVisibility(View.GONE);
        getUsersCourses();
        return  v;
    }

    @Override
    public void onPause() {
        super.onPause();
        courses.clear();
    }

    public void getUsersCourses(){
        progress.setVisibility(View.VISIBLE);
        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
        if(user != null){
            uid = user.getUid();
        }

        db.collection("courses")
                .whereArrayContainsAny("users", Arrays.asList(uid)).get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if(task.isSuccessful()){
                            if(!task.getResult().isEmpty()){
                                for(QueryDocumentSnapshot snapshot: task.getResult()){
                                    //make a collection of courses and put on UI
                                    courses.add(snapshot.toObject(Course.class));
                                    addCoursesToView();
                                }

                            }else{
                                //put a text view on UI for "No courses"
                                progress.setVisibility(View.GONE);
                                noCourses.setVisibility(View.VISIBLE);
                            }
                        }else{
                            progress.setVisibility(View.GONE);
                            Log.d(TAG, "Error getting courses: ", task.getException());
                        }
                    }
                });

    }

    private void addCoursesToView() {
        progress.setVisibility(View.GONE);
        recyclerView = getView().findViewById(R.id.CoursesRecycler);
        layoutManager = new LinearLayoutManager(getActivity());
        recyclerView.setLayoutManager(layoutManager);
        // specify an adapter
        mAdapter = new CoursesAdapter(getActivity(),courses);
        recyclerView.setAdapter(mAdapter);
    }
}

