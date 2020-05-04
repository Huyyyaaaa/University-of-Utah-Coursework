package com.example.teamup;

import java.util.ArrayList;

public class Course {
    //public String CourseID;
    public String schoolId;
    public String name;
    public String start;
    public String end;
    public ArrayList<String> days;
    public String semester;
    public ArrayList<String> users;

    public Course(){}
//    public Course(String schoolId, String name, String start, String end, ArrayList<String> days,
//                  String semester, ArrayList<String> users) {
//        this.schoolId = schoolId;
//        this.name = name;
//        this.start = start;
//        this.end = end;
//        this.days = days;
//        this.semester = semester;
//        this.users = users;
//    }

//    public String getCourseID(){
//        return CourseID;
//    }

    public String getSchoolId() {
        return schoolId;
    }

    public String getName() {
        return name;
    }

    public String getStart() {
        return start;
    }

    public String getEnd() {
        return end;
    }

    public ArrayList<String> getDays() {
        return days;
    }

    public String getSemester() {
        return semester;
    }

    public ArrayList<String> getUsers() {
        return users;
    }
}
