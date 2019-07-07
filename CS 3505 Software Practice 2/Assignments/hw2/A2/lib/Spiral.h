// Aaron Templeton
// U0734119
// CS 3505
//A2: Classes and Facades


#include <iostream>
#include <math.h>


class Spiral{

  double CX;
  double CY;
  double x;
  double y;
  double startRad;
  double angle;
  double textAngle;
  int increment;


public:
  Spiral(double centerX, double centerY, double rad, double angle);
  void changePos();
  double getTextX();
  double getTextY();
  double getTextAngle();
  Spiral operator++ (int n);
  Spiral& operator++ ();
  friend std::ostream& operator<<(std::ostream&, const Spiral&);

};
