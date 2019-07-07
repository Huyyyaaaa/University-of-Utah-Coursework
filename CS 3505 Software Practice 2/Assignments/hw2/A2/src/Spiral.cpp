// Aaron Templeton
// U0734119
// CS 3505
//A2: Classes and Facades

#include "Spiral.h"

//create a spiral given center x and y position and radius
Spiral::Spiral(double centerX, double centerY, double rad, double angle){
  CX = centerX;
  CY = centerY;
  startRad = rad;
  angle = 180;
  textAngle = 180;
  increment = 1;

  if(startRad < 0){
    startRad = 30;
  }

}
//return the x position of spiral
  double Spiral::getTextX(){
    return x;
  }
  //return the y position of spiral
  double Spiral::getTextY(){
    return y;
  }
  //return the angle of the text
  double Spiral::getTextAngle(){
    return textAngle;
  }
//change the position of the spiral and text angle
void Spiral::changePos(){

  //angle and radius calculations from pdfExample convert to degrees
  textAngle = (angle -90)/180 * M_PI;
  double newRad = angle/180 * M_PI;
  //increment of radius
  double r = startRad + increment;
  increment++;

  //control spacing of characters
  angle -= 1000/r;
  //update of postion of text
  x = CX + cos(newRad) * r;
  y = CY +sin(newRad) * r;
}
  Spiral& Spiral::operator++(){
    this->changePos();
    return *this;
  }

  Spiral Spiral::operator++(int n){
    Spiral s(*this);
    ++(*this);
    return s;
  }

  std::ostream& operator<<(std::ostream& out, Spiral& spiral){
      out << "angle of text: "<< spiral.getTextAngle() << "x pos: "<<spiral.getTextX()<< "y pos: "<< spiral.getTextY();
      return out;
  }
