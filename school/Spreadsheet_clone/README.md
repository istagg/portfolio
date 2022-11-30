```
Author:     Isaac Stagg
Partner:    Nate Tripp
Date:       1/12/2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  istagg
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/spreadsheet-istagg
Commit #:   298bdbc1833ec5267b1f87428c7cfca6b1bae531
Solution:   Spreadsheet
Copyright:  CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of interpreting an inputted string and turning it into either a
double, string or Formula. This program also keeps track of dependencies between cells for order of calculation.
It also includes support for a cell object that has a name, contents and a calculated value (or just the
string/double if not a formula). This version includes a method that calculates the value of a formula and
stores it in a cell as a double. The spreadsheet will also update cells if dependencies change. Future
extensions include a GUI.

# Examples of Good Software Practice (GSP)

DRY:
Don't repeat yourself. My code has many examples of the DRY principle. The major example is the
SetCellContents methods for strings and doubles. I saw that I wrote those methods in exactly the same way
with just a difference in the types. I pulled this code out into its own separate method and handled the
types and now I only have to maintain one spot. Other examples name checking. Instead of using the same code
every time name needs to be checked, make a small function that makes sure the inputted name is valid
instead of copying and pasting code.

Code Re-Use:
This sort of falls under the DRY principle but regardless, over the course of these 5 assignments we have
written many helper classes. Why not use them? Instead of rewriting everything or copying we can just call
those classes we created. The same applies for going between assignments 4 and 5. We wrote a lot of the main
code needed in assignment 4. Now in assignment 5 we only have one public method SetContentsOfCell. Why should
we rewrite the code inside of it? I called the previous methods I created that I knew worked and it made
the assignment a lot easier.

Regression Testing:
With this new assignment we have to rewrite the outward facing methods and also add a few new functionalities.
For the code that uses the old code, why not use the old tests? I used all of the same old tests but just
called the new outward facing function. This saves a lot of time and helps makes sure you don't forget anything
like edge cases you found previously.

Move examples:
 - Versioning
 - Testing Strategies
 - Separation of concern

# Best Team Practices
It was effective having two brains working together to solve problems. Nate has more experience then Isaac
and helped Isaac learn some of the ins and outs of Visual Studio and C#. Ultimately Isaac felt that he learned a
lot and Nate was able to learn how to explain concepts better. Having two sets of eyes on the program helped keep the
number of bugs down. We were also able to approach the problem from different directions because of differing viewpoints.

One of the biggest difficulties was finding time to meet. Nate works full time and doesn't have a lot of free time
outside of work and school. There were also some family events that came up that challenged us and caused us to have
to move to a different day. Ultimately we made it work and were able to complete the assignment before the deadline.
Overall we felt that working together was effective. We did most of the work over zoom and sometimes it was hard not
talk over each other.



# Time Expenditures:

    1. Assignment 1:   Predicted Hours:     9.0           Actual Hours:      6.5
    2. Assignment 2:   Predicted Hours:     8.0           Actual Hours:      9.0
    3. Assignment 3:   Predicted Hours:     8.0           Actual Hours:      8.0
    4. Assignment 4:   Predicted Hours:    10.0           Actual Hours:      9.75
    5. Assignment 5:   Predicted Hours:     9.0           Actual Hours:     10.5
    6. Assignment 6:   Predicted Hours:     5.0           Actual Hours:      5.75

Overall, I think my ability to estimate how much time a project will take has gotten better. Reading the documentation thoroughly
in order to fully understand the problem makes the biggest difference in coming up with a more accurate estimate.