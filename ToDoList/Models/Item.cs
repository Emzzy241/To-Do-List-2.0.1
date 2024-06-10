using MySqlConnector;
using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Item
    {
        public string Description { get; set; }

        // We don't add a set method, because this property will be set in the constructor automatically. In fact, we specifically don't ever want to manually edit it. That would increase the risk of IDs not being unique
        public int Id { get; }

        // Removing the _instances(the list we used in storing items), we will now be using a database
        // private static List<Item> _myList = new List<Item>() {};
        
        
        public Item(string myDescription)
        {
            Description = myDescription;
            // Overloading our constructor: Currently, our constructor only accepts description as an argument. Whenever we create a new object in our application, it should only have a description. However, when we retrieve a record from the database, we want its id, too. We can add an overloaded constructor so our application can instantiate an Item either way:
            //   Id = id;

            
        }

      
        // Our New GetAll() method
         public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> { };

            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString); //Opening a Database Connection
            conn.Open(); // Open() the connection. Our application will throw an exception if we try to make a SQL query without first opening a database connection.

            // Construct a SQL Query; Once our connection is open, we can construct our SQL query:
            // When we make a SQL query in our application, it's not just a string of text. The query needs to be stored in a special object called a MySqlCommand.

            // In order to do this, we call the createCommand() method on our conn object. We include the expression as MySqlCommand at the end of this line. Using as creates an expression that casts cmd into a MySqlCommand object.

            // This casting is important because there are many different types of SQL databases and many different types of objects that can interact with them. Because our connection is a MySqlConnection type object, we cast it to send a corresponding MySqlCommand to the database.
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand; 
            // Next, we'll add the actual text of our SQL command. Remember that cmd is a MySqlCommand object. A MySQLCommand object has a number of different properties we can set. We won't cover most of them, but the CommandText property is essential because it's where we'll store our actual SQL query.
            cmd.CommandText = "SELECT * FROM items;"; // just like you did on bash

            // Returning Results from the Database: Next, we need to create a Data Reader Object. It will be responsible for reading the data returned by our database in response to the "SELECT * FROM items;" command:
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            // In the above, we also We'll cast its type for use with MySQL just like we did with conn. We call this Data Reader rdr and use the as keyword to cast it into a MySqlDataReader object.
            
            // The rdr object represents the actual reading of the database. However, we will need to call other methods on the rdr object in order to display the results of the query in our application:
            // A MySqlDataReader object has a built-in Read() method that reads results from the database one at a time and then advances to the next record. This method returns a boolean. If the method advances to the next object in the database, it returns true. If it reaches the end of the records that our query has returned, it returns false and our while loop ends.
            while (rdr.Read())
            {
                // In the while loop, we'll take each individual record from our database and translate that record into an Item object our application understands:
                /*
                    Our MySQLDataReader rdr object has many methods available to it. Many of these methods are specifically for extracting data from a record. GetInt32() returns a 32 bit integer. GetString() is self-explanatory.

                    We also pass in a number value as an argument to both methods. This is because rows from the database are returned by the rdr.Read() method as indexed arrays. Let's use the following table as an example to demonstrate:

                    id | description ---+--------- 1  | Mow the lawn 2  | Walk the dog 3  | Make dinner
                    

                    When the reader object returns the first entry in this example database, it'll look like this:

                    { 1, "Mow the lawn" };
                    

                    The second object will look like this:

                    { 2, "Walk the dog" };
                    

                    The id column is at index 0 while the description column is at index 1. If we had a third column, it'd be at index 2.

                    In our while loop above, we define our itemId as rdr.GetInt32(0); because this will return the integer at the 0th index of the array returned by the reader. Similarly, we define itemDescription as rdr.GetString(1) because our item description will be at the 1st index of the array returned by the reader.

                    Once we've collected the data, we can use it to instantiate new Item objects and add them to our allItems list. Now each row in our database is an Item stored in a List that our application understands.
                */
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                Item newItem = new Item(itemDescription, itemId);
                allItems.Add(newItem);
            }
            // Closing the Connection​: Communicating with a database is a resource-intensive process. For this reason, it's important to close our database connection when we're done. This allows the database to reallocate resources to respond to requests from other users. We can use a built-in Close() method to do this.
            conn.Close();
            //The Close() method is self-explanatory. We also include a conditional because on rare occasions, our database connection will fail to close properly. It's considered best practice to confirm it's fully closed. That's why we put the Dispose() method inside a conditional. This method will only run if conn is not null.
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }
        
        public static void ClearAll() 
        { 
            // _myList.Clear(); 
        }

        //  It's static because it must sift through all Items to find the one we're seeking.
        // Also, notice we subtract 1 from the provided searchId because indexes in the _instances array begin at 0, whereas our Id properties will begin at 1. since we made use of the .Count property
        public static Item Find(int searchId)
        {
            //  Temporarily returning placeholder item to get beyond compiler errors until we refactor to work with database.
            Item placeholderItem = new Item("placeholder item");
            return placeholderItem;
        }    
        
    }
}
    
// }