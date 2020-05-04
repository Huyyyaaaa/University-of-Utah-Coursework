package com.example.teamup;

import android.content.Context;
import android.text.Spannable;
import android.text.SpannableString;
import android.text.style.UnderlineSpan;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;

public class MessageSenderAdapter extends RecyclerView.Adapter<MessageSenderAdapter.ViewHolder> {

    private List<MessageSenderAdapterListItem> items;
    private OnNoteListener onNoteListener;
    private static int TYPE_OTHER_MESSAGE = 1;
    private static int TYPE_MY_MESSAGE = 2;

    public MessageSenderAdapter(List<MessageSenderAdapterListItem> items, OnNoteListener onNoteListener){
        this.items = items;
        this.onNoteListener = onNoteListener;
    }

    @NonNull
    @Override
    public MessageSenderAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View view;

        if (viewType == TYPE_OTHER_MESSAGE){
            view = inflater.inflate(R.layout.other_user_message_block, parent, false);
        }
        else{
            view = inflater.inflate(R.layout.my_user_message_block, parent, false);
        }
        // Return a new holder instance
        ViewHolder viewHolder = new ViewHolder(view, onNoteListener);
        return viewHolder;
    }
    @Override
    public void onBindViewHolder(@NonNull MessageSenderAdapter.ViewHolder holder, int position) {

        MessageSenderAdapterListItem item = items.get(position);
        if (item.profileImage != null){
            holder.icon_avata.setImageDrawable(item.profileImage);
        }
        holder.message.setText(item.messageText);
        holder.message_time.setText(item.messageTime);
        holder.sender_title.setText(item.senderTitle);

    }

    @Override
    public int getItemViewType (int position){
        MessageSenderAdapterListItem item = items.get(position);
        if (item.currentUserIsSender){
            return TYPE_MY_MESSAGE;
        }
        else{
            return TYPE_OTHER_MESSAGE;
        }
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
        public TextView sender_title;
        public TextView message;
        public TextView message_time;

        OnNoteListener monNoteListener;


        // We also create a constructor that accepts the entire item row
        // and does the view lookups to find each subview
        public ViewHolder(View itemView, OnNoteListener onNoteListener) {
            // Stores the itemView in a public final member variable that can be used
            // to access the context from any ViewHolder instance.
            super(itemView);

            icon_avata = (CircleImageView) itemView.findViewById(R.id.icon_avata);
            sender_title = (TextView) itemView.findViewById(R.id.sender_title);
            message = (TextView) itemView.findViewById(R.id.message);
            message_time = (TextView) itemView.findViewById(R.id.message_time);

            monNoteListener = onNoteListener;

            itemView.setOnClickListener(this);
        }

        @Override
        public void onClick(View v) {
            Log.d("MessageSenderAdapter", "Note clicked");
            onNoteListener.onNoteClick(getAdapterPosition());
        }
    }

    public interface OnNoteListener{
        void onNoteClick(int position);
    }
}