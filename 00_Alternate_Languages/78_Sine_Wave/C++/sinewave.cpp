#include <iostream> // std::cout, std::endl
#include <string>	// std::string(size_t n, char c)
#include <cmath>	// std::sin(double x)

int main()
{
	std::cout << std::string(30, ' ') << "SINE WAVE" << std::endl;
	std::cout << std::string(15, ' ') << "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY" << std::endl;
	std::cout << std::string(5, '\n');

	bool b = true;

	for (double t = 0.0; t <= 40.0; t += 0.25)
	{
		int a = int(26 + 25 * std::sin(t));
		std::cout << std::string(a, ' ') << (b ? "CREATIVE" : "COMPUTING") << std::endl;
		b = !b;
	}

	return 0;
}
