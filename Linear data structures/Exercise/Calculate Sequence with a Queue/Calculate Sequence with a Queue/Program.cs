using System.Net.NetworkInformation;

namespace Calculate_Sequence_with_a_Queue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int number = int.Parse(Console.ReadLine());
            var result = new Queue<int>();
            var currentNumber = 0;

            result.Enqueue(number);
            result.Enqueue(number + 1);

            for (int i = 1; i <= 50; i++)
            {               
                if (i % 3 == 0)
                {
                    result.Enqueue(2 * currentNumber + 1);
                }
                if (i % 4 == 0)
                {
                    result.Enqueue(currentNumber + 2);
                    result.Dequeue();
                }                   
                if(i % 5 == 0)
                {
                    result.Enqueue(currentNumber + 1);
                }
                currentNumber = result.Peek();
                
            }

            Console.WriteLine(string.Join(" ", result));
        }
    }
}