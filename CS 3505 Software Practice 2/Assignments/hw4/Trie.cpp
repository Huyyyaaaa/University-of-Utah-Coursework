/*****************************************************************
*	Lance Atkinson
* Aaron Templeton
*	CS 3505 - Software Practice II
*	Assignment 4 - Travis and Refactor
*****************************************************************/
#include "Trie.h"
#include <string.h>

//NODE IMPLEMENTATION
Node::Node()
{
	isValidWord = false;
	for(int i = 0; i < 26; i++)
		branches[i] = nullptr; //removing garbage data
}

Node** Node::operator[](char letter)
{
	return &branches[letter - 'a']; //turns the index of a to 0, b to 1 etc.
}

Node* Node::operator[](char letter) const
{
	return branches[letter - 'a']; //turns the index of a to 0, b to 1 etc.
}


//TRIE IMPLEMENTATION
Trie::Trie()
{
	//Implementation not needed; simply creates an empty dictionary
}

Trie::~Trie()
{
	deleteNodes(&root);
}

void Trie::deleteNodes(Node* nodePtr)
{
	//look at all of the non-null pointers and delete their data recursively
	//delete leaves first to avoid data leaks
	for(char c = 'a'; c <= 'z'; c++)
	{
		if(*(*nodePtr)[c] != nullptr)
		{
			Node* nextNode = *((*nodePtr)[c]);
			deleteNodes(nextNode);
			delete nextNode;
		}
	}
}

//Copy Constructor
Trie::Trie(const Trie& rhs)
{
	copyNodes(&(rhs.root), &root);
}

void Trie::copyNodes(const Node* otherNode, Node* thisNode)
{
	for(char c = 'a'; c <= 'z'; c++) //look at all non-null pointers
	{
		if((*otherNode)[c] != nullptr) //if this letter is in the old Trie, put it in the new one
		{
			*(*thisNode)[c] = new Node();
			(*(*thisNode)[c])->isValidWord = (*otherNode)[c]->isValidWord; //if old node is an end node, the new one will be too

			const Node* nextOtherNode = (*otherNode)[c];
			Node* nextThisNode = *(*thisNode)[c];

			copyNodes(nextOtherNode, nextThisNode); //recurse
		}
	}
}

//Assignment Operator Overload
Trie& Trie::operator=(Trie rhs)
{
	std::swap(this->root, rhs.root);
	return *this;
}

void Trie::addAWord(std::string word)
{
	Node* currentNode = &root;
	int wordLength = word.length();
	if(wordLength == 0)
		return; //do not add empty strings
	
	for(int i = 0; i < wordLength; i++) //add a node for each letter, going deeper into the tree
	{
		char currentChar = word[i];
		Node** nextNode = (*currentNode)[currentChar];
		if(*nextNode == nullptr)
			*nextNode = new Node();
		currentNode = *nextNode;
	}
	
	currentNode->isValidWord = true; //the final node should be the end of a word
}

bool Trie::isAWord(std::string word)
{
	Node* currentNode = &root;
	int wordLength = word.length();
	
	for(int i = 0; i < wordLength; i++) //traverse the tree using each successive character
	{
		char currentChar = word[i];
		Node** nextNode = (*currentNode)[currentChar];
		if(*nextNode == nullptr)
			return false; //if there is no path, it's not a word
		currentNode = *nextNode;
	}
	
	return currentNode->isValidWord; //if this is not the end of a word, it's not a word
}

std::vector<std::string> Trie::allWordsStartingWithPrefix(std::string prefix)
{
	Node* currentNode = &root;
	int prefixLength = prefix.length();

	std::vector<std::string> words;

	for(int i = 0; i < prefixLength; i++) //try to see if the prefix is even a valid path
	{
		char currentChar = prefix[i];
		Node** nextNode = (*currentNode)[currentChar];
		if(*nextNode == nullptr)
			return words; //return empty vector

		currentNode = *nextNode;
	}
	
	traverseBranches(currentNode, words, prefix); //recursively add words to the vector from the prefix onward
	return words;
}

void Trie::traverseBranches(Node* currentNode, std::vector<std::string>& words, std::string currentString)
{
	if(currentNode->isValidWord) //if this is an end node, it's a valid word so add it to the vector
		words.push_back(currentString);
	for(char c = 'a'; c <= 'z'; c++)
	{
		Node** nextNode = (*currentNode)[c];
		if(*nextNode != nullptr)
		{
			traverseBranches(*nextNode, words, (currentString + c));
		}
	}
}

//Dummy method. Only for extra credit. Not implemented.
std::vector<std::string> wordsWithWildcardPrefix(std::string wildCard)
{
	std::vector<std::string>  v;
	return v;
}