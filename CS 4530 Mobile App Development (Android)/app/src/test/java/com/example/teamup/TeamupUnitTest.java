package com.example.teamup;

import android.app.Instrumentation;
import android.content.Context;

import com.example.teamup.ui.schedule.ScheduleFragment;
import com.example.teamup.ui.teams.TeamsFragment;

import org.junit.Test;

import java.util.ArrayList;
import java.util.List;

import static org.junit.Assert.*;

/**
 * Example local unit test, which will execute on the development machine (host).
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
public class TeamupUnitTest {
    @Test
    public void addition_isCorrect() {
        assertEquals(4, 2 + 2);
    }


    @Test
    public void testValidateCourseNameTrue() {
        AddCourseFragment fragment = new AddCourseFragment();
        assertEquals(true, fragment.validateCourseName("CS 2420"));
    }

    @Test
    public void testValidateCourseNameFalse() {
        AddCourseFragment fragment = new AddCourseFragment();
        assertEquals(false, fragment.validateCourseName("CS2420"));
    }

    @Test
    public void testValidateCourseNameNull() {
        AddCourseFragment fragment = new AddCourseFragment();
        assertEquals(false, fragment.validateCourseName(null));
    }

    @Test
    public void testDurationInMinutes(){
        ScheduleFragment fragment = new ScheduleFragment();
        assertEquals(1, fragment.durationInMinutes("1:00 PM", "1:01 PM"));
    }
    @Test
    public void testDurationInMinutesSameTime(){
        ScheduleFragment fragment = new ScheduleFragment();
        assertEquals(0, fragment.durationInMinutes("1:00 PM", "01:00 PM"));
    }
}