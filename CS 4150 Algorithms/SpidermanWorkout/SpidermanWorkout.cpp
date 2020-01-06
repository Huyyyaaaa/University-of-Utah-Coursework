#include <math.h>
#include <iostream>
#include <algorithm>
#include <vector>
#include <map>

using namespace std;


struct state {
	int h;
	string p;
};

bool operator < (const state& lhs, const state& rhs) {
	return lhs.h < rhs.h;
}

state getstate(state oldstate, int currheight, int dir) {
	state newstate;
	newstate.h = max(currheight, oldstate.h);
	if (dir == 1) {
		newstate.p = oldstate.p + "U";
	}
	if (dir == -1) {
		newstate.p = oldstate.p + "D";
	}
	return newstate;
}

void solve() {
	int n;
	cin >> n;
	vector<int> v(n);
	int remaining = 0;
	for (auto& i : v) {
		cin >> i;
		remaining += i;
	}

	map<int, state> curr;
	curr[0] = { 0,"" };

	for (auto i : v) {
		map<int, state> next;
		remaining -= i;

		for (auto j : curr) {
			int hi = j.first + i;
			state s1 = getstate(j.second, hi, 1);
			if (next.count(hi) == 0) {
				next[hi] = s1;
			}
			else {
				next[hi] = min(next[hi], s1);
			}

			int lo = j.first - i;
			state s2 = getstate(j.second, lo, -1);
			if (next.count(lo) == 0) {
				next[lo] = s2;
			}
			else {
				next[lo] = min(next[lo], s2);
			}
		}

		// Clear out all negative and all greater than remaining sum
		map<int, state> actual;
		for (auto i : next) {
			if (i.first <= remaining && i.first >= 0) {
				actual[i.first] = i.second;
			}
		}

		curr = actual;
	}

	if (curr.count(0) > 0) {
		cout << curr[0].p << endl;
	}
	else {
		cout << "IMPOSSIBLE" << endl;
	}
}

int main() {
	//file();
	//fast();

	int cases;
	cin >> cases;
	while (cases--) {
		solve();
	}

	return 0;
}