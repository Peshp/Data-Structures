namespace Reverse_numbers_with_a_stack
{
    public class Program
    {
        static void Main(string[] args)
        {
            var stack = new Stack<int>(Console.ReadLine().Split().Select(int.Parse).ToArray());
            var result = ReverseNumbers(stack);

            Console.WriteLine(string.Join(" ", result));
        }

        private static List<int> ReverseNumbers(Stack<int> numbers)
        {
            var result = new List<int>();
            while (numbers.Any())
                result.Add(numbers.Pop());

            return result;
        }
    }
}