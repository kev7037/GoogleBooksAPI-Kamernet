# GoogleBooksAPI Demo - Kamernet
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%23563D7C.svg?style=for-the-badge&logo=bootstrap&logoColor=white)
![jQuery](https://img.shields.io/badge/jquery-%230769AD.svg?style=for-the-badge&logo=jquery&logoColor=white)
<br/><br/>
.NET MVC ( <a href="https://dotnet.microsoft.com/en-us/download/dotnet-framework/net47">target framework 4.7</a> ) Website for a searchable books list.

The website makes `HttpGet` request to <a href="https://developers.google.com/books/docs/overview">Google Books API</a> whith respect to filters client enters.
<br/>
Search can be made based on Title, Author, Publisher, Description of the books.
<br/>
The result of the search will be stored using `SqlClient`.
<br/>
User next searches will be either fetched from Google Books API or Local Database.


The project is `code-first` and uses:
<ul>
 <li>Linq</li><li>EF</li><li>Dapper</li><li>Memory Cache</li><li>SqlClient</li>
</ul> 

# How to use
> You need to have .net framework 4.7 installed on your machine!

Run the project using `Visual Studio` / `Visual Studio code` or `dotnet CLI`. You can use:

    dotnet build
    dotnet run


## Live Library Page (API/Local search)

> This page only uses Entity Framework ORM

This page contains 3 buttons that operate the search functionality:

### - Search button
> Local database will be checked to find whether this search query has been made before. If found, the booklist will be retrieved from the local database.
If no result is found, an HTTP request will be sent to the Google Books API, the result will be stored locally and shown to the client.


### - Search (With Every 5min cache) button
> Local database will be checked to find whether this search query has been made during the last 5mins. If found, the booklist will be retrieved from the local database.
If no result is found, an HTTP request will be sent to the Google Books API, the result will be stored locally (Add or Update) and shown to the client.

### - Force Live Search button
> An HTTP request will be sent to the Google Books API without checking the database, the result will be stored locally (Add or Update) and shown to the client.

## Local Library Page (Only local search)
This page contains 2 buttons that operate the search functionality:

### - Search button
> Operates the search query on the local database using dapper by calling a stored procedure.


### - Search with cache button
> Operates the search query on the local database using Entity Framework. After the first search made by this button, a memory cache will be created for the next
5 minutes. If the client uses this method again in less than 5 minutes, the result will be retrieved from the memory cache. When memory cache expires, another memory cache
will be created on the next client search.
