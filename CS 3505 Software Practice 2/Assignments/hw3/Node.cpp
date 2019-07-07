/*****************************
Aaron Templeton
U0734119
CS 3505
A3: Trie and Rule of Three
****************************/


#include "Node.h"

Node::Node(){
  endValidWord = false;
  for(int i = 0; i < 26; i++){
    nodeArray[i]=nullptr;
  }
}

Node::~Node(){
  delete &endValidWord;
  for(unsigned int i = 0; i < 26; i++){
    delete nodeArray[i];
  }
  delete &nodeArray;
}

//checks if the Node has the character
Node* Node::hasChar(char c){
int index = c -97;
return nodeArray[index];
}

//add character to a Node
Node* Node::addChar(char c){
  int index = c - 97;
  nodeArray[index] = new Node();
  return nodeArray[index];
}

//mark the node index as valid word end
void Node::markValidWord(){
  endValidWord = true;
}

bool Node::isValidWord(){
  return endValidWord;
}

//returns the branch of the node with given character
Node* Node::getBranch(char c){
  int index = c -97;
  return nodeArray[index];
}

//copy constructor
Node::Node(const Node& other){
  endValidWord = other.endValidWord;
    for(unsigned int i = 0; i < 26; i++){
      delete nodeArray[i];
      nodeArray[i] = other.nodeArray[i];
    }
}


//assignment operator overload
Node& Node::operator=(Node other){
  if(this != &other){
    endValidWord = other.endValidWord;
    for(int i =0; i < 26; i++){
      nodeArray[i] = other.nodeArray[i];
    }
  }
  return *this;
}
