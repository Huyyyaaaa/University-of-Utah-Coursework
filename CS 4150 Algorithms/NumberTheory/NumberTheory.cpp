#include "pch.h"
#include <iostream>
#include <string>

long long gcd(long long a, long long b)
{
	return b == 0 ? a : gcd(b, a%b);
}

long long mod_exp(long long a, long long b, long long N)
{
	long long result = 1;
	a = a % N;
	while (b > 0)
	{
		if (b & 1)
		{
			result = (result*a) % N;
		}
		b = b >> 1;
		a = (a*a) % N;
	}
	return result;
}
long long euclids(long long a, long long b, long long &x, long long &y)
{
	if (a == 0)
	{
		x = 0;
		y = 1;
		return b;
	}
	else
	{
		long long xp, yp;
		long long d = euclids(b%a, a, xp, yp);
		x = yp - (b / a) * xp;
		y = xp;
		return d;
	}
}
long long mod_inv(long long a, long long N)
{
	long long x, y;
	long long z = euclids(a, N, x, y);
	if (z != 1) {
		return -1;
	}
	else {
		return (x%N + N) % N;
	}
}



bool prime(long long a)
{
	int i = 2;
	if (a <= 1 || a == 4) return false;
	else if (a <= 3) return true;
	while (i <= 5)
	{
		if (mod_exp(i, a - 1, a) != 1) return false;
		i = (i * 2) - 1;
	}
	return true;
}

std::string RSA(long long p, long long q)
{
	std::string res = "";
	long long pub = 2, priv;
	long long N = p * q;
	res += (std::to_string(N) + " ");

	long long phi = (p - 1)*(q - 1);
	while (pub < phi)
	{
		if (gcd(pub, phi) == 1)
		{
			res += (std::to_string(pub) + " ");
			break;
		}
		else
		{
			pub++;
		}
	}
	priv = mod_inv(pub, phi);
	res += std::to_string(priv);
	return res;
}

int main()
{
	std::string type;
	long long a,b,c;

	while (std::cin >> type >> a) 
	{
		if (type == "gcd")
		{
			std::cin >> b;
			std::cout << gcd(a, b) << std::endl;
		}
		else if (type == "exp")
		{
			std::cin >> b >> c;
			std::cout << mod_exp(a, b, c) << std::endl;
		}
		else if (type == "inverse")
		{
			std::cin >> b;
			long long result;
			if ((result = mod_inv(a, b)) == -1)
			{
				std::cout << "none" << std::endl;
			}
			else
			{
				std::cout << result << std::endl;
			}
		}
		else if (type == "isprime")
		{
			if (prime(a))
			{
				std::cout << "yes" << std::endl;
			}
			else
			{
				std::cout << "no" << std::endl;
			}
		}
		else
		{
			std::cin >> b;
			std::cout << RSA(a, b) << std::endl;
		}
	}
	return 0;
}
