#include "pch.h"
#include <iostream>
#include <string>
#include <unordered_map>

int trees;
int layers;

struct Node {
	int data;
	struct Node* left;
	struct Node* right;
};

Node* newNode(int data) {
	Node* node = new Node;
	node->data = data;
	node->left = NULL;
	node->right = NULL;

	return(node);
}

std::string getStructure(Node* root, int k) {

	if (root == NULL) {
		return "";
	}

	std::string res;
	res += getStructure(root->left, k + 1);
	res += char(k);
	res += getStructure(root->right, k + 1);
	return res;
}

Node* insert( Node* root, int key) {

	if (root == NULL) {
		return newNode(key);
	};
	
		if (key < root->data) {
			root->left = insert(root->left, key);
		}
		else if (key > root->data) {
			root->right = insert(root->right, key);
		}

	return root;
}

int main(int argc, char** argv)
{
	std::cin >> trees >> layers;

	std::unordered_map<std::string, int> types;
	
	for (int i = 0; i < trees; i++) {
		
		Node* root = newNode(-1);

		for (int j = 0; j < layers; j++) {
			
			int key;
			std::cin >> key;
			insert(root, key);
		}

		std::string s = getStructure(root, 0);
		types[s] ++;
	}

	int total = 0;
	for (auto i : types) {
		total++;
	}

	std::cout << total;

}

