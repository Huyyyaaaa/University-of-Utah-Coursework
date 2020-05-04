package com.example.teamup;

import android.Manifest;
import android.content.Context;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Build;
import android.os.Bundle;
import android.widget.Toast;

import androidx.annotation.RequiresApi;

public class GPSTracker implements LocationListener {
    Context context;

    public GPSTracker(Context c){
        context = c;
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    public Location getLocation(){
        if(context.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED){
            Toast.makeText(context, "Please give permission to access location.", Toast.LENGTH_LONG).show();
            return null;
        }
        LocationManager manager = (LocationManager)context.getSystemService(Context.LOCATION_SERVICE);
        boolean gpsEnabled = manager.isProviderEnabled(LocationManager.GPS_PROVIDER);
        if(!gpsEnabled){
            Toast.makeText(context, "GPS is not enabled on this device", Toast.LENGTH_LONG).show();
            return null;
        }
        manager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 6000, 10, this);
        Location location = manager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
        return location;
    }

    @Override
    public void onLocationChanged(Location location) {
        //Function not necessary for our purposes.
    }

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {
        //Function not necessary for our purposes.
    }

    @Override
    public void onProviderEnabled(String provider) {
        //Function not necessary for our purposes.
    }

    @Override
    public void onProviderDisabled(String provider) {
        //Function not necessary for our purposes.
    }
}
