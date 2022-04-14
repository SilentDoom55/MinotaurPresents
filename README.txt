This code was written for .NET 6 and can be run by the command 'dotnet run Program.cs'

Their method resulted in more presents than thank you notes. This can occur because multiple presents can be removed if multiple servants write a thank you note for the same present. This would occur if Servant A wrote a thank you for Present[1], then Servant B also wrote a thank you for Present[1]. In their method, Servant A would remove Present[1], causing Present[2] to become Present[1]. This new Present[1] would then get removed by Servant B. This would result in only a thank you written for Present[1] and not for Present[2] despite both being removed.

This method instead used a Linked-List that was locked whenever it was modified, meaning that until a servant finishes writing a thank you and removes a present, no present can be added or removed from the linked-list.

This program is highly customizable with changing the number of servants (threads) as well as presents recieved. Through testing various numbers for both of these values, the program always ended with the correct number of "Thank You's" as well as having no presents remaining in either the Bag or the Present Linked List. Additionally, using the parameters given in the assignment (4 servants, 500000 presents), the program executed fully in less than a minute. As such, I believe this program to be fully functional and efficient.

In addition, due to this program being written using C#, the built-in lock() command will automically cause any thread stuck behind a lock to wait until the lock is released before automically obtaining the lock itself and continuing. This means that there is always progress being made.
