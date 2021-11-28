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
                AppendTower(row.Item1);
                AppendTower(row.Item2);
                AppendTower(row.Item3);
                builder.AppendLine();
            }

            return builder.ToString();

            void AppendTower(int size)
            {
                var padding = 10 - size / 2;
                builder.Append(' ', padding).Append('*', Math.Max(1, size)).Append(' ', padding);
            }
        }
    }
}