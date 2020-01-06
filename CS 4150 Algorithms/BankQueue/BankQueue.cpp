#include "pch.h"
#include <iostream>
#include <vector>

int main()
{
	int N, T;
	std::cin >> N >> T;

	std::vector<int> arr(T + 1, 0);

	for (int i = 0; i < N; i++)
	{
		int amount, time;
		std::cin >> amount >> time;

		for (int j = time; j >= 0; j--)
		{
			if (amount > arr[j])
			{
				std::swap(amount, arr[j]);
			}
		}
	}

	int total = 0;
	for (auto i : arr)
	{
		total += i;
	}

	std::cout << total << std::endl;
}

