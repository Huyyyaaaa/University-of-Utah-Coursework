// Aaron Templeton
// U0734119
// CS 3505
//A2: Classes and Facades

 #include "HaruPDF.h"
 #include "Spiral.h"
 #include <iostream>

//main method as in PDFexample
 int main(int argc, char **argv){

    double x = 225;
    double y = 300;
    double rad = 50;
    double angle = 180;

    if(argc < 2){
      std::cout << "Provide text for the spiral" << std::endl;
      return 0;
    }

    Spiral s(x, y, rad, angle);
    HaruPDF h(argv[0]);

    char text[256];
    strcpy(text, argv[1]);

    int length = strlen(text);
    for(int i = 0; i < length; i++){
        char c = text[i];
        s.changePos();
        h.PrintPDF(c, s.getTextX(), s.getTextY(), s.getTextAngle());
    }
    h.SavePDF();
    return 0;

 }
