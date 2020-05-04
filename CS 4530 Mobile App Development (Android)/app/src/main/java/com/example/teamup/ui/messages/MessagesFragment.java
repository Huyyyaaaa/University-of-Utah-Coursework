package com.example.teamup.ui.messages;

import android.content.Intent;
import android.os.Bundle;
import android.text.SpannableString;
import android.text.style.UnderlineSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.teamup.MessageSenderAdapterListItem;
import com.example.teamup.MessageTeam;
import com.example.teamup.MessagesAdapter;
import com.example.teamup.MessagesAdapterListItem;
import com.example.teamup.MessageSenderFragment;
import com.example.teamup.R;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.DocumentChange;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.EventListener;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.FirebaseFirestoreException;
import com.google.firebase.firestore.ListenerRegistration;
import com.google.firebase.firestore.Query;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import com.example.teamup.UserTeam;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.Random;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class MessagesFragment extends Fragment implements MessagesAdapter.OnNoteListener{
    private MessagesViewModel messagesViewModel;
    private ArrayList<UserTeam> userTeams = new ArrayList<UserTeam>();
    ListenerRegistration listener_registration;
    android.text.format.DateFormat df = new android.text.format.DateFormat();
    public static final SimpleDateFormat sdf = new SimpleDateFormat("M/d/yyyy h:mm aa");
    String firebase_sdf_format = "yyyy-MM-dd HH:mm:ss.SSS";
    SimpleDateFormat firebase_sdf = new SimpleDateFormat(firebase_sdf_format);

    private FirebaseFirestore db;
    private String email;
    private View view;

    private void listenForMessages(final RecyclerView recyclerView){
        listener_registration = db.collection("messagesTeams").addSnapshotListener(new EventListener<QuerySnapshot>() {
            @Override
            public void onEvent(@Nullable QuerySnapshot queryDocumentSnapshots, @Nullable FirebaseFirestoreException e) {
                Log.d("MessagesFragment", "listen:event");
                if (e != null) {
                    Log.e("MessagesFragment", "listen:error", e);
                    return;
                }
                // for calculating only notifying
                Date time_thresh = Calendar.getInstance().getTime();
                Calendar cal = Calendar.getInstance();
                cal.setTime(time_thresh);
                cal.add(Calendar.MINUTE, -1);
                time_thresh = cal.getTime();
                for (DocumentChange dc : queryDocumentSnapshots.getDocumentChanges()) {
                    try{
                        Date datetime = firebase_sdf.parse((String)dc.getDocument().get("datetime"));
                        if (dc.getType() == DocumentChange.Type.ADDED && datetime.after(time_thresh)){
                            items.clear();
                            // refresh team message items
                            initMessageItemList(recyclerView);
                        }
                    }
                    catch (Exception gE){
                        Log.e("MessagesFragment", "Error " + gE.getStackTrace());
                    }
                }
            }
        });


    }

    @Override
    public void onDetach(){
        Log.d("MessagesFragment", "fragment detached");
        if (listener_registration != null){
            listener_registration.remove();
        }
        super.onDetach();
    }


    public View onCreateView(@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {
        Log.d("MessagesFragment", "this ran");
        messagesViewModel =
                ViewModelProviders.of(this).get(MessagesViewModel.class);
        View root = inflater.inflate(R.layout.fragment_messages, container, false);
//        final TextView textView = root.findViewById(R.id.text_messages);
//        messagesViewModel.getText().observe(getViewLifecycleOwner(), new Observer<String>() {
//            @Override
//            public void onChanged(@Nullable String s) {
//  //              textView.setText(s);
//            }
//        });
        Log.d("MessagesFragment", "that ran");
        super.onCreate(savedInstanceState);

        db = FirebaseFirestore.getInstance();

        //register adapter
        // Lookup the recyclerview in activity layout
        RecyclerView recyclerView = (RecyclerView) root.findViewById(R.id.messageItems);
        items.clear();
        // Initialize team message items
        initMessageItemList(recyclerView);
        listenForMessages(recyclerView);
        return root;
    }

    ArrayList<MessagesAdapterListItem> items = new ArrayList<MessagesAdapterListItem>();
    ReentrantReadWriteLock itemsLock = new ReentrantReadWriteLock();

    public void initMessageItemList(final RecyclerView recyclerView){

        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
        if(user != null){
            email = user.getEmail();
        }

        db.collection("userTeams").whereEqualTo("email", email).get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if(task.isSuccessful()){

                            if(!task.getResult().isEmpty()){
                                for(QueryDocumentSnapshot snapshot: task.getResult()){
                                    //make a collection of courses and put on ui item
                                    final UserTeam userteam = snapshot.toObject(UserTeam.class);
                                    Log.d("MessagesFragment", "Just read user team - team id" + userteam.teamId);
                                    // get team relating to team id
                                    db.collection("teams").document(userteam.teamId).get()
                                            .addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
                                                @Override
                                                public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                                                    if (task.isSuccessful()) {
                                                        if(task.getResult().exists()){
                                                            final String team = (String)task.getResult().getData().get("name");

                                                            Log.d("MessagesFragment", "Just read the team - team name " + team);

                                                            // Get last known message on team

                                                            db.collection("messagesTeams").whereEqualTo("teamId", userteam.teamId).orderBy("datetime", Query.Direction.DESCENDING).limit(1).get()
                                                                    .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                                                                        @Override
                                                                        public void onComplete(@NonNull Task<QuerySnapshot> task) {
                                                                            if(task.isSuccessful()){
                                                                                if(!task.getResult().isEmpty()){
                                                                                    for(QueryDocumentSnapshot snapshot: task.getResult()){
                                                                                        final MessageTeam message = snapshot.toObject(MessageTeam.class);

                                                                                        if (message == null){
                                                                                            constructMessageItemAddAndUpdateView(recyclerView, userteam, team, null, "");
                                                                                            return;
                                                                                        }

                                                                                        db.collection("users").document(message.sender).get()
                                                                                                .addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
                                                                                                    @Override
                                                                                                    public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                                                                                                        if (task.isSuccessful() && task.getResult().exists()) {
                                                                                                            String first_name = (String) task.getResult().getData().get("firstName");
                                                                                                            String last_name = (String) task.getResult().getData().get("lastName");
                                                                                                            final String sender_name =  get_full_proper_name(first_name, last_name);

                                                                                                            Log.d("MessagesFragment", "Updating teams messages with:" + message.text + " from " + sender_name);
                                                                                                            constructMessageItemAddAndUpdateView(recyclerView,userteam, team, message, sender_name);

                                                                                                        }
                                                                                                        else{
                                                                                                            Log.d("MessagesFragment", "Failure getting user");
                                                                                                        }
                                                                                                    }
                                                                                                });
                                                                                        return;
                                                                                    }
                                                                                }else{
                                                                                    // no message has been posted on team chat yet
                                                                                    constructMessageItemAddAndUpdateView(recyclerView, userteam, team, null, "");
                                                                                }

                                                                            }else{
                                                                                Log.d("MessagesFragment", "Error getting messages: ", task.getException());
                                                                            }
                                                                        }
                                                                    });
                                                        }
                                                        else{
                                                            Log.d("MessagesFragment", "Error getting team [FATAL]: ");
                                                        }
                                                    } else {
                                                        Log.d("MessagesFragment", "Error getting team: ", task.getException());
                                                    }
                                                }
                                            });
                                }

                            }else{
                                //put a text view on UI for "No courses"
                                //noCourses.setVisibility(View.VISIBLE);
                            }

                            // Create adapter passing in the sample user data

                            // Attach the adapter to the recyclerview to populate items

                            // Set layout manager to position the items

                        }else{
                            Log.d("MessagesFragment", "Error getting teams: ", task.getException());
                        }
                    }
                });

    }

    private String get_proper_name(String first, String last){
        if (first.length() > 1){
            first = first.substring(0, 1).toUpperCase() + first.substring(1).toLowerCase();
        }
        else{
           first = first.substring(0, 1).toUpperCase();
        }
        last = last.substring(0, 1).toUpperCase();
        return first + " " + last;
    }

    private String get_full_proper_name(String first, String last){
        if (first.length() > 1){
            first = first.substring(0, 1).toUpperCase() + first.substring(1).toLowerCase();
        }
        else{
            first = first.substring(0, 1).toUpperCase();
        }
        if (last.length() > 1){
            last = last.substring(0, 1).toUpperCase() + last.substring(1).toLowerCase();
        }
        else{
            last = last.substring(0, 1).toUpperCase();
        }
        return first + " " + last;
    }

    private void constructMessageItemAddAndUpdateView(RecyclerView recyclerView, UserTeam userteam, String team, MessageTeam message, String sender){
        //itemsLock.writeLock().lock();

        try{
            MessagesAdapterListItem item = new MessagesAdapterListItem();

            String img_path = "profile_img" + (Math.abs((team.hashCode() % 15)) + 1);
            Log.d("MessagesFragment", "Loading image " + img_path);
            int profile_res_id = getResources().getIdentifier(img_path, "drawable", getActivity().getPackageName());
            item.profileImage = ContextCompat.getDrawable(getContext(), profile_res_id);
            item.teamName = team;
            item.teamId = userteam.teamId;
            if (message != null){
                item.message = message.text;
            }
            else{
                item.message = "Currently no messages have been posted on this team. Be the first!";
            }
            item.sender = sender;
            item.messageTime = sdf.format(firebase_sdf.parse(message.datetime));

            //itemsLock.writeLock().lock();
            for (MessagesAdapterListItem i : items){
                if (i.teamId.equals(item.teamId)){
                    // dont add this
                    return;
                }
            }
            items.add(item);
            //itemsLock.writeLock().unlock();

            //itemsLock.writeLock().unlock();

            //itemsLock.readLock().lock();

            recyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
            MessagesAdapter adapter = new MessagesAdapter(items, this);
            recyclerView.setAdapter(adapter);
            //itemsLock.readLock().unlock();
        }
        catch (Exception gE){
            Log.e("MessagesFragment", gE.getStackTrace().toString());
        }
    }

    // When user clicks on team messages block
    @Override
    public void onNoteClick(int position) {
        String teamId = items.get(position).teamId;

        //Toast.makeText(getActivity().getApplicationContext(), "messanger teamid " + teamId + " was clicked", Toast.LENGTH_SHORT).show();

        Bundle bundle = new Bundle();
        bundle.putString("teamId", teamId);
        NavController navController = Navigation.findNavController(getActivity(), R.id.nav_host_fragment);
        navController.navigate(R.id.messageSenderFragment, bundle);
}

//    private void addTeamMessageItem() {
//        recyclerView = getView().findViewById(R.id.CoursesRecycler);
//        layoutManager = new LinearLayoutManager(getActivity());
//        recyclerView.setLayoutManager(layoutManager);
//        // specify an adapter
//        mAdapter = new CoursesAdapter(getActivity(),courses);
//        recyclerView.setAdapter(mAdapter);
//    }
}
