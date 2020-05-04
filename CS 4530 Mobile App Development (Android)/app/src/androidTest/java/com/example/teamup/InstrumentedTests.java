package com.example.teamup;

import android.content.Context;

import androidx.test.espresso.action.CloseKeyboardAction;
import androidx.test.platform.app.InstrumentationRegistry;
import androidx.test.ext.junit.runners.AndroidJUnit4;

import org.junit.Rule;
import org.junit.Test;
import org.junit.runner.RunWith;

import static androidx.test.espresso.Espresso.onData;
import static androidx.test.espresso.Espresso.onView;
import static androidx.test.espresso.action.ViewActions.click;
import static androidx.test.espresso.action.ViewActions.typeText;
import static androidx.test.espresso.assertion.ViewAssertions.matches;
import static androidx.test.espresso.matcher.ViewMatchers.withId;
import static androidx.test.espresso.matcher.ViewMatchers.withText;
import static junit.framework.TestCase.assertEquals;
import static org.junit.Assert.*;

import 	androidx.test.*;
import androidx.test.rule.ActivityTestRule;

/**
 * Instrumented test, which will execute on an Android device.
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
@RunWith(AndroidJUnit4.class)
public class InstrumentedTests {

    @Rule
    public ActivityTestRule<MainActivity> mainActivityTestRule = new ActivityTestRule<>(MainActivity.class);

    @Test
    public void loginMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.emailText)).perform(typeText("j@j.com"));
        onView(withId(R.id.paswordLoginText)).perform(typeText("password\n"));
        onView(withId(R.id.loginBtn)).perform(click());
        Thread.sleep(10 * 1000);
        onView(withId(R.id.toolbar_text)).check(matches(withText("Teams")));
    }

    @Test
    public void registerMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.registerText)).perform(click());
        Thread.sleep(5 * 1000);
        onView(withId(R.id.textView2)).check(matches(withText("Register")));
    }

    @Test
    public void checkScheduleMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.emailText)).perform(typeText("j@j.com"));
        onView(withId(R.id.paswordLoginText)).perform(typeText("password\n"));
        onView(withId(R.id.loginBtn)).perform(click());
        Thread.sleep(10 * 1000);
        onView(withId(R.id.navigation_schedule)).perform(click());
        onView(withId(R.id.btnCourses)).check(matches(withText("View Courses")));
    }

    @Test
    public void addClassMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.emailText)).perform(typeText("j@j.com"));
        onView(withId(R.id.paswordLoginText)).perform(typeText("password\n"));
        onView(withId(R.id.loginBtn)).perform(click());
        Thread.sleep(15 * 1000);
        onView(withId(R.id.navigation_schedule)).perform(click());
        Thread.sleep(2 * 1000);
        onView(withId(R.id.btnCourses)).perform(click());
        Thread.sleep(2 * 1000);
        onView(withId(R.id.btnAddCourse)).perform(click());
        Thread.sleep(2 * 1000);
        onView(withId(R.id.addCourseText)).check(matches(withText("Add Course")));
    }

    @Test
    public void viewNotificationsMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.emailText)).perform(typeText("j@j.com"));
        onView(withId(R.id.paswordLoginText)).perform(typeText("password\n"));
        onView(withId(R.id.loginBtn)).perform(click());
        Thread.sleep(15 * 1000);
        onView(withId(R.id.navigation_notifications)).perform(click());
        Thread.sleep(2 * 1000);
        onView(withId(R.id.toolbar_text)).check(matches(withText("Notifications")));
    }

    @Test
    public void checkMessagesMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.emailText)).perform(typeText("j@j.com"));
        onView(withId(R.id.paswordLoginText)).perform(typeText("password\n"));
        onView(withId(R.id.loginBtn)).perform(click());
        Thread.sleep(15 * 1000);
        onView(withId(R.id.navigation_messages)).perform(click());
        Thread.sleep(2 * 1000);
        onView(withId(R.id.toolbar_text)).check(matches(withText("Messages")));
    }

    @Test
    public void createTeamMakesCorrectIntent() throws InterruptedException {
        onView(withId(R.id.emailText)).perform(typeText("j@j.com"));
        onView(withId(R.id.paswordLoginText)).perform(typeText("password\n"));
        onView(withId(R.id.loginBtn)).perform(click());
        Thread.sleep(15 * 1000);
        onView(withId(R.id.plusButton)).perform(click());
        Thread.sleep(2 * 1000);
        onView(withId(R.id.textView7)).check(matches(withText("Create Team")));
    }

}

