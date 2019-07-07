// Aaron Templeton
// U0734119
// CS 3505
//A2: Classes and Facades 

#include <stdlib.h>
#include <stdio.h>
#include<iostream>
#include<string>
#include "hpdf.h"
#include <string.h>
#include <math.h>

//class for haruPDF
class HaruPDF
{
  //lib haru fields used to create a pdf
  HPDF_Doc  pdf;
  HPDF_Page page;
  char fname[256];
  HPDF_Font font;
  float angle2;
  float rad1;
  float rad2;
  unsigned int i;
  const char* TEXT;
  char buf[2];

public:
  HaruPDF(char name[256]);
  void PrintPDF(char character, double x, double y, double angle);
  void SavePDF();
};
