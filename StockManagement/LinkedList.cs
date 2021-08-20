using System;

namespace StockManagement
{
    public class LinkedList
    {
        internal Node head;

        // Method to add at first position
        internal void AddFirst(string symbol, double units, double price, string type, string date)
        {
            Node node = new Node(symbol, units, price, type, date);
            node.next = this.head;
            this.head = node;
        }

        // Method to append at last position
        internal void AddLast(string symbol, double units, double price, string type, string date)
        {
            Node node = new Node(symbol, units, price, type, date);
            if (this.head == null)
            {
                this.head = node;
            }
            else
            {
                Node temp = head;
                while (temp.next != null)
                {
                    temp = temp.next;
                }
                temp.next = node;
            }
        }

        
        // Method to display
        internal void Display()
        {
            Node temp = this.head;
            if (temp == null)
            {
                Console.WriteLine("No transaction records");
                return;
            }
            Console.WriteLine($"{"Symbol",10}\t{"Type",10}\t{"Units",10}\t{"Price(Rs.)",10}\tDate");
            while (temp != null)
            {
                Console.WriteLine($"{temp.symbol,10} |\t{temp.type.ToUpper(),10} |\t{temp.units,10} |\t{temp.price,10} |\t{temp.date}");
                temp = temp.next;
            }
        }
    }
}
