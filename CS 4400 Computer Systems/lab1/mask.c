#include <stdio.h>

int main(){
  unsigned int bits = 0xAABBCCDD;
  unsigned char MSB = bits >> 24;
  unsigned char shifted = bits >> 20;
  unsigned char mask = 0x3f;
  unsigned char ans = shifted & mask;

  printf("%x\n", ans);
  unsigned char ex = bits >> 16;

  return 0;



}
