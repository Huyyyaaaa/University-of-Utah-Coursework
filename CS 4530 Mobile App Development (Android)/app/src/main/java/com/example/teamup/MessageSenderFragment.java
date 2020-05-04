package com.example.teamup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.DocumentChange;
import com.google.firebase.firestore.DocumentReference;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.EventListener;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.FirebaseFirestoreException;
import com.google.firebase.firestore.ListenerRegistration;
import com.google.firebase.firestore.Query;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.Instant;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.Random;

@RequiresApi(api = Build.VERSION_CODES.O)
public class MessageSenderFragment extends Fragment implements MessageSenderAdapter.OnNoteListener {

    private FirebaseFirestore db;
    ArrayList<MessageSenderAdapterListItem> items = new ArrayList<MessageSenderAdapterListItem>();
    String teamId;
    android.text.format.DateFormat df = new android.text.format.DateFormat();
    public static final SimpleDateFormat sdf = new SimpleDateFormat("M/d/yyyy h:mm aa");
    String firebase_sdf_format = "yyyy-MM-dd HH:mm:ss.SSS";
    SimpleDateFormat firebase_sdf = new SimpleDateFormat(firebase_sdf_format);
    ListenerRegistration listener_registration;
    private NetworkManager networkManager;

    public MessageSenderFragment(){
        // Required empty public constructor
    }

    private void listenForMessages(final RecyclerView recyclerView){
            listener_registration = db.collection("messagesTeams").addSnapshotListener(new EventListener<QuerySnapshot>() {
                @Override
                public void onEvent(@Nullable QuerySnapshot queryDocumentSnapshots, @Nullable FirebaseFirestoreException e) {
                    Log.d("MessageSenderFragment", "listen:event");
                    if (e != null) {
                        Log.e("MessageSenderFragment", "listen:error", e);
                        return;
                    }
                    for (DocumentChange dc : queryDocumentSnapshots.getDocumentChanges()) {
                        if (dc.getType() == DocumentChange.Type.ADDED){
                            boolean dc_break = false;
                            for(MessageSenderAdapterListItem i : items){
                                Log.d("MessageSenderFragment", "comparing " + i.snapshotId + " to  " + dc.getDocument().getId());
                                if (i.snapshotId != dc.getDocument().getId()){
                                    Log.d("MessageSenderFragment", "listen:initmessageItemList");
                                    initMessageItemList(recyclerView);
                                    dc_break = true;
                                    break;
                                }
                            }
                            if (dc_break) break;
                        }
                    }
                }
            });


    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        networkManager = new NetworkManager(getContext());
        db = FirebaseFirestore.getInstance();

        View v = inflater.inflate(R.layout.fragment_messagesender, container, false);
        getActivity().findViewById(R.id.nav_view).setVisibility(View.INVISIBLE);
        if (getArguments() == null){
            Log.e("MessageSenderFragment", "TeamId not passed to Messanger Activity Launch");
            return v;
        }
        teamId = getArguments().getString("teamId");
        Log.d("MessageSenderFragment", "messanger teamid " + teamId + " was launched!");

        // set submit button listener
        v.findViewById(R.id.send_btn).setOnClickListener(new View.OnClickListener() {
            @RequiresApi(api = Build.VERSION_CODES.O)
            @Override
            public void onClick(final View v) {
                Log.d("MessageSenderFragment", "send button clicked");
                // send message
                if(networkManager.isOnline()) {
                    // get message text
                    final EditText message_input = ((EditText)getActivity().findViewById(R.id.message_input));
                    String message_text = message_input.getText().toString();
                    // if not blank, add message to firebase
                    if (message_text == "" || message_text.isEmpty() || message_text.length() == 0){
                        return;
                    }
                    FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
                    final String user_id = user.getUid();
                    // prepare firebase message object we will send
                    Map<String, Object> messageData = new HashMap<>();
                    messageData.put("datetime", firebase_sdf.format(new Date()));
                    messageData.put("sender", user_id);
                    messageData.put("teamId", teamId);
                    messageData.put("text", message_text);
                    // add message to firebase
                    db.collection("messagesTeams").add(messageData).addOnSuccessListener(new OnSuccessListener<DocumentReference>() {
                        @RequiresApi(api = Build.VERSION_CODES.O)
                        @Override
                        public void onSuccess(DocumentReference documentReference) {
                            //RecyclerView recyclerView = (RecyclerView) getActivity().findViewById(R.id.recyclerview_chat);
                            //items.clear();
                            // Initialize team message items again
                            //initMessageItemList(recyclerView); COMMENT BLOCK EXPLANATION no need for this listener should take care of
                            // clear text box
                            message_input.setText("");
                            Log.d("MessageSenderFragment", "Added message to teams messages -  message id: " + documentReference.getId());
                        }
                    });



                }
                else{
                    Toast.makeText(getActivity().getApplicationContext(), "Failed to send message, device is currently offline", Toast.LENGTH_SHORT).show();
                }
            }
        });





        RecyclerView recyclerView = (RecyclerView) v.findViewById(R.id.recyclerview_chat);
        items.clear();
        initMessageItemList(recyclerView);
        listenForMessages(recyclerView);
        return v;
    }


    @Override
    public void onDetach(){
        Log.d("MessageSenderFragment", "fragment detached");
        getActivity().findViewById(R.id.nav_view).setVisibility(View.VISIBLE);
        if (listener_registration != null){
            listener_registration.remove();
        }
        super.onDetach();
    }

    public void initMessageItemList(final RecyclerView recyclerView) {
        items.clear();
        try{
            FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
            final String user_id = user.getUid();
            db.collection("messagesTeams").whereEqualTo("teamId", teamId).orderBy("datetime", Query.Direction.ASCENDING).get()
                    .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                        @Override
                        public void onComplete(@NonNull Task<QuerySnapshot> task) {
                            if (task.isSuccessful()) {
                                if (!task.getResult().isEmpty()) {
                                    for (final QueryDocumentSnapshot snapshot : task.getResult()) {
                                        final MessageTeam teamMessage = snapshot.toObject(MessageTeam.class);
                                        Log.d("messageSenderFragment", "Just read team message - messageTeam id" + snapshot.getId());
                                        // get the user name pertaining to the user

                                        // Create message block item
                                        MessageSenderAdapterListItem item = new MessageSenderAdapterListItem();
                                        item.senderTitle = ""; // will be updated later async
                                        try {
                                            item.messageTime = sdf.format(firebase_sdf.parse(teamMessage.datetime));
                                        } catch (ParseException e) {
                                            e.printStackTrace();
                                        }
                                        item.messageText = teamMessage.text;
                                        if (getActivity() == null){
                                            items.clear();
                                            return;
                                        }
                                        String img_path = "profile_img" + ((Math.abs(teamMessage.sender.hashCode()) % 15) + 1);
                                        Log.d("messageSenderFragment", "Loading image " + img_path);
                                        int profile_res_id = getResources().getIdentifier(img_path, "drawable", getActivity().getPackageName());
                                        item.profileImage = ContextCompat.getDrawable(getContext(), profile_res_id);
                                        if (user_id.equals(teamMessage.sender)){
                                            item.currentUserIsSender = true;
                                        }
                                        else{
                                            item.currentUserIsSender = false;
                                        }
                                        item.snapshotId = snapshot.getId();
                                        items.add(item);


                                        db.collection("users").document(teamMessage.sender).get()
                                                .addOnCompleteListener(new OnCompleteListener<DocumentSnapshot>() {
                                                    @RequiresApi(api = Build.VERSION_CODES.O)
                                                    @Override
                                                    public void onComplete(@NonNull Task<DocumentSnapshot> task) {
                                                        if (task.isSuccessful() && task.getResult().exists()) {
                                                            final String sender_name = (String) task.getResult().getData().get("firstName") + " " + ((String) task.getResult().getData().get("lastName")).charAt(0);
                                                            Log.d("messageSenderFragment", "Just read user " + sender_name);
                                                            // update sender name in item
                                                            for (MessageSenderAdapterListItem i : items){
                                                                if (i.snapshotId.equals(snapshot.getId())){
                                                                    i.senderTitle = sender_name;
                                                                    break;
                                                                }
                                                            }
                                                            updateview(recyclerView);
                                                        } else {
                                                            Log.e("messageSenderFragment", "Error getting sender: " + teamMessage.sender, task.getException());
                                                        }
                                                    }
                                                });

                                    }
                                } else {
                                    Log.d("messageSenderFragment", "Getting messages for teamId " + teamId + " got zero entries");
                                }
                            } else {
                                // task not successful
                                Log.e("messageSenderFragment", "Getting messages for teamId " + teamId + " not successful " + task.getException());
                            }
                        }
                    });
        } catch(Exception e){
            Log.e("messageSenderFragment", e.getStackTrace().toString());
        }


    }

    private void updateview(RecyclerView recyclerView){
        LinearLayoutManager llm = new LinearLayoutManager(getActivity());
        llm.setStackFromEnd(true);
        recyclerView.setLayoutManager(llm);
        MessageSenderAdapter adapter = new MessageSenderAdapter(items, this);
        recyclerView.setAdapter(adapter);
    }

    public void initItemsTest(){
        android.text.format.DateFormat df = new android.text.format.DateFormat();
        String formattedDate = df.format("M/d/yyyy h:mm tt", Calendar.getInstance().getTime()).toString();
        //test stub
        MessageSenderAdapterListItem e = new MessageSenderAdapterListItem();
        int profile_res_id = getResources().getIdentifier("profile_img" + (new Random().nextInt(3) + 1), "drawable", getActivity().getPackageName());
        e.profileImage = ContextCompat.getDrawable(getContext(), profile_res_id);
        e.currentUserIsSender = true;
        e.messageText = "This is a short message from myself";
        e.messageTime = formattedDate;
        e.senderTitle = "bb (Me)";
        items.add(e);
        MessageSenderAdapterListItem e2 = new MessageSenderAdapterListItem();
        profile_res_id = getResources().getIdentifier("profile_img" + (new Random().nextInt(3) + 1), "drawable", getActivity().getPackageName());
        e2.profileImage = ContextCompat.getDrawable(getContext(), profile_res_id);
        e2.currentUserIsSender = false;
        e2.messageText = "This is a short message from another";
        e2.messageTime = formattedDate;
        e2.senderTitle = "bb2 (Other)";
        items.add(e2);
        items.add(e2);
        items.add(e);
        items.add(e2);
    }

    public static MessageSenderFragment newInstance(String teamId) {
        MessageSenderFragment fragment = new MessageSenderFragment();
        Bundle args = new Bundle();
        args.putString("teamId", teamId);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onNoteClick(int position) {
    }
}
