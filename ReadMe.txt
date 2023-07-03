[Base Info]
	This project describes the company-employee model, as well as implement an algorithm for calculating the salary of each employee at an arbitrary point in time (as well as calculating the total salary of all employees of the company as a whole) using C# (web services).
	There is a company model. The company can have employees. An employee is characterized by a name, date of hire to work, a base rate (which can be assigned and reassigned).
	There are 3 types of employees - Employee, Manager, Sales. Each employee can have a boss. Each employee except Employee can have subordinates. Employees (except Employee) can have any number of subordinates of any kind.
- An Employee's salary is the base rate plus 3% for each year of employment with the company, up to a maximum of 30% of the total allowance.
- The salary of a Manager employee is the base rate plus 5% for each year of work in the company (but not more than 40% of the total allowance) plus 0.5% of the salary of all first-level subordinates.
- The salary of a Sales employee is the base rate plus 1% for each year of work in the company (but not more than 35% of the total allowance) plus 0.3% of the salary of all subordinates at all levels.
	As an example of unit testing, unit tests are written to test the logic of all company services.


[Conclusions]
- Due to the fact that when writing this solution, the functionality was divided into different projects, in the future this solution can be scaled and expanded.
- Optionally, other types of user interfaces can be added. For example, launching an application on a desktop or android.
- It is also possible to add new models in the database and logic, as well as controllers associated with these models.
- Due to the use of a separate ConfigProvider, namely the GetDbConnectionString() method, the solution can be connected to different Sql servers. To do this, it is enough to change the DefaultConnection settings in the appsettings.json file of the AsposeTestTask.Web project.
- Most of the business logic of this solution is asynchronous, which increases the throughput of the solution. At the same time, if there is time, the use of CancellationTokens can be implemented in the solution to save system resources. All asynchronous methods in this solution already accept CancellationTokens.

In general, in the future this solution can be expanded and used as an ERP product.