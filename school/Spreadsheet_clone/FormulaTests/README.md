```
Author:     Isaac Stagg
Partner:    None
Date:       1/29/2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  istagg
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/spreadsheet-istagg
Commit #:   b47a696ede60db7925d986597004c9f722331b82
Solution:   Spreadsheet
Copyright:  CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

Code stands on its own. I specifically only tested for passing null into the equals method and none others
since our class is non-nullable. Parent Equals method can take in a null though so parameter must be an
optional, thus must test for null.

98.4% line coverage of Formula class. Only parts not covered have to do with if/else statements that were
added just in case but I can't think of any tests that would execute the else statements.

# Assignment Specific Topics

None.

# Consulted Peers:

None.

# References:

    1. https://stackoverflow.com/questions/933613/how-do-i-use-assert-to-verify-that-an-exception-has-been-thrown
    2. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions