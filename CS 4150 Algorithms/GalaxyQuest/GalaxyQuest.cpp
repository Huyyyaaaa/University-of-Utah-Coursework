#include "pch.h"
#include <iostream>
#include <vector>
#include <cmath>


double diameter;
double stars;

bool in_galaxy(std::pair<int, int> a, std::pair<int, int>b)
{
	return (std::pow((a.first - b.first), 2) + std::pow((a.second - b.second), 2)) <= std::pow(diameter, 2);
}

std::pair<int, int> find_maj(std::vector<std::pair<int, int>> coords)
{
	if (coords.size() == 0) return std::pair<int, int>(-1, -1);
	else if (coords.size() == 1) return coords[0];

	std::vector<std::pair<int, int>> new_coords;
	for (int i = 1; i < coords.size(); i += 2)
	{
		if (in_galaxy(coords[i - 1], coords[i])) new_coords.push_back(coords[i]);
	}

	std::pair<int, int> left_coords;
	if (coords.size() % 2 == 1) left_coords = coords.back();

	std::pair<int, int> maj = find_maj(new_coords);
	if (maj.first == -1 && maj.second == -1)
	{
		if (coords.size() % 2 == 1)
		{
			int left_over = 0;
			for (int i = 0; i < coords.size(); i++)
			{
				if (in_galaxy(left_coords, coords[i])) left_over++;
			}
			if (left_over > coords.size() / 2) return left_coords;
		}

		return std::pair<int, int>(-1, -1);
	}
	else
	{
		int num_maj = 0;
		for (int i = 0; i < coords.size(); i++)
		{
			if (in_galaxy(maj, coords[i])) num_maj++;
		}
		if (num_maj > coords.size() / 2) return maj;
		else return std::pair<int, int>(-1, -1);
	}
}

int main()
{
	std::cin >> diameter >> stars;

	std::vector<std::pair<int, int>> coords;

	for (int i = 0; i < stars; i++)
	{
		double x, y;
		std::cin >> x >> y;
		std::pair<int, int> c(x, y);
		if(x!= 0 && y!= 0)
		coords.push_back(c);
	}

	std::pair<int, int> maj = find_maj(coords);

	if (maj.first == -1 & maj.second == -1)
	{
		std::cout << "NO";
	}
	else
	{
		int res = 0;
		for (int i = 0; i < coords.size(); i++)
		{
			if (in_galaxy(maj, coords[i]))
			{
				res++;
			}
		}

		std::cout << res;
	}

}
