int main(){
  int x,y,result;
  x = 5;
  switch(x) {
  case 5:
  case 7:
    result = 2 * y;
    break;
  case 8:
    result += y - 4;
    break;
  case 9:
  case 10:
    result += 8;
    break;
  default:
    result = y;
  }
}
