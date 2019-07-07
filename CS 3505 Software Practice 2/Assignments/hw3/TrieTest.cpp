/*****************************
Aaron Templeton
U0734119
CS 3505
A3: Trie and Rule of Three
****************************/

#include <iostream>
#include <vector>
#include <string>
#include <fstream>

#include "Trie.h"
#include "Node.h"

using namespace std;

int main(int argc, char* argv[]){

  //check for the correct number of arguments
  if(argc != 3){
    std::cout << "not enough arguments given. Please give 2 filenames."<< std::endl;
    return 0;
  }

  string file1 = argv[1];
  string file2 = argv[2];
  string input = "";
  ifstream fileStream;
  Trie* trie = new Trie();

  //open first file and add words from file to Trie
  fileStream.open(file1);
  if(fileStream.is_open()){
    while(getline(fileStream, input)){
      trie->addAWord(input);
    }
  }
  else{
    std::cout << "unable to read first file given. please check that the file exists and the correct path is given." << std::endl;
    return 0;
  }

  fileStream.close();

  //open second file and query the Trie to see if the words are in the Trie
  fileStream.open(file2);
  if(fileStream.is_open()){
    while(getline(fileStream, input)){

      if(trie->isAWord(input)){
        cout << input << " is found." << endl;
      }
      else{

        vector<string> list = trie->allWordsStartingWithPrefix(input);

        if(list.size() != 0){
          cout << input << " was not found, did you mean " << endl;

          for(string & word: list){
            cout << "   " << word << endl;
          }
        }else{
          cout << input << " was not found\n   no alternatives were found"<<endl;
        }

      }
    }
  }else{
    cout << "Error reading the second file. please make sure the file exists and the correct path was given"<<endl;
  }



}
