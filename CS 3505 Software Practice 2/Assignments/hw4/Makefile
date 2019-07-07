GOOGLETEST ?= googletest/googletest

all : TrieTest TestRun clean

TrieTest : TrieTest.cpp
	g++ -o TrieTest -std=c++11 Trie.cpp TrieTest.cpp -L $(GOOGLETEST)/lib -I $(GOOGLETEST)/include -lgtest -lgtest_main -lpthread

TestRun : TrieTest
	./TrieTest

clean :
	-rm TrieTest
	echo Clean done
