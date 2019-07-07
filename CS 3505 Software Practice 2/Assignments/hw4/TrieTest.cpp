/*****************************************************************
*	Lance Atkinson
* 	Aaron Templeton
*	CS 3505 - Software Practice II
*	Assignment 4 - Travis and Refactor
*****************************************************************/

#include "googletest/googletest/include/gtest/gtest.h"
#include "Trie.h"

using namespace std;

TEST(TrieTest, IsAWord)
{
	Trie t;
	
	ASSERT_FALSE(t.isAWord("mother"));
	t.addAWord("mother");
	ASSERT_TRUE(t.isAWord("mother"));
	
	ASSERT_FALSE(t.isAWord("father"));
	t.addAWord("father");
	ASSERT_TRUE(t.isAWord("father"));
}

TEST(TrieTest, SubWordIsNotWord)
{
	Trie t;
	t.addAWord("mother");
	ASSERT_FALSE(t.isAWord("moth"));
}

TEST(TrieTest, SuperWordIsNotWord)
{
	Trie t;
	t.addAWord("mother");
	ASSERT_FALSE(t.isAWord("mothers"));
}

TEST(TrieTest, EmptyStringIsWord){
	Trie t;
	ASSERT_FALSE(t.isAWord(""));
	t.addAWord("");
	ASSERT_FALSE(t.isAWord("")); //test it twice
}

TEST(TrieTest, TestPrefixWords){
	Trie t;
	t.addAWord("dog");
	t.addAWord("dogs");
	
	vector<string> prefixWord;
	prefixWord = t.allWordsStartingWithPrefix("do");
	
	//dog/ dogs could be index 0 or 1 depending on implementation
	bool dogIsInDictionary = (prefixWord[0] == "dog") || (prefixWord[1] == "dog");
	bool dogsIsInDictionary = (prefixWord[0] == "dogs") || (prefixWord[1] == "dogs"); 
	ASSERT_TRUE(dogIsInDictionary);
	ASSERT_TRUE(dogsIsInDictionary);
}

TEST(TrieTest, TestCopyConstructor){
	Trie t;
	t.addAWord("apple");
	
	Trie s = t;
	
	t.addAWord("banana");
	s.addAWord("coconut");

	ASSERT_NE(&t, &s);
	ASSERT_TRUE(t.isAWord("apple"));
	ASSERT_TRUE(s.isAWord("apple"));
	ASSERT_FALSE(s.isAWord("banana"));
	ASSERT_FALSE(t.isAWord("coconut"));
	
}

TEST(TrieTest, CopyWordsFromTrieToNewTrie){
	Trie t;
	t.addAWord("hello");
	Trie s = t;
	ASSERT_TRUE(s.isAWord("hello"));
}

TEST(TrieTest, PrefixesBetweenTries){
	Trie t;
	t.addAWord("dog");
	Trie s = t;
	ASSERT_EQ("dog", s.allWordsStartingWithPrefix("do")[0]);
}

TEST(TrieTest, TriePointer){
	Trie* t = new Trie();
	t->addAWord("word");
	ASSERT_TRUE(t->isAWord("word"));

	delete t;
}

TEST(TrieTest, TriePointerCopy){
	Trie* t = new Trie();
	t->addAWord("word");
	
	Trie& s = *t;
	
	ASSERT_TRUE(s.isAWord("word"));
	ASSERT_EQ(&s, t);
	
	delete t;
}

TEST(TrieTest, PrefixesCopy){
	Trie t;
	t.addAWord("dog");
	Trie s = t;
	ASSERT_EQ("dog", s.allWordsStartingWithPrefix("do")[0]);
	s.addAWord("cat");
	ASSERT_TRUE(s.isAWord("cat"));
	ASSERT_EQ("cat", s.allWordsStartingWithPrefix("ca")[0]);
	ASSERT_FALSE(t.isAWord("cat"));
}

TEST(TrieTest, PrefixSameAsWord)
{
	Trie t;
	t.addAWord("ocean");
	vector<string> allWords = t.allWordsStartingWithPrefix("ocean");
	ASSERT_EQ(allWords[0], "ocean");
}

TEST(TrieTest, PrefixBiggerThanWord)
{
	Trie t;
	t.addAWord("ocean");
	vector<string> allWords = t.allWordsStartingWithPrefix("oceans");
	ASSERT_EQ(allWords.size(), 0);
}

TEST(TrieTest, PrefixNotInDictionary)
{
	Trie t;
	t.addAWord("ocean");
	t.addAWord("sea");
	t.addAWord("lake");
	t.addAWord("river");
	t.addAWord("pond");
	vector<string> allWords = t.allWordsStartingWithPrefix("a");
	ASSERT_EQ(allWords.size(), 0);
}

TEST(TrieTest, EmptyWordNotInPrefixList)
{
	Trie t;
	vector<string> allWords = t.allWordsStartingWithPrefix("");
	ASSERT_EQ(allWords.size(), 0);
	
	t.addAWord("");
	
	//test it again
	allWords = t.allWordsStartingWithPrefix("");
	ASSERT_EQ(allWords.size(), 0);
}

TEST(TrieTest, AddReallyLongWord)
{
	Trie t;
	t.addAWord("qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm");
	ASSERT_TRUE(t.isAWord("qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm"));
}

TEST(TrieTest, AddReallyShortWord)
{
	Trie t;
	t.addAWord("y");
	ASSERT_TRUE(t.isAWord("y"));
}

TEST(TrieTest, AddWordMadeOfQs)
{
	Trie t;

	t.addAWord("qqqqqqqq");
	ASSERT_TRUE(t.isAWord("qqqqqqqq"));
	
	ASSERT_FALSE(t.isAWord("qqqqqqqqq"));
	ASSERT_FALSE(t.isAWord("qqqqqqq"));
	
}

TEST(TrieTest, AssignmentOverloadTest)
{
	Trie t1;
	Trie t2;
	
	t1.addAWord("antelope");
	t2.addAWord("baboon");
	
	t2 = t1;
	
	t1.addAWord("chinchilla");
	t2.addAWord("deer");
	
	ASSERT_NE(&t1, &t2);
	ASSERT_FALSE(t1.isAWord("deer"));
	ASSERT_FALSE(t2.isAWord("baboon"));
	ASSERT_FALSE(t2.isAWord("chinchilla"));
	ASSERT_TRUE(t2.isAWord("antelope"));
}

int main(int argc, char* argv[])
{
	::testing::InitGoogleTest(&argc, argv);
	return RUN_ALL_TESTS();
}
