using System;

namespace MinotaurPresents
{
    internal class Program
    {
        static LinkedList<int> presents = new LinkedList<int>();    // Linkedlist of the presents, in order from least to greatest
        static List<int> bag = new List<int>();                     // Bag containing all the presents before sorting
        static int pNum = 500000;                                   // Number of presents the minotaur recieved
        static int thankCount = 0;                                  // Number of thank you cards written
        static int servCount = 4;                                   // Number of servants working
        static Random rnd = new Random();                           // Random Number Generator for servant work
        static void Main(string[] args)
        {
            // Adds all the presents to the bag
            for(int i = 1; i <= pNum; i++)
            {
                bag.Add(i);
            }

            Thread[] threads = new Thread[servCount];

            // Creates and starts all servants
            for (int i = 0; i < servCount; i++)
            {
                threads[i] = new Thread(new ThreadStart(() => Servant()));
                threads[i].Start();
            }

            // Waits for servants to finish
            for (int i = 0; i < servCount; i++)
                threads[i].Join();

            Console.WriteLine("Thanked {0} gifters", thankCount);
            Console.WriteLine("{0} gifts left in the bag", bag.Count);
            Console.WriteLine("{0} gifts left in the list", presents.Count);
        }

        // Main servant function
        private static void Servant()
        {
            // Declaring action variable, controls what action the servant does
            // 0 is to add a present to the list, 1 is to write a thank you
            int action = 0;

            // Loops until the number of thank you's written is equal to the number of presents recieved
            while(!Done())
            {
                // Randomizes what action the servant does
                action = rnd.Next(0, 2);
                // The bag must still have presents to add them to the list
                if(action == 0 && bag.Count > 0)
                {
                    int presentNum = Remove();
                    if(presentNum != -1)
                        Write(presentNum);
                }
                // There must be a present in the list to thank
                else if(presents.Count > 0)
                {
                    Thank();
                }

            }
        }

        // Helper funciton to remove item from the bag
        private static int Remove()
        {
            // Locks the bag from other threads
            lock(bag)
            {
                // Verifies that the bag still has presents in it
                if(bag.Count > 0)
                {
                    try
                    {
                        // Gets the present's index in the bag and gets the value of the present
                        int presentIndex = -1;
                        presentIndex = rnd.Next(0, bag.Count);
                        int presentNum = bag.ElementAt(presentIndex);
                        Console.WriteLine("Removed {0} from bag", presentNum);

                        // Removes the present at the index and returns its value
                        bag.RemoveAt(presentIndex);
                        return presentNum;
                    }
                    catch(Exception e)
                    {
                        // Returns -1 if theres an error
                        return -1;
                    }
                }
                // Returns -1 if theres an error
                return -1;
            }
        }

        // Helper function to add a present to the present list
        private static void Write(int num)
        {
            // Locks the present list from other threads
            lock (presents)
            {
                // If there aren't any presents in the list, it simply adds the present to it
                if (presents.Count == 0)
                {
                    presents.AddFirst(num);
                    Console.WriteLine("No items in bag, added {0}", num);
                }
                else
                {
                    // Looks for the first present that is a higher value than the one being inserted
                    // Then insert the new present before that one
                    foreach (int pres in presents)
                    {
                        if (pres > num)
                        {
                            var current = Find(pres);
                            presents.AddBefore(current, num);
                            Console.WriteLine("Added {0} to list before {1}", num, pres);
                            break;
                        }
                    }
                    // This occurs if the present is larger than all presents currently in the list
                    // So, it inserts it as the last item
                    if(!presents.Contains(num))
                    {
                        presents.AddLast(num);
                        Console.WriteLine("Added {0} to list as the last item", num);
                    }
                }
            }
        }

        // Helper function to thank the first present in the list
        private static void Thank()
        {
            // Locks the presents list from other threads
            lock (presents)
            {
                // Verifies that there are presents in the list
                if(presents.Count > 0)
                {
                    // Removes the first present in the list and increments the thankCounter
                    try
                    {
                        presents.RemoveFirst();
                        thankCount++;
                        Console.WriteLine("Thanked person from list");
                    }
                    catch (Exception e)
                    {
                        return;
                    }
                }
            }
        }

        // Helper function that simply returns the node from presents of a specified present
        private static LinkedListNode<int> Find(int num)
        {
            lock(presents)
            {
                return presents.Find(num);
            }
        }

        // Helper function that determines if the servants are done yet
        // They are done when the number of thank you's written equals
        // the number of presents recieved
        private static bool Done()
        {
            return thankCount == pNum;
        }
    }
}