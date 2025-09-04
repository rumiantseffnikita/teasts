using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Чтение всех строк из входного файла
        string[] input = File.ReadAllLines("input.txt");

        // Парсинг запасов (первая строка файла)
        int[] supply = Array.ConvertAll(input[0].Split(), int.Parse);

        // Парсинг потребностей (вторая строка файла)
        int[] demand = Array.ConvertAll(input[1].Split(), int.Parse);

        // Парсинг матрицы стоимостей (оставшиеся строки)
        int[][] costs = input.Skip(2).Select(line => Array.ConvertAll(line.Split(), int.Parse)).ToArray();

        // Решение транспортной задачи
        int[,] plan = SolveTransportProblem(supply, demand, costs);

        // Запись плана перевозок в выходной файл
        using (StreamWriter writer = new StreamWriter("output.txt"))
        {
            for (int i = 0; i < plan.GetLength(0); i++)
            {
                for (int j = 0; j < plan.GetLength(1); j++)
                {
                    writer.Write(plan[i, j] + " ");
                }
                writer.WriteLine();
            }
        }
    }

    static int[,] SolveTransportProblem(int[] supply, int[] demand, int[][] costs)
    {
        // Инициализация матрицы плана перевозок
        int[,] plan = new int[supply.Length, demand.Length];

        // Копии массивов для отслеживания остатков
        int[] remainingSupply = (int[])supply.Clone();
        int[] remainingDemand = (int[])demand.Clone();

        // Пока есть неудовлетворённые запасы и потребности
        while (remainingSupply.Any(s => s > 0) && remainingDemand.Any(d => d > 0))
        {
            int minCost = int.MaxValue;
            int minI = -1, minJ = -1;

            // Поиск клетки с минимальной стоимостью
            for (int i = 0; i < supply.Length; i++)
            {
                for (int j = 0; j < demand.Length; j++)
                {
                    if (remainingSupply[i] > 0 && remainingDemand[j] > 0 && costs[i][j] < minCost)
                    {
                        minCost = costs[i][j];
                        minI = i;
                        minJ = j;
                    }
                }
            }

            // Определение объёма перевозки
            int amount = Math.Min(remainingSupply[minI], remainingDemand[minJ]);

            // Запись в план и обновление остатков
            plan[minI, minJ] = amount;
            remainingSupply[minI] -= amount;
            remainingDemand[minJ] -= amount;
        }

        return plan;
    }
}