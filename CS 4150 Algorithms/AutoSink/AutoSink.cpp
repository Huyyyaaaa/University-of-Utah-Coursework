#include "pch.h"
#include <iostream>
#include <string>
#include <vector>
#include <map>

using namespace std;

map<string, map<string, int>> roadmap;
map<string, map<string, int>> reversemap;
vector<string> sorted;
map<string, int> tolls;


void topological_sort(map<string, int> inputs)
{
	sorted = vector<string>();

	while (inputs.size() > 0)
	{
		vector<string> erase;
		for (auto& i : inputs)
		{
			if (i.second == 0)
			{
				string src = i.first;
				sorted.push_back(src);
				erase.push_back(src);
			}

		}
		for (int i = 0; i < erase.size(); i++)
		{
			string src = erase[i];
			for (auto& i : roadmap[src])
			{
				string dest = i.first;
				if (roadmap[src][dest] != INT32_MAX)
				{
					inputs[dest]--;
				}
			}
			inputs.erase(src);
		}
	}
}

int cheapest_path(string src, string dest)
{
	map<string, int> cost;

	for (auto& i : tolls)
	{
		cost[i.first] = INT32_MAX;
	}
	cost[src] = 0;
	int i;
	for (i = 0; i < sorted.size(); i++)
	{
		if (sorted[i] == dest)
		{
			return -1;
		}
		if (sorted[i] == src)
		{
			break;
		}
	}
	for (i += 1; i < sorted.size(); i++)
	{
		string temp = sorted[i];
		int c = INT32_MAX;

		for (auto& i : reversemap[temp])
		{
			string s = i.first;
			if (roadmap[s][temp] != INT32_MAX)
			{
				if (cost[s] != INT32_MAX && cost[s] + roadmap[s][temp] < c)
				{
					c = cost[s] + roadmap[s][temp];
				}
			}
		}

		if (temp == dest) return c;
		else cost[temp] = c;
	}

	return -1;
}

int main()
{

	int cities = 0;
	std::cin >> cities;

	map<string, int> inputs;

	for (int i = 0; i < cities; i++) {
		std::string city;
		int toll;

		std::cin >> city >> toll;

		tolls[city] = toll;
		inputs[city] = 0;
	}

	for (auto& i : tolls) {
		for (auto& j : tolls) {
			roadmap[i.first][j.first] = INT32_MAX;
		}
	}

	int edges= 0;
	std::cin >> edges;
	for (int i = 0; i < edges; i++) {
		 
		std::string src, dest;
		std::cin >> src >> dest;
	
		roadmap[src][dest] = tolls[dest];
		reversemap[dest][src] = tolls[dest];
		inputs[dest]++;

	}

	topological_sort(inputs);

	int travels=0;
	std::cin >> travels;
	for (int i = 0; i < travels; i++) {
		std::string src, dest;
		std::cin >> src >> dest;

		if (src == dest)
		{
			std::cout << 0 << std::endl;
		}
		else
		{
			int toll = cheapest_path(src, dest);
			if (toll == INT32_MAX || toll < 0) std::cout << "NO" << std::endl;
			else std::cout << toll << std::endl;
		}

	}

	return 0;
}
