# Bingo 90

## Algorithm
To generate bingo 90 strips application is using algorithm build out of two main phases 
### 1. Numbers distribution to tickets in strip
1. **Generate a structure** (`NumberGroups.kt`)\
    This structure contains all available numbers (1-90), arranged into groups
    that corresponds to their target column in tickets.\
    It serves the source from which numbers are drawn throughout the process
1. **Distribute one number per column per ticket**  
    This step could technically be skipped, as the next step could complete this operation.
    However, introducing it results in a noticeable performance boost. At this stage, all tickets are empty, so numbers
    can be placed without additional checks for available spaces.
    Additionally, this step ensures that no ticket contains a column with only blank spaces
1. **Distribute rest of numbers**
    This phase iterates through the groups in `NumberGroups` structure and randomly depletes them into corresponding\
    columns. Numbers are placed in tickets that currently have the fewest numbers[^1] while ensuring that the column in\
    that ticket can still accept another number (since each column has a maximum of three cells).  
    This approach ensures a balanced allocation of numbers, eliminating the need for further adjustments to\
    fully distribute all numbers
### 2. Balancing positions of blank spaces on each ticket
1. Test

[^1]: Test