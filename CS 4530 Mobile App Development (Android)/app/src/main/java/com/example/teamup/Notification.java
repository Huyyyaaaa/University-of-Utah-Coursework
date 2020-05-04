package com.example.teamup;

public class Notification {
    public String title;
    public String description;
    public String documentID;
    public String notificationSource; //options are: location, joinTeam

    public Notification(){

    }

    public String getTitle(){
        return title;
    }

    public String getDescription(){
        return description;
    }

    public String getDocumentID(){ return documentID;}

    public String getNotificationSource(){ return notificationSource;}
}
