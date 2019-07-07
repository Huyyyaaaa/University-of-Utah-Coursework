/*****************************************************************
*	Lance Atkinson
*	CS 3505 - Software Practice II
*	Assignment 3 - A Trie and the Rule of Three
*****************************************************************/
#ifndef TRIE_H
#define TRIE_H
#include <string>
#include <vector>

class Node
{
	//FIELDS
	private:
		Node* branches[26]; //one element for each letter of the alphabet
	
	public:
		bool isValidWord;
	
	//METHODS / CONSTRUCTORS
	public:
		Node();
		
		//This array index operator returns the address of the element for you to assign something to it.  ('a' maps to index 0, 'b' maps to 1 etc.)
		Node** operator[](char letter); 
		
		//This array index operator returns value of what's in the index ('a' maps to index 0, 'b' maps to 1 etc.)
		Node* operator[](char letter) const;
};
	
class Trie
{
	//FIELDS
	private:
		Node root;
	
	//METHODS / CONSTRUCTORS
	public:
		Trie();
		~Trie();
		Trie(const Trie& rhs); //Copy Constructor
		Trie& operator=(const Trie rhs); //Assignment Operator Overload
		
		//Adds a word to the dictionary
		void addAWord(std::string word);
		
		//Searches the tree for a given word. Returns true if it contains this words, false otherwise.
		bool isAWord(std::string word);
		
		//Returns a vector listing all words in the tree that start with a certain prefix
		//An empty string of a prefix will return all words in the tree
		std::vector<std::string> allWordsStartingWithPrefix(std::string prefix); 
		
		//Dummy method. Only for extra credit. Not implemented.
		std::vector<std::string> wordsWithWildcardPrefix(std::string wildCard);
		
	private: //HELPER METHODS
	
		//This is a helper method for allWordsStartingWithPrefix. It adds words to a vector 
		//as it finds them while recursively navigating the tree
		void traverseBranches(Node* currentNode, std::vector<std::string>& words, std::string prefix);
		
		//This is used by the destructor to delete nodes on the heap. It recursively 
		//traverses the tree and deletes the leaves first to avoid memory-leaks.
		void deleteNodes(Node* nodePtr);
		
		//This is a helper method for the copy constructor. 
		//It recursively adds nodes to the new tree.
		void copyNodes(const Node* otherNode, Node* thisNode);
};
#endif