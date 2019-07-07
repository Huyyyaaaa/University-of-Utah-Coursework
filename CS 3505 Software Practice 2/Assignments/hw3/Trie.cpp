/*****************************
Aaron Templeton
U0734119
CS 3505
A3: Trie and Rule of Three
****************************/


#include "Trie.h"
#include "Node.h"
#include <vector>
#include <string>


using namespace std;

Trie::Trie(){
  root = new Node();
}

//destructor
Trie::~Trie(){

  delete[] root;
}

//copy constructor
Trie::Trie(const Trie& other){
  root = other.root;
}

//assignment operator overload
Trie& Trie::operator=(Trie other){
  if(this != &other){
    delete[] root;
    root = new Node(*(other.root));
  }
  return *this;
}

//add a word to the Trie
void Trie::addAWord(std::string s){

  Node* current = root;
  for(unsigned int i = 0; i < s.length();i++){

    Node* last = current;

    if(i==s.length()-1){
      if(current->hasChar(s[i])!=nullptr){
        current->markValidWord();
        break;
      }
      else{
        current = last->addChar(s[i]);
        current->markValidWord();
        break;
      }
    }
      if((current=current->hasChar(s[i])) == nullptr){
        current = last->addChar(s[i]);
      }

  }
}

//checks if a word is in the Trie
 bool Trie::isAWord(std::string s){

   if(s == ""){
     return false;
   }

   Node* current = root;

   if(current != NULL){

     for(unsigned int i = 0; i < s.length(); i++){
       Node* temp = current->getBranch(s[i]);

       if(temp == NULL) return false;

       current = temp;
     }

     return (current->isValidWord());
   }
   return false;
 }

//gets the list of reccommened words in the Trie after given a word used as a prefix
 vector<std::string> Trie::allWordsStartingWithPrefix(std::string s){

    vector<std::string> list;

    Node* current = root;

    for(unsigned int i = 0; i < s.length(); i++){
      Node* temp = current->getBranch(s[i]);

      if(temp == NULL){
        return list;
      }

      current = temp;
    }

    traverse(s, current, list);
    return list;
 }

 //helper method to traverse through the Trie
 void Trie::traverse(string& prefix, Node* node, vector<std::string>& list){

    if(node == NULL){
      return;
    }

    if(node->isValidWord()) list.push_back(prefix);

    for(char i = 0; i <26; i++){
      char c = 97+i;
      Node* temp = node->getBranch(c);

      if(node != NULL){
          prefix.push_back(c);
          traverse(prefix, temp, list);
          prefix.pop_back();
      }
    }
 }
