/*******************************************
 * Solutions for the CS:APP Performance Lab
 ********************************************/

#include <stdio.h>
#include <stdlib.h>
#include "defs.h"

/* 
 * Please fill in the following student struct 
 */
student_t student = {
  "Aaron Templeton",     /* Full name */
  "u0734119@utah.edu",  /* Email address */
};

/***************
 * COMPLEX KERNEL
 ***************/

/******************************************************
 * Your different versions of the complex kernel go here
  ******************************************************/


char first_complex_descr[] = "first_complex:first edit of Naive baseline implementation";
void first_complex(int dim, pixel *src, pixel *dest)
{
  int i, j;

  for(i = 0; i < dim; i++)
    for(j = 0; j < dim; j++)
    {
      pixel source = src[RIDX(i,j,dim)];
      int complex = (int)(source.red +source.green +source.blue)/3;
     int d =RIDX(dim-j-1,dim-i-1,dim);
      
     dest[d].red = complex;

      dest[d].green = complex;
      
      dest[d].blue = complex;

    }
}


char tiled_complex_descr[] = "tiled_complex:tiled edit of Naive baseline implementation";
void tiled_complex(int dim, pixel *src, pixel *dest)
{
  int i, j, ii, jj, W;

  if(dim >= 256){
    W=32;
   }else
  W = 16;

  for(i = 0; i < dim; i+=W)
    {
      int dim2 = dim -1;

    for(j = 0; j < dim; j+=W)
    {
      for(ii = i; ii<i+W; ii++)
	{
	  int dim3 = dim2 - ii;

	  for(jj = j; jj<j+W; jj++)
	    {
	      
	      pixel source = src[RIDX(jj,ii,dim)];
	      int complex = (int)(source.red +source.green +source.blue)/3;
	      int d =RIDX(dim3, dim2 -jj ,dim);
      
	      dest[d].red = complex;

	      dest[d].green = complex;
      
	      dest[d].blue = complex;
	    }
	}
      
    }
   }
}


/* naive_complex - The naive baseline version of complex 
 */
char naive_complex_descr[] = "naive_complex: Naive baseline implementation";
void naive_complex(int dim, pixel *src, pixel *dest)
{
  int i, j;

  for(i = 0; i < dim; i++)
    for(j = 0; j < dim; j++)
    {

      dest[RIDX(dim - j - 1, dim - i - 1, dim)].red = ((int)src[RIDX(i, j, dim)].red +
						      (int)src[RIDX(i, j, dim)].green +
						      (int)src[RIDX(i, j, dim)].blue) / 3;
      
      dest[RIDX(dim - j - 1, dim - i - 1, dim)].green = ((int)src[RIDX(i, j, dim)].red +
							(int)src[RIDX(i, j, dim)].green +
							(int)src[RIDX(i, j, dim)].blue) / 3;
      
      dest[RIDX(dim - j - 1, dim - i - 1, dim)].blue = ((int)src[RIDX(i, j, dim)].red +
						       (int)src[RIDX(i, j, dim)].green +
						       (int)src[RIDX(i, j, dim)].blue) / 3;

    }
}


/* 
 * complex - Your current working version of complex
 * IMPORTANT: This is the version you will be graded on
 */
char complex_descr[] = "complex: Current working version";
void complex(int dim, pixel *src, pixel *dest)
{
  tiled_complex(dim, src, dest);
}

/*********************************************************************
 * register_complex_functions - Register all of your different versions
 *     of the complex kernel with the driver by calling the
 *     add_complex_function() for each test function. When you run the
 *     driver program, it will test and report the performance of each
 *     registered test function.  
 *********************************************************************/

void register_complex_functions() {
  add_complex_function(&complex, complex_descr);
  add_complex_function(&naive_complex, naive_complex_descr);
}


/***************
 * MOTION KERNEL
 **************/

/***************************************************************
 * Various helper functions for the motion kernel
 * You may modify these or add new ones any way you like.
 **************************************************************/


/* 
 * weighted_combo - Returns new pixel value at (i,j) 
 */
static pixel weighted_combo(int dim, int i, int j, pixel *src) 
{
  int ii, jj;
  pixel current_pixel;

  int red, green, blue;
  red = green = blue = 0;

  int num_neighbors = 0;
  for(ii=0; ii < 3; ii++)
    for(jj=0; jj < 3; jj++) 
      if ((i + ii < dim) && (j + jj < dim)) 
      {
	num_neighbors++;
	red += (int) src[RIDX(i+ii,j+jj,dim)].red;
	green += (int) src[RIDX(i+ii,j+jj,dim)].green;
	blue += (int) src[RIDX(i+ii,j+jj,dim)].blue;
      }
  
  current_pixel.red = (unsigned short) (red / num_neighbors);
  current_pixel.green = (unsigned short) (green / num_neighbors);
  current_pixel.blue = (unsigned short) (blue / num_neighbors);
  
  return current_pixel;
  }
  __attribute__((always_inline)) static pixel full_combo(int i, int j, int dim, pixel *src)
  {
    pixel current_pixel;
    int red, green, blue;
    red = green = blue = 0;

    int row1 = i*dim;
    int row1j = row1+j;
    int row1j1 = row1+j+1;
    int row1j2 = row1+j+2;

    int row2 = (i+1)*dim;
    int row2j = row2+j;
    int row2j1 = row2+j+1;
    int row2j2 = row2+j+2;

    int row3 = (i+2)*dim;
    int row3j = row3+j;
    int row3j1 = row3+j+1;
    int row3j2 = row3+j+2;
    
    //get source colors
    red += (int) src[row1j].red;
    blue += (int) src [row1j].blue;
    green += (int) src[row1j].green;

    //get first row
    red += (int) src[row1j1].red;
    blue += (int) src[row1j1].blue;
    green += (int) src[row1j1].green;
    red += (int) src[row1j2].red;
    blue += (int) src[row1j2].blue;
    green += (int) src[row1j2].green;

    //get second row colors
    red += (int) src[row2j].red;
    blue += (int) src[row2j].blue;
    green += (int) src[row2j].green;
        red += (int) src[row2j1].red;
    blue += (int) src[row2j1].blue;
    green += (int) src[row2j1].green;
        red += (int) src[row2j2].red;
    blue += (int) src[row2j2].blue;
    green += (int) src[row2j2].green;

    //get third row colors
        red += (int) src[row3j].red;
    blue += (int) src[row3j].blue;
    green += (int) src[row3j].green;
     red += (int) src[row3j1].red;
    blue += (int) src[row3j1].blue;
    green += (int) src[row3j1].green;
     red += (int) src[row3j2].red;
    blue += (int) src[row3j2].blue;
    green += (int) src[row3j2].green;

    //return new pixel color
    current_pixel.red = (unsigned short)(red/9);
    current_pixel.blue = (unsigned short)(blue/9);
    current_pixel.green = (unsigned short)(green/9);
    return current_pixel;

  }

__attribute__((always_inline)) static pixel right_almost_edge(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

   int row1 = i*dim;
    int row1j = row1+j;
   int  row1j1 = row1+j+1;

    int row2 = (i+1)*dim;
   int  row2j = row2+j;
    int row2j1 = row2+j+1;

    int row3 = (i+2)*dim;
    int row3j = row3+j;
    int row3j1 = row3+j+1;
    

  //get source colors
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //first row colors

  red += (int) src[row1j1].red;
  blue += (int) src[row1j1].blue;
  green += (int) src[row1j1].green;

  //second row colors
  
  red += (int) src[row2j].red;
  blue += (int) src[row2j].blue;
  green += (int) src[row2j].green;
  
  red += (int) src[row2j1].red;
  blue += (int) src[row2j1].blue;
  green += (int) src[row2j1].green;

  //third row colors
  
  red += (int) src[row3j].red;
  blue += (int) src[row3j].blue;
  green += (int) src[row3j].green;
  
  red += (int) src[row3j1].red;
  blue += (int) src[row3j1].blue;
  green += (int) src[row3j1].green;

  //return current pixel new colors
  current_pixel.red = (unsigned short)(red/6);
  current_pixel.blue = (unsigned short)(blue/6);
  current_pixel.green = (unsigned short)(green/6);
  return current_pixel;

}

__attribute__((always_inline)) static pixel right_edge(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, blue, green;
  red=blue=green=0;

  int row1 = i*dim;
    int row1j = row1+j;
 

    int row2 = (i+1)*dim;
    int row2j = row2+j;
   

    int row3 = (i+2)*dim;
    int row3j = row3+j;
    
  
  //get pixel source colors 
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //get second row colors  
  red += (int) src[row2j].red;
  blue += (int) src[row2j].blue;
  green += (int) src[row2j].green;

  //get third row  
  red += (int) src[row3j].red;
  blue += (int) src[row3j].blue;
  green += (int) src[row3j].green;

  //get new pixel colors
  current_pixel.red = (unsigned short)(red/3);
  current_pixel.blue = (unsigned short)(blue/3);
  current_pixel.green = (unsigned short)(green/3);
  return current_pixel;

}

__attribute__((always_inline)) static pixel second_bottom_edge(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

    int row1 = i*dim;
    int row1j = row1+j;
    int row1j1 = row1+j+1;
    int row1j2 = row1+j+2;

    int row2 = (i+1)*dim;
    int row2j = row2+j;
    int row2j1 = row2+j+1;
    int row2j2 = row2+j+2;


  //get source colors  
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //get first row
    red += (int) src[row1j1].red;
  blue += (int) src[row1j1].blue;
  green += (int) src[row1j1].green;
   red += (int) src[row1j2].red;
  blue += (int) src[row1j2].blue;
  green += (int) src[row1j2].green;

  //get second row
  red += (int) src[row2j].red;
  blue += (int) src[row2j].blue;
  green += (int) src[row2j].green;
  red += (int) src[row2j1].red;
  blue += (int) src[row2j1].blue;
  green += (int) src[row2j1].green;
   red += (int) src[row2j2].red;
  blue += (int) src[row2j2].blue;
  green += (int) src[row2j2].green;

  //return new pixel colors
  current_pixel.red = (unsigned short)(red/6);
  current_pixel.blue = (unsigned short)(blue/6);
  current_pixel.green = (unsigned short)(green/6);
  return current_pixel;



}

__attribute__((always_inline)) static pixel bottom_edge(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

   int row1 = i*dim;
    int row1j = row1+j;
    int row1j1 = row1+j+1;
    int row1j2 = row1+j+2;


  //get source colors
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;
  
  //get row colors
  red += (int) src[row1j1].red;
  blue += (int) src[row1j1].blue;
  green += (int) src[row1j1].green;
  red += (int) src[row1j2].red;
  blue += (int) src[row1j2].blue;
  green += (int) src[row1j2].green;


  //return new colors
  current_pixel.red = (unsigned short)(red/3);
  current_pixel.blue = (unsigned short)(blue/3);
  current_pixel.green = (unsigned short)(green/3);
  return current_pixel;
}

__attribute__((always_inline)) static pixel right_inner_corner(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

   int row1 = i*dim;
    int row1j = row1+j;
    int row1j1 = row1+j+1;

  int row2 = (i+1)*dim;
  int row2j = row2+j;
  int row2j1 = row2+j+1;

  //get source
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //get first row
  red += (int) src[row1j1].red;
  blue += (int) src[row1j1].blue;
  green += (int) src[row1j1].green;

  //get second row
  red += (int) src[row2j].red;
  blue += (int) src[row2j].blue;
  green += (int) src[row2j].green;
  red += (int) src[row2j1].red;
  blue += (int) src[row2j1].blue;
  green += (int) src[row2j1].green;

  //return pixel
  current_pixel.red = (unsigned short)(red/4);
  current_pixel.blue = (unsigned short)(blue/4);
  current_pixel.green = (unsigned short)(green/4);
  return current_pixel;

}

__attribute__((always_inline)) static pixel right_small_edge(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

    int row1 = i*dim;
    int row1j = row1+j;

  int row2 = (i+1)*dim;
  int row2j = row2+j;

  //get source colors
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //get second row
  red += (int) src[row2j].red;
  blue += (int) src[row2j].blue;
  green += (int) src[row2j].green;

  //return new colors
  current_pixel.red = (unsigned short)(red/2);
  current_pixel.blue = (unsigned short)(blue/2);
  current_pixel.green = (unsigned short)(green/2);
  return current_pixel;
}

__attribute__((always_inline)) static pixel bottom_small_edge(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

  int row1 = i*dim;
    int row1j = row1+j;
    int row1j1 = row1+j+1;



  //get source colors
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //get first row
  red += (int) src[row1j1].red;
  blue += (int) src[row1j1].blue;
  green += (int) src[row1j1].green;

  //return new colors
  current_pixel.red = (unsigned short)(red/2);
  current_pixel.blue = (unsigned short)(blue/2);
  current_pixel.green = (unsigned short)(green/2);
  return current_pixel;
}

__attribute__((always_inline)) static pixel right_corner(int i, int j, int dim, pixel *src)
{
  pixel current_pixel;
  int red, green, blue;
  red = green = blue = 0;

    int row1 = i*dim;
    int row1j = row1+j;


  //get source
  red += (int) src[row1j].red;
  blue += (int) src[row1j].blue;
  green += (int) src[row1j].green;

  //return new 
  current_pixel.red = (unsigned short)(red);
  current_pixel.blue = (unsigned short)(blue);
  current_pixel.green = (unsigned short)(green);
  return current_pixel;
}
/******************************************************
 * Your different versions of the motion kernel go here
 ******************************************************/

char motion1_descr[] = "motion1: edit 1 of baseline implementation";
void motion1(int dim, pixel *src, pixel *dst) 
{
  int i, j, k;
    
  for (i = 0; i < dim-2; i++){
    for (j = 0; j < dim-2; j++){
      dst[RIDX(i,j,dim)] = full_combo(i,j,dim,src);
    }
   
    dst[RIDX(i,j,dim)] = right_almost_edge(i,j,dim,src);
    dst[RIDX(i,j+1,dim)] = right_edge(i,j+1,dim,src);
  }
  
  for(k=0;k<dim-2;k++){
    dst[RIDX(i,k,dim)] = second_bottom_edge(i,k,dim,src);
    dst[RIDX(i+1,k,dim)] = bottom_edge(i+1,k,dim,src);
  }

dst[RIDX(i, j, dim)] = right_inner_corner(i,j,dim,src);
dst[RIDX(i,j+1, dim)] = right_small_edge(i,j+1,dim,src);
dst[RIDX(i+1,j,dim)] = bottom_small_edge(i+1,j,dim,src);
dst[RIDX(i+1, j+1, dim)] = right_corner(i+1,j+1,dim,src);

}


/*
 * naive_motion - The naive baseline version of motion 
 */
char naive_motion_descr[] = "naive_motion: Naive baseline implementation";
void naive_motion(int dim, pixel *src, pixel *dst) 
{
  int i, j;
    
  for (i = 0; i < dim; i++)
    for (j = 0; j < dim; j++)
      dst[RIDX(i, j, dim)] = weighted_combo(dim, i, j, src);
}


/*
 * motion - Your current working version of motion. 
 * IMPORTANT: This is the version you will be graded on
 */
char motion_descr[] = "motion: Current working version";
void motion(int dim, pixel *src, pixel *dst) 
{
  motion1(dim, src, dst);
}

/********************************************************************* 
 * register_motion_functions - Register all of your different versions
 *     of the motion kernel with the driver by calling the
 *     add_motion_function() for each test function.  When you run the
 *     driver program, it will test and report the performance of each
 *     registered test function.  
 *********************************************************************/

void register_motion_functions() {
  add_motion_function(&motion, motion_descr);
  add_motion_function(&naive_motion, naive_motion_descr);
}
