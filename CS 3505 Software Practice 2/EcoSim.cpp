#include <iostream>
#include <stdlib.h>
#include <math.h>
using namespace std;



//prints the number of spaces and the character
void plotCharacterAtPosition(int number, char c){
  //output through standard out number - 1 spaces followed by the char
  int space = number -1;

    for(int i =1; i<space; i++){
      cout<<' ';
    }
    cout<<c;

}

//charts the populations
void plotPopulations(double rabbits, double foxes, double scale){
  int positionR = (rabbits * scale) + 1;
  int positionF = (foxes * scale)+1;
  if(positionF == positionR){
    plotCharacterAtPosition(positionR, '*');

  }else if(positionR > positionF){
    plotCharacterAtPosition(positionF, 'F');
    plotCharacterAtPosition(positionR-positionF, 'r');
  }else{
    plotCharacterAtPosition(positionR, 'r');
    plotCharacterAtPosition(positionF-positionR, 'F');
  }
  cout<<endl;
}

//update the populations
void updatePopulations(double g, double p, double c, double m, double K, double& numRabbits, double& numFoxes){

    double deltaRabbits = (g*numRabbits*(1-(numRabbits/K))) - (p*numRabbits*numFoxes);
    double deltaFoxes = (c*p*numRabbits*numFoxes)-(m*numFoxes);
    numRabbits += deltaRabbits;
    numFoxes += deltaFoxes;


 }

void incrementCounter(int* n){
  *n += 1;
}

main(){
  //rabbit growth
  double growth = 0.2;
  //preadation rate
  double pred = 0.002;
  //fox prey conversion
  double conv = 0.6;
  //fox mortality rate
  double mort = 0.2;
  //carry capacity
  double Kap = 1000.0;
  //number of Rabbits
  double numberRabbits;
  //number of Foxes
  double numberFoxes;
  //ask user for populations
  cout << "Enter number of rabbits: ";
  cin >> numberRabbits;
  if(!numberRabbits){
    cout << "Invalid entry for number of rabbits. Must be number" <<endl;
    exit(0);
  }

  cout << "Enter number of foxes: ";
  cin >> numberFoxes;
  if(!numberFoxes){
    cout << "invalid entry for foxes. must be number"<<endl;
  }
  //loop 500 times or until predator or prey is below 1
  for(int i =0; i<=500;){
    if(numberRabbits < 1 || numberFoxes < 1){
      exit(0);
    }else{
    plotPopulations(numberRabbits, numberFoxes, 0.1);
    updatePopulations(growth, pred, conv, mort, Kap, numberRabbits, numberFoxes);

  }
  incrementCounter(&i);
  cout<<i;
  }

}
