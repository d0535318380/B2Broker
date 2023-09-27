
### 1. What issue do asynchronous methods help to address? 
> Asynchronous methods help to address the issue of blocking threads in a program. 
> This can lead to poor performance and unresponsive user interfaces.
> Asynchronous execution is typically used for I/O-bound operations, where the thread is waiting for a response from an external system, such as a database or network.

### 2. What is the difference between asynchronous and parallel execution?
> Asynchronous execution allows a single thread to perform multiple tasks by interleaving the execution of those tasks.  

> Parallel execution, on the other hand, uses multiple threads to perform multiple tasks simultaneously. Each thread executes a separate task, and the tasks are coordinated to ensure that they do not interfere with each other.

### 3. Are there any nuances or issues arising from the use of ThreadPool, TPL and async/await that you can describe based on your experience?
> Synchronize threads and access to shared data. 

### 4. Which approaches do you know to signal about an occurred event to another thread? Can you elaborate on their differences and specifics?
> ManualResetEvent and AutoResetEvent, Semaphore, TaskCompletionSource

### 5. Can you describe your experience with applying common design and development principles and patterns? 
> WTF?

### 6. Which of them do you find most useful? 
> Principes - TDD, SOLID, DRY, KISS 
> Patterns - DI, IoC, Factory, Command/Query segregation, Mediator

### 7. Which tasks did you solve by following them and why did you choose them?
> WTF?

### 8. Can you list the mechanisms used to implement Dependency Injection and compare them in detail, with some examples from your own experience?
- Constructor Injection: This is the most common form of DI, where dependencies are injected through a class constructor.
````csharp
public class MyClass
{
    private readonly IDependency _dependency;

    public MyClass(IDependency dependency)
    {
        _dependency = dependency;
    }

}
````

- Property Injection: In this form of DI, dependencies are injected through public properties of a class. 
````csharp
public class MyClass
{
    public IDependency Dependency { get; set; }
}
````
- Method Injection: In this form of DI, dependencies are injected through a method call. 
````csharp
public class MyClass
{
    private IDependency _dependency;

    public void SetDependency(IDependency dependency)
    {
        _dependency = dependency;
    }
}
````