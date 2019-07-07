#include <iostream>
#include <stdlib.h>
using namespace std;


class Cat{
  int lives;
public:

  Cat(int startLives) : lives{startLives}{}

  void reportLives(){
    cout << "number lives: " << lives <<endl;
  }

  void loseLife(){
    lives--;
  }
};

int main(){
  Cat mittens(9);
  mittens.reportLives();
  mittens.loseLife();
  mittens.reportLives();
}
