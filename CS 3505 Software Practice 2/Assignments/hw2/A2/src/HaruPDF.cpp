// Aaron Templeton
// U0734119
// CS 3505
//A2: Classes and Facades


#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>
#include "hpdf.h"
#include <HaruPDF.h>

//create a Haru PDF
HaruPDF::HaruPDF(char name[256]){

  pdf = HPDF_New (NULL, NULL);
  /* add a new page object. */
  page = HPDF_AddPage (pdf);
  HPDF_Page_SetSize (page, HPDF_PAGE_SIZE_A5, HPDF_PAGE_PORTRAIT);
//    print_grid  (pdf, page);
  font = HPDF_GetFont (pdf, "Helvetica", NULL);
  HPDF_Page_SetTextLeading (page, 20);
  HPDF_Page_SetGrayStroke (page, 0);
  HPDF_Page_BeginText (page);
  font = HPDF_GetFont (pdf, "Courier-Bold", NULL);
  HPDF_Page_SetFontAndSize (page, font, 30);

strcpy(fname, name);
strcat(fname, ".pdf");

buf[1]=0;
}

//prints a haru pdf using haru functions
void HaruPDF::PrintPDF(char character, double x, double y, double angle){
  buf[0]=character;
  HPDF_Page_SetTextMatrix(page, cos(angle), sin(angle), -sin(angle), cos(angle), x, y);
  HPDF_Page_ShowText(page, buf);
}
//save a pdf using haru functions
void HaruPDF::SavePDF(){
  HPDF_Page_EndText(page);
  HPDF_SaveToFile(pdf, fname);
  HPDF_Free(pdf);
}
