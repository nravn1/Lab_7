using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blazor_Lab_Starter_Code;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
namespace Lab5MSTestProject
{
    [TestClass]
    public class ProgramTests
    {
        [TestInitialize]
        public void Setup()
        {
            // Use reflection to access and clear the private static fields
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var borrowedBooksField = typeof(Program).GetField("borrowedBooks", BindingFlags.Static | BindingFlags.NonPublic);

            if (booksField != null)
            {
                var books = (List<Book>)booksField.GetValue(null);
                books?.Clear(); // Clear the list to reset state
            }

            if (usersField != null)
            {
                var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);
                users?.Clear(); // Clear the list to reset state
            }

            if (borrowedBooksField != null)
            {
                var borrowedBooks = (Dictionary<Blazor_Lab_Starter_Code.User, List<Book>>)borrowedBooksField.GetValue(null);
                borrowedBooks?.Clear(); // Clear the dictionary to reset state
            }
        }

        [TestMethod]
        public void ReadBook_ShouldLoadList()
        {
            // Arrange
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var books = (List<Book>)booksField.GetValue(null);
            //Program.books.Clear();
            int expcount = 1000;

            // Act
            Program.ReadBooks();

            // Assert
            Assert.AreEqual(expcount, books.Count, "Book list should contain 1000 books.");
        }

        [TestMethod]
        public void AddBook_ShouldAddBookToList()
        {
            // Arrange
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var books = (List<Book>)booksField.GetValue(null);

            string title = "Booky";
            string author = "Arthy";
            string isbn = "123-2";

            // Act
            int id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(new Book { Id = id, Title = title, Author = author, ISBN = isbn });

            // Assert
            Assert.AreEqual(1, books.Count, "Book list should contain one book.");
            Assert.IsNotNull(books[0].Title, "Book title should not be null.");
            Assert.IsNotNull(books[0].Author, "Book author should not be null.");

        }

        [TestMethod]
        public void AddBook_ShouldAddBookToExistingList()
        {
            // Arrange
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var books = (List<Book>)booksField.GetValue(null);
            Program.ReadBooks();

            string title = "Booky";
            string author = "Arthy";
            string isbn = "123-2";
            int origcount = books.Count;


            // Act
            int id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(new Book { Id = id, Title = title, Author = author, ISBN = isbn });

            // Assert
            Assert.AreEqual(origcount + 1, books.Count, "book list should contain one more book.");
       
        }

        [TestMethod]
        public void DeleteBook_ShouldRemoveBook()
        {
            // Arrange
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var books = (List<Book>)booksField.GetValue(null);
            Program.ReadBooks();

            int bookId = 9;
            int origcount = books.Count;

            // Act
            Book book = books.FirstOrDefault(b => b.Id == bookId);
            books.Remove(book);

            // Assert
            Assert.AreEqual(origcount - 1, books.Count, "Book list should contain one less book.");
   
        }

        [TestMethod]
        public void DeleteBook_BadIDError()
        {
            // Arrange
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var books = (List<Book>)booksField.GetValue(null);
            Program.ReadBooks();

            int bookId = 1009;
            int origcount = books.Count;

            // Act
            Book book = books.FirstOrDefault(b => b.Id == bookId);
            

            // Assert
            Assert.IsNull(book, "If ID does not exist, should be null");

        }


        [TestMethod]
        public void ReadUser_ShouldLoadList()
        {
            // Arrange
            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);

            int expcount = 100;

            // Act
            users?.Clear();
            Program.ReadUsers();

            // Assert
            Assert.AreEqual(expcount, users.Count, "User list should contain 100 users.");
        }

        [TestMethod]
        public void AddUser_ShouldAddUserToList()
        {
            // Arrange
            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);

            string name = "John Dale";
            string email = "jd@gmail.com";


            // Act
            int id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(new Blazor_Lab_Starter_Code.User { Id = id, Name = name, Email = email });

            // Assert
            Assert.AreEqual(1, users.Count, "User list should contain one User.");
            Assert.IsNotNull(users[0].Name, "User name should not be null.");
            Assert.IsNotNull(users[0].Email, "user email should not be null.");
        }

        [TestMethod]
        public void AddUser_ShouldAddUserToExistingList()
        {
            // Arrange
            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);
            Program.ReadUsers();

            string name = "John Dale";
            string email = "jd@gmail.com";
            int origcount = users.Count;


            // Act
            int id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(new Blazor_Lab_Starter_Code.User { Id = id, Name = name, Email = email });

            // Assert
            Assert.AreEqual(origcount + 1, users.Count, "User list should contain one more User.");
        }

        [TestMethod]
        public void DeleteUser_ShouldRemoveUser()
        {
            // Arrange
            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);
            Program.ReadUsers();

            int userId = 15;
            int origcount = users.Count;

            // Act
            Blazor_Lab_Starter_Code.User user = users.FirstOrDefault(u => u.Id == userId);
            users.Remove(user);

            // Assert
            Assert.AreEqual(origcount - 1, users.Count, "User list should contain one less user.");

        }

        [TestMethod]
        public void DeleteUser_BadIDError()
        {
            // Arrange
            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);
            Program.ReadUsers();

            int userId = 109;
            int origcount = users.Count;

            // Act
            Blazor_Lab_Starter_Code.User user = users.FirstOrDefault(u => u.Id == userId);


            // Assert
            Assert.IsNull(user, "If user ID does not exist, should be null");

        }

        [TestMethod]
        public void BorrowBook_ShouldBorrowSuccessfully()
        {
            // Arrange
            var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
            var books = (List<Book>)booksField.GetValue(null);

            var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
            var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);

            Dictionary<Blazor_Lab_Starter_Code.User, List<Book>> borrowedBooks = new Dictionary<Blazor_Lab_Starter_Code.User, List<Book>>();
            
            var borrowedBooksField = typeof(Program).GetField("borrowedBooks", BindingFlags.Static | BindingFlags.NonPublic);
            //var borrowedBooks = (Dictionary<Blazor_Lab_Starter_Code.User, List<Book>>)borrowedBooksField.GetValue(null);

            Program.ReadBooks();
            Program.ReadUsers();

            int bookId = 9;
            int userId = 15;
            int origcount = books.Count;
            int origusercount = users.Count;

            // Act
            Book book = books.FirstOrDefault(b => b.Id == bookId);
            Blazor_Lab_Starter_Code.User user = users.FirstOrDefault(u => u.Id == userId);
            //borrowedBooks[user].Add(book);
            if (!borrowedBooks.ContainsKey(user))
            {
                borrowedBooks[user] = new List<Book>();
            }

            borrowedBooks[user].Add(book);

            books.Remove(book);

            // Assert
            Assert.AreEqual(origcount - 1, books.Count, "Books should contain 1 less book from being borrowed.");
            Assert.AreEqual(1, borrowedBooks.Count, "Borrowed Book List should be 1.");

        }

        //[TestMethod]
        //public void ReturnBook_ShouldReturnSuccessfully()
        //{
        //    // Arrange
        //    var booksField = typeof(Program).GetField("books", BindingFlags.Static | BindingFlags.NonPublic);
        //    var books = (List<Book>)booksField.GetValue(null);

        //    var usersField = typeof(Program).GetField("users", BindingFlags.Static | BindingFlags.NonPublic);
        //    var users = (List<Blazor_Lab_Starter_Code.User>)usersField.GetValue(null);

        //    Dictionary<Blazor_Lab_Starter_Code.User, List<Book>> borrowedBooks = new Dictionary<Blazor_Lab_Starter_Code.User, List<Book>>();
          

        //    Program.ReadBooks();
        //    Program.ReadUsers();

        //    int bookId = 9;
        //    int userId = 15;
        //    int origcount = books.Count;
        //    int origusercount = users.Count;

        //    Book book = books.FirstOrDefault(b => b.Id == bookId);
        //    Blazor_Lab_Starter_Code.User user = users.FirstOrDefault(u => u.Id == userId);
            
        //    if (!borrowedBooks.ContainsKey(user))
        //    {
        //        borrowedBooks[user] = new List<Book>();
        //    }

        //    borrowedBooks[user].Add(book);

        //    books.Remove(book);

        //    // Act
        //    Book book = books.FirstOrDefault(b => b.Id == bookId);
        //    Blazor_Lab_Starter_Code.User user = users.FirstOrDefault(u => u.Id == userId);
        //    //borrowedBooks[user].Add(book);
        //    if (!borrowedBooks.ContainsKey(user))
        //    {
        //        borrowedBooks[user] = new List<Book>();
        //    }

        //    borrowedBooks[user].Add(book);

        //    books.Remove(book);

        //    // Assert
        //    Assert.AreEqual(origcount - 1, books.Count, "Books should contain 1 less book from being borrowed.");
        //    Assert.AreEqual(1, borrowedBooks.Count, "Borrowed Book List should be 1.");

        //}

    }
}
