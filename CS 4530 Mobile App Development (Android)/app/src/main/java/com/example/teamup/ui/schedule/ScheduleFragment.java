package com.example.teamup.ui.schedule;

import android.content.res.Resources;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.util.Pair;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;

import com.example.teamup.Course;
import com.example.teamup.R;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.QueryDocumentSnapshot;
import com.google.firebase.firestore.QuerySnapshot;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.List;

public class ScheduleFragment extends Fragment {

    private ScheduleViewModel scheduleViewModel;
    private String uid;
    private List<Course> courses = new ArrayList<>();
    private TextView mon_date, tues_date, wed_date, thurs_date, fri_date, month_date, year_date;
    private RelativeLayout mon, tues, wed, thurs, fri;
    private FirebaseFirestore db;
    private ProgressBar progressBar;
    private TextView noScheduleText;
    private int eventBgColor = 0;
    private String[] bgColors = {"#4287f5", "#f5a442", "#23a63b", "#5d209e", "#b51f38"};
    private HashMap<String, String> courseColors = new HashMap<>();
    private int navIndex = 0;
    private Button today_Btn;
    private Button next_Btn;
    private Button prev_Btn;
    private Calendar rightNow_Clndr;
    private Calendar nav_Clndr;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRetainInstance(true);
    }

    @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN_MR1)
    @Override
    public View onCreateView (@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {
        scheduleViewModel =
                ViewModelProviders.of(this).get(ScheduleViewModel.class);

        RelativeLayout.LayoutParams textviewparam = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.MATCH_PARENT, RelativeLayout.LayoutParams.WRAP_CONTENT);
        textviewparam.addRule(RelativeLayout.CENTER_HORIZONTAL);

        View root = inflater.inflate(R.layout.fragment_schedule, container, false);

        progressBar = root.findViewById(R.id.progressBarSchedule);
        noScheduleText = root.findViewById(R.id.textViewScheduleNone);
        noScheduleText.setVisibility(View.GONE);

        //get text views of days of week
        mon_date = root.findViewById(R.id.monday_Date_TextView);
        tues_date = root.findViewById(R.id.tuesday_Date_TextView);
        wed_date = root.findViewById(R.id.wednesday_Date_TextView);
        thurs_date = root.findViewById(R.id.thursday_Date_TextView);
        fri_date = root.findViewById(R.id.friday_Date_TextView);

        //get text views of month and year
        month_date = root.findViewById(R.id.current_Month_TextView);
        year_date = root.findViewById(R.id.current_Year_TextView);

        //get relative views of days of week
        mon = root.findViewById(R.id.mondayRelativeLayout);
        tues = root.findViewById(R.id.tuesdayRelativeLayout);
        wed = root.findViewById(R.id.wednesdayRelativeLayout);
        thurs = root.findViewById(R.id.thursdayRelativeLayout);
        fri = root.findViewById(R.id.fridayRelativeLayout);

        //get weekly navigation buttons
        today_Btn = root.findViewById(R.id.today_Button);
        next_Btn = root.findViewById(R.id.next_Button);
        prev_Btn = root.findViewById(R.id.previous_Button);

        setScheduleDate();

        db = FirebaseFirestore.getInstance();
        getUserEvents();

        today_Btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                navIndex = 0;
//                nav_Clndr.setTime(rightNow_Clndr.getTime());
                setScheduleDate();
            }
        });
        next_Btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                navIndex++;
                rightNow_Clndr.add(Calendar.WEEK_OF_MONTH, 1);
                setScheduleDate();
            }
        });
        prev_Btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                navIndex--;
                rightNow_Clndr.add(Calendar.WEEK_OF_MONTH, -1);
                setScheduleDate();
            }
        });

        return root;
    }

    private void setScheduleDate()
    {
        //limit excessive navigation calls
        if(navIndex==0){
            rightNow_Clndr = Calendar.getInstance();
            today_Btn.setEnabled(false);
            markPresentDay(true);
        }
        else if(navIndex == 1 || navIndex == -1) {
            today_Btn.setEnabled(true);
            markPresentDay(false);
        }

        nav_Clndr = (Calendar) rightNow_Clndr.clone();

        Calendar cal = nav_Clndr;

        SimpleDateFormat schedule_fmt = new SimpleDateFormat("EEE MMMM dd yyyy");
        SimpleDateFormat day_fmt = new SimpleDateFormat("dd");

        String currentTime = schedule_fmt.format(cal.getTime());
        String[] schedule_Arr = currentTime.split(" ");

        //Assign dates for the month and the year
        month_date.setText(schedule_Arr[1]);
        year_date.setText(schedule_Arr[3]);

        //Assign dates for the week
        cal.set(Calendar.DAY_OF_WEEK, Calendar.MONDAY);

        mon_date.setText(day_fmt.format(cal.getTime()));
        cal.add(Calendar.DATE, 1);
        tues_date.setText(day_fmt.format(cal.getTime()));
        cal.add(Calendar.DATE, 1);
        wed_date.setText(day_fmt.format(cal.getTime()));
        cal.add(Calendar.DATE, 1);
        thurs_date.setText(day_fmt.format(cal.getTime()));
        cal.add(Calendar.DATE, 1);
        fri_date.setText(day_fmt.format(cal.getTime()));
    }

    private void markPresentDay(Boolean present)
    {
        SimpleDateFormat dayofweek_fmt = new SimpleDateFormat("EEEE");
        String day = dayofweek_fmt.format(rightNow_Clndr.getTime());

        //edge case for weekend
        if(day.contains("Sat") || day.contains("Sun")) { return; }

        int text_Color = 0;
        Drawable text_BG = null;

        if (present) {
            text_Color = getResources().getColor(R.color.pureWhite);
            text_BG = getResources().getDrawable(R.drawable.rounded_textview);
        } else { text_Color = getResources().getColor(R.color.andriodBlack); }

        if(day.contains("Mon")) {
            mon_date.setTextColor(text_Color);
            mon_date.setBackground(text_BG);
        }
        else if(day.contains("Tues")) {
            tues_date.setTextColor(text_Color);
            tues_date.setBackground(text_BG);
        }
        else if(day.contains("Wed")) {
            wed_date.setTextColor(text_Color);
            wed_date.setBackground(text_BG);
        }
        else if(day.contains("Thurs")) {
            thurs_date.setTextColor(text_Color);
            thurs_date.setBackground(text_BG);
        }
        else if(day.contains("Fri")) {
            fri_date.setTextColor(text_Color);
            fri_date.setBackground(text_BG);
        }
    }


    private void getUserEvents ()
    {
        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();

        if (user != null) { uid = user.getUid(); }

        CollectionReference schoolsRef = db.collection("courses");
        schoolsRef.whereArrayContains("users", uid).get()
                .addOnCompleteListener(new OnCompleteListener<QuerySnapshot>() {
                    @Override
                    public void onComplete(@NonNull Task<QuerySnapshot> task) {
                        if(task.isSuccessful()){
                            if(!task.getResult().isEmpty()){
                                for(QueryDocumentSnapshot snapshot: task.getResult()){
                                    courses.add(snapshot.toObject(Course.class));
                                }
                                addEventsToView();
                            }else{
                                progressBar.setVisibility(View.GONE);
                                noScheduleText.setVisibility(View.VISIBLE);
                                Toast.makeText(getActivity(),
                                        "Add Courses to see them in schedule",
                                        Toast.LENGTH_SHORT).show();
                            }
                        }
                    }
                });
    }


    private void addEventsToView ()
    {
        progressBar.setVisibility(View.GONE);

        for(Course crs : courses) {
            ArrayList<String> days = crs.getDays();
            ArrayList<Pair<String, ViewGroup>> days_events = new ArrayList<Pair<String, ViewGroup>>();

            if(days.size() >= 1) {
                for(String day : days) {
                    days_events.add(new Pair<String, ViewGroup>(day, createEventView(crs)));
                }
            }

            if(!days_events.isEmpty()) {
                for (Pair<String, ViewGroup> de_Pair : days_events) {
                    if (de_Pair.first.contains("Mon")) { mon.addView(de_Pair.second); }
                    if (de_Pair.first.contains("Tues")) { tues.addView(de_Pair.second); }
                    if (de_Pair.first.contains("Wed")) { wed.addView(de_Pair.second); }
                    if (de_Pair.first.contains("Thurs")) { thurs.addView(de_Pair.second); }
                    if (de_Pair.first.contains("Fri")) { fri.addView(de_Pair.second); }
                }
                days_events.clear();
            }
        }
    }


    private ViewGroup createEventView (Course c)
    {
        View event_Vw =  LayoutInflater.from(getContext()).inflate(
                R.layout.event,null,false);
        ViewGroup event_VwGrp = (ViewGroup) event_Vw;

//        TextView event_Lbl = ((TextView) event_VwGrp.getChildAt(0));
//        event_Lbl.setText(c.getStart()+"\n"+c.getEnd());

        TextView event_Body = ((TextView) event_VwGrp.getChildAt(0));
        if(courseColors.containsKey(c.getName())){
            event_Body.setBackgroundColor(Color.parseColor(courseColors.get(c.getName())));
        }else{
            if(eventBgColor == 4) { eventBgColor = 0; }
            courseColors.put(c.getName(), bgColors[eventBgColor]);
            event_Body.setBackgroundColor(Color.parseColor(bgColors[eventBgColor]));
            eventBgColor++;

        }
        event_Body.setText(c.getName());
        event_Body.setTextColor(Color.WHITE);



        int duration = convertIntToPx(durationInMinutes(c.start, c.end));

        event_VwGrp.setLayoutParams(new ViewGroup.LayoutParams(mon.getWidth()-2, duration));
        //event_Body.setHeight(duration);
        //event_Body.setWidth(mon.getWidth()-2);

        event_VwGrp.setTranslationY(findEventPosition(c.getStart()));

        return event_VwGrp;
    }


    private int findEventPosition (String startTime)
    {
        SimpleDateFormat time_fmt = new SimpleDateFormat("hh:mm aa");
        String noon_Str = "12:00 PM";
        String hour_Str = "";
        int hourIndex = 0;
        int position = 60;

        try {
            int hour = Integer.parseInt(startTime.substring(0, 2));
            int mins = Integer.parseInt(startTime.substring(3, 5));
            hour_Str = String.valueOf(hour);
            Date noon_Dt = time_fmt.parse(noon_Str);
            Date start_Dt = time_fmt.parse(startTime);

            if(start_Dt.before(noon_Dt)) {
                switch (hour_Str) {
                    case "7":
                        hourIndex=0;
                        break;
                    case "8":
                        hourIndex=1;
                        break;
                    case "9":
                        hourIndex=2;
                        break;
                    case "10":
                        hourIndex=3;
                        break;
                    case "11":
                        hourIndex=4;
                        break;
                    default:
                        break;
                }
            }
            else { hourIndex = hour + 5; }

            Resources r = getContext().getResources();
            position = (int) TypedValue.applyDimension(
                    TypedValue.COMPLEX_UNIT_DIP, ((hourIndex*60)+mins), r.getDisplayMetrics()
            );

        } catch (ParseException e) { e.printStackTrace(); }

        return position;
    }


    private int convertIntToPx(int duration)
    {
        int timeViewHeight = 0;
        Resources r = getContext().getResources();

        int px = (int) TypedValue.applyDimension(
                TypedValue.COMPLEX_UNIT_DIP, duration-timeViewHeight, r.getDisplayMetrics()
        );

        return px;
    }


    public int durationInMinutes(String startTime, String endTime)
    {
        int duration = 0;
        SimpleDateFormat time_fmt = new SimpleDateFormat("hh:mm aa");

        try {
            Date start_Dt = time_fmt.parse(startTime);
            Date end_Dt = time_fmt.parse(endTime);
            long mills = end_Dt.getTime() - start_Dt.getTime();

            int hours = (int) (mills/(1000 * 60 * 60));
            int mins = (int) (mills/(1000*60)) % 60;
            duration = (hours * 60) + mins;

        } catch (ParseException e) { e.printStackTrace(); }

        return duration;
    }
}
