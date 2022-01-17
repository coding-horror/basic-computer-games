using System.Text;

namespace Pizza
{
    internal class CustomerMap
    {
        private readonly int _mapSize;
        private readonly string[,] _customerMap;

        public CustomerMap(int mapSize)
        {
            _mapSize = mapSize;
            _customerMap = GenerateCustomerMap();
        }

        /// <summary>
        /// Gets customer on position X, Y.
        /// </summary>
        /// <param name="x">Represents X position.</param>
        /// <param name="y">Represents Y position.</param>
        /// <returns>If positions is valid then returns customer name otherwise returns empty string.</returns>
        public string GetCustomerOnPosition(int x, int y)
        {
            if(IsPositionOutOfRange(x, y))
            {
                return string.Empty;
            }
                        
            return _customerMap[y, x];
        }

        /// <summary>
        /// Overridden ToString for getting text representation of customers map.
        /// </summary>
        /// <returns>Text representation of customers map.</returns>
        public override string ToString()
        {
            int verticalSpace = 4;
            int horizontalSpace = 5;

            var mapToDisplay = new StringBuilder();

            AppendXLine(mapToDisplay, horizontalSpace);

            for (int i = _customerMap.GetLength(0) - 1; i >= 0; i--)
            {
                mapToDisplay.AppendLine("-", verticalSpace);
                mapToDisplay.Append($"{i + 1}");
                mapToDisplay.Append(' ', horizontalSpace);

                for (var j = 0; j < _customerMap.GetLength(1); j++)
                {
                    mapToDisplay.Append($"{_customerMap[i, j]}");
                    mapToDisplay.Append(' ', horizontalSpace);
                }

                mapToDisplay.Append($"{i + 1}");
                mapToDisplay.Append(' ', horizontalSpace);
                mapToDisplay.Append(Environment.NewLine);
            }

            mapToDisplay.AppendLine("-", verticalSpace);

            AppendXLine(mapToDisplay, horizontalSpace);

            return mapToDisplay.ToString();
        }

        /// <summary>
        /// Checks if position is out of range or not.
        /// </summary>
        /// <param name="x">Represents X position.</param>
        /// <param name="y">Represents Y position.</param>
        /// <returns>True if position is out of range otherwise false.</returns>
        private bool IsPositionOutOfRange(int x, int y)
        {
            return 
                x < 0 || x > _mapSize - 1 || 
                y < 0 || y > _mapSize - 1;
        }

        /// <summary>
        /// Generates array which represents customers map.
        /// </summary>
        /// <returns>Returns customers map.</returns>
        private string[,] GenerateCustomerMap()
        {
            string[,] customerMap = new string[_mapSize, _mapSize];
            string[] customerNames = GetCustomerNames(_mapSize * _mapSize);
            int currentCustomerNameIndex = 0;

            for (int i = 0; i < customerMap.GetLength(0); i++)
            {
                for (int j = 0; j < customerMap.GetLength(1); j++)
                {
                    customerMap[i, j] = customerNames[currentCustomerNameIndex++].ToString();
                }
            }

            return customerMap;
        }

        /// <summary>
        /// Generates customer names. Names are represented by alphanumerics from 'A'. Name of last customer depends on passed parameter.
        /// </summary>
        /// <param name="numberOfCustomers">How many customers need to be generated.</param>
        /// <returns>List of customer names.</returns>
        private static string[] GetCustomerNames(int numberOfCustomers)
        {
            // returns ["A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P"];
            return Enumerable.Range(65, numberOfCustomers).Select(c => ((Char)c).ToString()).ToArray();
        }

        /// <summary>
        /// Appends line with X coordinates.
        /// </summary>
        /// <param name="mapToDisplay">Current map where a new line will be appended.</param>
        /// <param name="horizontalSpace">Number of horizontal delimiters which will be added between each coordination.</param>
        private void AppendXLine(StringBuilder mapToDisplay, int horizontalSpace)
        {
            mapToDisplay.Append(' ');
            mapToDisplay.Append('-', horizontalSpace);
            for (var i = 0; i < _customerMap.GetLength(0); i++)
            {
                mapToDisplay.Append($"{i + 1}");
                mapToDisplay.Append('-', horizontalSpace);
            }
            mapToDisplay.Append(Environment.NewLine);
        }
    }
}