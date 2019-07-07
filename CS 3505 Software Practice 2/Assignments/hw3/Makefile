# Aaron Templeton
# U0734119
# CS3505
#A3: Trie and Rule of Three

CC = g++
FLAGS = -Wall -g --std=c++11


TrieTest: TrieTest.cpp Trie.o Node.o
	$(CC) $(FLAGS) -o TrieTest Trie.o Node.o TrieTest.cpp


Trie.o: Trie.cpp Trie.h
		$(CC) $(FLAGS) -c Trie.cpp

Node.o: Node.cpp Node.h
	$(CC) $(FLAGS) -c  Node.cpp

clean:
	rm TrieTest *.o

test:
	./TrieTest "dictionary.txt" "query.txt"

test2:
	./TrieTest "dictionar.txt" "query.txt"

test3:
	./TrieTest "dictionary.txt" "q.txt"
