/*****************************
Aaron Templeton
U0734119
CS 3505
A3: Trie and Rule of Three
****************************/

#ifndef NODE_H
#define NODE_H

class Node{
  Node *nodeArray[26];
  bool endValidWord;

public:
  Node();
  ~Node();
  Node* hasChar(char);
  Node* addChar(char);
  void markValidWord();
  bool isValidWord();
  Node* getBranch(char);
  Node(const Node&);
  Node& operator=(Node);
};
#endif
