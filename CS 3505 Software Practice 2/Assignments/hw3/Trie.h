/*****************************
Aaron Templeton
U0734119
CS 3505
A3: Trie and Rule of Three
****************************/


#ifndef TRIE_H
#define TRIE_H

#include <iostream>
#include <vector>
#include <string>
#include "Node.h"

using namespace std;

class Trie{
public:
    Node* root;
  Trie();
  ~Trie();
  Trie(const Trie&);
  Trie& operator=(Trie);
  void addAWord(std::string);
  bool  isAWord(std::string);
  vector<std::string> allWordsStartingWithPrefix(std::string);
  void traverse(string&, Node*, vector<std::string>&);
};
#endif
