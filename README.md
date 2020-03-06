# Identity-Server-4-Web-API-Access
Use Identity Server to Control Web API Access


## [InMemory DB](https://stackoverflow.com/questions/48061096/why-cant-i-call-the-useinmemorydatabase-method-on-dbcontextoptionsbuilder/48062124)

According to EF Core: Testing with InMemory reference, you need to add the Microsoft.EntityFrameworkCore.InMemory package to use UseInMemoryDatabase() extension method with DbContextOptionsBuilder:

Install-Package Microsoft.EntityFrameworkCore.InMemory
Afterwards, you can follow example given in "Writing tests" section like this:

var options = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase(databaseName: "database_name").Options;

using (var context = new ProductContext(options))
{
    // add service here
}

## [Use EF InMemory DB Example](https://exceptionnotfound.net/ef-core-inmemory-asp-net-core-store-database
/)

