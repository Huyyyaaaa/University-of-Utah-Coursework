package com.example.teamup;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Build;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.FieldValue;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.util.List;

public class CoursesAdapter extends RecyclerView.Adapter<CoursesAdapter.CoursesViewHolder> {
    private String TAG = "CoursesAdapter";
    private List<Course> courses;
    Context mContext;
    private NetworkManager networkManager;

    public CoursesAdapter(Context mContext, List<Course> courses) {
        this.courses = courses;
        this.mContext = mContext;
        networkManager = new NetworkManager(mContext);
    }

    @Override
    public CoursesViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemCourse = LayoutInflater.from(parent.getContext()).inflate(R.layout.course_row, parent, false);
        CoursesViewHolder coursesViewHolder = new CoursesViewHolder(itemCourse);
        coursesViewHolder.name = itemCourse.findViewById(R.id.courseRowName);
        coursesViewHolder.time = itemCourse.findViewById(R.id.courseRowTime);
        coursesViewHolder.days = itemCourse.findViewById(R.id.courseRowDays);
        coursesViewHolder.deleteBtn = itemCourse.findViewById(R.id.courseRowDeleteBtn);

        return coursesViewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull final CoursesAdapter.CoursesViewHolder holder, final int position) {
        final Course course = courses.get(position);
        holder.name.setText(course.getName());
        holder.time.setText(course.getStart() + " to " + course.getEnd());
        holder.days.setText(course.getDays().toString());
        holder.deleteBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //create a dialog
                showDeleteDialog(course.getName(), position);
            }
        });

    }

    private void showDeleteDialog(final String course, final int pos) {
        AlertDialog.Builder builder = new AlertDialog.Builder(mContext);
        builder.setTitle("Delete Course?");
        builder.setMessage("Are you sure you want to delete " + course);
        builder.setPositiveButton("Yes", new DialogInterface.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.M)
            @Override
            public void onClick(DialogInterface dialog, int which) {
                    //remove user from course
                    removeUserFromCourse(course, pos);
            }
        });
        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                    dialog.cancel();
            }
        });
        AlertDialog dialog = builder.create();
        dialog.show();
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    private void removeUserFromCourse(final String course, final int pos) {
        if(!networkManager.isOnline()){
            return;
        }
        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
        if(user!= null){
            final String uid = user.getUid();

            FirebaseFirestore db = FirebaseFirestore.getInstance();
            final CollectionReference courseRef = db.collection("courses");
            courseRef.whereEqualTo("name", course)
                    .whereArrayContains("users", uid).get()
                    .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                        @Override
                        public void onComplete(@NonNull Task<QuerySnapshot> task) {
                            if(task.isSuccessful()){
                                if(!task.getResult().isEmpty()){
                                    String courseId="";
                                    for(QueryDocumentSnapshot snapshot: task.getResult()){
                                         courseId = snapshot.getId();
                                    }
                                    courseRef.document(courseId)
                                            .update("users",FieldValue.arrayRemove(uid));

                                }
                            }else{
                                Log.d(TAG,"Unable to remove uid", task.getException());
                            }
                        }
                    });
        }

        courses.remove(pos);
        this.notifyDataSetChanged();//update array adapter after delete
    }

    public void clear(){
        int size = courses.size();
        courses.clear();
        notifyItemRangeRemoved(0, size);
    }


    @Override
    public int getItemCount() {
        return courses.size();
    }


    public static class CoursesViewHolder extends RecyclerView.ViewHolder {
        View view;
        TextView name, time, days;
        Button deleteBtn;

        public CoursesViewHolder(View v) {
            super(v);
            this.view = v;
        }
    }

}