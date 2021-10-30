using System;
using System.Text;
using Tower.Models;

namespace Tower.UI
{
    internal class TowerDisplay
    {
        private readonly Towers _towers;

        public TowerDisplay(Towers towers)
        {
            _towers = towers;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var row in _towers)
            {
                AddTower(row.Item1);
                AddTower(row.Item2);
                AddTower(row.Item3);
                builder.AppendLine();
            }

            return builder.ToString();

            void AddTower(int size)
            {
                var padding = 10 - size / 2;
                builder.Append(' ', padding).Append('*', Math.Max(1, size)).Append(' ', padding);
            }
        }
    }
}