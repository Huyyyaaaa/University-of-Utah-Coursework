#include "pch.h"
#include <iostream>
#include <vector>
#include <queue>
typedef long double ld;

int main()
{
	int n, m;
	while (std::cin >> n >> m) 
	{
		if (n == 0 && m == 0) break;

		std::vector<std::vector<std::pair<ld, int>>> adj(n);
		for (int i = 0; i < m; i++)
		{
			int first, second;
			ld weight;
			std::cin >> first >> second >> weight;
			adj[first].push_back({ weight, second });
			adj[second].push_back({ weight, first });

		}

		std::vector<bool> vis(n, false);
		std::vector<ld> dist(n, 0);
		std::priority_queue<std::pair<ld, int>> pq;

		pq.push({ 1,0 });
		while (!pq.empty())
		{
			int curr = pq.top().second;
			ld size = pq.top().first;
			pq.pop();

			if (vis[curr]) continue;

			vis[curr] = true;
			dist[curr] = size;

			for (auto i : adj[curr])
			{
				int next = i.second;
				ld fact = i.first;
				pq.push({ size * fact, next });

			}
		}

		std::cout << std::fixed;
		std::cout.precision(4);
		std::cout << dist[n - 1] << std::endl;
	}


}
