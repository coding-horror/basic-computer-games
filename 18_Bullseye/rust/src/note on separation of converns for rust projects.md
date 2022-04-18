 # Separation of concerns for Binary Projects

The organizational problem of allocating responsibility for multiple tasks to the main function is common to many projects. As a result, the Rust community has developed a process to use as a guideline for splitting the separate concerns of a binary program when main starts getting large. 

 ## The process has the following steps:
 - Split your program into a main.rs and a lib.rs and move your program’s logic to lib.rs.
 - As long as your command line logic is small, it can remain in main.rs.
 - When the command line logic starts getting complicated, extract it from main.rs and move it to lib.rs.

 ## The responsibilities that remain in the main function after this process should be limited to the following:
 - Calling the command line or input parsing logic with the argument values
 - Setting up any other configuration
 - Calling a run function in lib.rs
 - Handling the error if run returns an error

This pattern is about separating concerns: main.rs handles running the program, and lib.rs handles all the logic of the task at hand. Because you can’t test the main function directly, this structure lets you test all of your program’s logic by moving it into functions in lib.rs. The only code that remains in main.rs will be small enough to verify its correctness by reading it.