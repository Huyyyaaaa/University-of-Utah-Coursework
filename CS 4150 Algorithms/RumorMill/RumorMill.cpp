
#include "pch.h"
#include <iostream>
#include <vector>
#include <cstdlib>
#include <string>
#include <map>
#include <queue>
#include <limits>
#include <algorithm>
#include <iostream>

class Student
{
public:
	std::string student;
	std::vector<Student*> friends;

	Student* prev = NULL;
	double distance = std::numeric_limits<double>::infinity();

	Student(std::string student) : student(student){}
	
	void add_friend(Student* f)
	{
		friends.push_back(f);
	}
};

struct Rumor
{
	std::string start;
};

std::vector<Student*> sort_map(std::map<std::string, Student*> &friends)
{
	std::vector<Student*> sorted;
	for (auto f : friends)
	{
		sorted.push_back(f.second);
	}
	std::stable_sort(sorted.begin(), sorted.end(), [](Student *s1, Student *s2) {
		if (s1->distance == s2->distance)
		{
			if (s1->distance < s2->distance) return true;
			if (s1->distance > s2->distance) return false;
		}
		if (s1->distance < s2->distance)return true;
		if (s1->distance > s2->distance)return false;
		return false;
	});

	return sorted;
}

void BFS(std::map<std::string, Student*> friends, Student* src)
{
	std::queue<Student*> students;
	src->distance = 0;
	students.push(src);

	while (!students.empty())
	{
		Student* s = students.front();
		students.pop();

		for (auto it : s->friends)
		{
			if (it->distance == std::numeric_limits<double>::infinity())
			{
				students.push(it);
				it->distance = s->distance + 1;
				it->prev = s;
			}
		}
	}
}

void reset_graph(const std::map<std::string, Student*> &friends)
{
	for (auto f : friends)
	{
		f.second->distance = std::numeric_limits<double>::infinity();
		f.second->prev = NULL;
	}
}

void print_tree(std::vector<Student*> list)
{
	for (auto i = list.begin(); i != list.end(); ++i)
	{
		if (i == list.begin()) std::cout << (*i)->student;
		else std::cout << " " << (*i)->student;	
	}

	std::cout << std::endl;
}

void create_rumors(std::queue<Rumor> rumors, std::map<std::string, Student*> friends)
{
	std::vector<Student*> list;
	while (!rumors.empty())
	{
		Rumor r = rumors.front();
		rumors.pop();

		Student* src = friends.find(r.start)->second;
		BFS(friends, src);
		list = sort_map(friends);
		print_tree(list);
		reset_graph(friends);
	}
}

int main()
{
	int n = 0;
	std::cin >> n;

	std::map <std::string , Student* > friends;
	std::queue<Rumor> rumors;

	for (int i = 0; i < n; i++)
	{
		std::string s = "";
		std::cin >> s;

		Student* st = new Student(s);
		friends.insert(std::make_pair(s, st));

	}

	int f = 0;
	std::cin >> f;

	for (int i = 0; i < f; i++)
	{
		std::string student;
		std::string friend_name;

		std::cin >> student >> friend_name;

		Student* s = friends.find(student)->second;
		Student* f = friends.find(friend_name)->second;
		s->add_friend(f);
		f->add_friend(s);
	}

	int r = 0;
	std::cin >> r;

	for (int i = 0; i < r; i++)
	{
		Rumor rumor;
		std::cin >> rumor.start;
		rumors.push(rumor);
	}

	create_rumors(rumors, friends);
	return 0;
}

