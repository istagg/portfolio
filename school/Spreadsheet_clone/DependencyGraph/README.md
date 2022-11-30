```
Author:     Isaac Stagg
Partner:    None
Date:       1/25/2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  istagg
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/spreadsheet-istagg
Commit #:   2c526b3b17420029b1b8196da758eaa6664a4fdb
Solution:   Spreadsheet
Copyright:  CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

Code stands on its own.

Incorporates a method that "cleans up" the data structure every time a dependency is
changed or removed. For example: if a->b->c and b->c is removed the data structure would end up looking like
a->b c with c floating around with no dependees or dependents. This method finds these "floaters" and removes
them so that over time it doesn't clog up the algorithm.

# Assignment Specific Topics

None.

# Consulted Peers:

None.

# References:

    1. https://stackoverflow.com/questions/1273139/c-sharp-java-hashmap-equivalent
    2. https://docs.microsoft.com
    3. https://stackoverflow.com/questions/33478572/passing-arraylist-as-ienumerable-to-a-method
    4. https://stackoverflow.com/questions/4700613/convert-from-list-into-ienumerable-format
    5. https://www.geeksforgeeks.org/c-sharp-isnullorwhitespace-method/