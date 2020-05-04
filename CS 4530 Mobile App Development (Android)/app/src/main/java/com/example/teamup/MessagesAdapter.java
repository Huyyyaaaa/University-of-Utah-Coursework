package com.example.teamup;

import android.content.Context;
import android.text.SpannableString;
import android.text.style.UnderlineSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView;

import java.util.List;
import java.util.concurrent.locks.ReentrantReadWriteLock;

import de.hdodenhof.circleimageview.CircleImageView;

public class MessagesAdapter extends RecyclerView.Adapter<MessagesAdapter.ViewHolder> {

    private List<MessagesAdapterListItem> items;
    private OnNoteListener onNoteListener;
    ReentrantReadWriteLock itemsLock = new ReentrantReadWriteLock();

    public MessagesAdapter(List<MessagesAdapterListItem> items, OnNoteListener onNoteListener){
        this.items = items;
        this.onNoteListener = onNoteListener;
    }

    @NonNull
    @Override
    public MessagesAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);

        // Inflate the custom layout
        View contactView = inflater.inflate(R.layout.team_message_item, parent, false);

        // Return a new holder instance
        ViewHolder viewHolder = new ViewHolder(contactView, onNoteListener);
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull MessagesAdapter.ViewHolder holder, int position) {

        MessagesAdapterListItem item = items.get(position);
        holder.icon_avata.setImageDrawable(item.profileImage);
        holder.message.setText(item.message);
        holder.message_time.setText(item.messageTime);
        SpannableString content = new SpannableString(item.teamName);
        content.setSpan(new UnderlineSpan(), 0, item.teamName.length(), 0);
        holder.team_name.setText(content);
        item.sender = "-" + item.sender;
        holder.sender.setText(item.sender);


    }

    @Override
    public int getItemCount() {
        return items.size();
    }

    // Provide a direct reference to each of the views within a data item
    // Used to cache the views within the item layout for fast access
    public class ViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener{
        // Your holder should contain a member variable
        // for any view that will be set as you render a row
        public CircleImageView icon_avata;
        public TextView team_name;
        public TextView message;
        public TextView message_time;
        public TextView sender;

        OnNoteListener monNoteListener;


        // We also create a constructor that accepts the entire item row
        // and does the view lookups to find each subview
        public ViewHolder(View itemView, OnNoteListener onNoteListener) {
            // Stores the itemView in a public final member variable that can be used
            // to access the context from any ViewHolder instance.
            super(itemView);

            icon_avata = (CircleImageView) itemView.findViewById(R.id.icon_avata);
            team_name = (TextView) itemView.findViewById(R.id.team_name);
            message = (TextView) itemView.findViewById(R.id.message);
            message_time = (TextView) itemView.findViewById(R.id.message_time);
            sender = (TextView) itemView.findViewById(R.id.sender);

            monNoteListener = onNoteListener;

            itemView.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            Log.d("MessagesAdapter", "Note clicked");
            onNoteListener.onNoteClick(getAdapterPosition());
        }
    }

    public interface OnNoteListener{
        void onNoteClick(int position);
    }
}