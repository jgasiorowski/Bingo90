# Bingo 90

## How to run
- Build
    ```
    ./gradlew build  
    ```
- Run tests
    ```
    ./gradlew test   
    ```
    or
    ```
    ./gradlew check   
    ```
- Run
  - Generate one strip and print it
      ```
      ./gradlew run --args="print-one"   
      ```
  - Run performance check (10k strips without printing) 
    - Without warmup
        ```
        ./gradlew run --args="perf"   
        ```
    - with warmup (default 1000 iterations)
        ```
        ./gradlew run --args="perf -w"   
        ```
    - with custom warmup
        ```
        ./gradlew run --args="perf -w 5000"   
        ```
## Results 
Generating 10k strips takes around **240-280ms** on a cold run\
A warmed-up run can take up to **110-150ms** 
> [!NOTE]
> Performance was measured on i7-12700K processor running in single thread

## Algorithm
To generate bingo 90 strips, the application uses an algorithm built around two main phases 
### 1. Numbers distribution to tickets in strip
1. **Generate a structure** (`NumberGroups.kt`)\
    This structure contains all available numbers (1-90), arranged into groups
    that corresponds to their target column in tickets.\
    It serves the source from which numbers are drawn throughout the process
2. **Distribute one number per column per ticket**  
    This step could technically be skipped, as the next step could complete this operation.
    However, introducing it results in a noticeable performance boost. At this stage, all tickets are empty, so numbers
    can be placed without additional checks for available spaces.
    Additionally, this step ensures that no ticket contains a column with only blank spaces
3. **Distribute rest of numbers**
    This phase iterates through the groups in `NumberGroups` structure and randomly depletes them into corresponding\
    columns. Numbers are placed in tickets that currently have the fewest numbers[^1] while ensuring that the column in\
    that ticket can still accept another number (since each column has a maximum of three cells).  
    This approach ensures a balanced allocation of numbers, eliminating the need for further adjustments to\
    fully distribute all numbers
### 2. Balancing positions of blank spaces on each ticket
> [!NOTE]
> After the first phase, each ticket contains columns with 1, 2 or 3 numbers, so 2, 1 or 0 blanks needs to be distributed accordingly
1. Introduce a structure which tracks how many blank cells remain in each row
2. Place randomly empty cells in columns that contain only one number[^1] and update structure from step 1
3. Allocate randomly blank cell in columns that contain two numbers[^1] and update structure from step 1
4. Skip columns with three numbers - there is nothing to arrange here
5. Order the numbers in each column in ascending order before returning final result

[^1]: Whenever multiple such resources are available, the process selects at random to make numbers on the ticket _appear_ more evenly distributed