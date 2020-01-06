
#include "pch.h"
#include <iostream>
#include <vector>

int main()
{
	int hotels= 0;
	std::vector<int> distance;

	std::cin >> hotels;
	
	hotels++;

	for (int i = 0; i < hotels; i++)
	{
		int temp;
		std::cin >> temp;
		distance.push_back(temp);
	}

	std::vector<int> penalty;

	for (int i = 0; i < hotels; i++)
	{
		int x = (400 - distance[i]) * (400 - distance[i]);
		penalty.push_back(x);

		for (int j = 0; j < i; j++)
		{
			int day = (400 - (distance[i] - distance[j])) * (400 - (distance[i] - distance[j]));

			if ((penalty[j] + day) < penalty[i])
			{
				penalty[i] = penalty[j] + day;
			}
		}
	}

	std::cout << penalty[hotels-1];
}
