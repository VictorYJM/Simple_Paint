using System;
using System.Collections.Generic;

public class LinkedList<Data> where Data : IComparable<Data>
{
    private Node<Data> first, last, previous, current;
    private int numberOfNodes;
    private bool firstAccess;

    public LinkedList()
    {
        first = last = previous = current = null;
        numberOfNodes = 0;
        firstAccess = false;
    }

    public bool Empty { get => First == null; } // Checks if the list is empty

    public Node<Data> First { get => first; } // Returns the first node

    public Node<Data> Last { get => last; } // Returns the last node

    public Node<Data> Previous { get => previous; } // Returns the previous node

    public Node<Data> Current { get => current; } // Returns the current node

    public int NumberOfNodes { get => numberOfNodes; } // Returns the number of nodes

    // Starts the sequential traversal
    public void StartSequentialTraversal()
    {
        firstAccess = true;
        current = first;
        previous = null;
    }

    // Checks if traversal is possible
    public bool CanTraverse()
    {
        if (!firstAccess)
        {
            previous = current;
            current = current.Next;
        }
        else
            firstAccess = false;

        return current != null;
    }

    // Inserts a new node before the start of the list
    public void InsertBeforeStart(Data newData)
    {
        var newNode = new Node<Data>(newData, first);

        if (last == null)
            last = newNode;

        first = newNode;
        numberOfNodes++;
    }

    // Inserts a new node in the middle of the list
    public void InsertInMiddle(Data newData)
    {
        var newNode = new Node<Data>(newData);

        previous.Next = newNode;
        newNode.Next = current;

        if (previous == last)
            last = newNode;

        numberOfNodes++;
    }

    // Inserts a new node after the end of the list
    public void InsertAfterEnd(Data newData)
    {
        var newNode = new Node<Data>(newData);

        if (first == null)
            first = newNode;
        else
            last.Next = newNode;

        last = newNode;
        numberOfNodes++;
    }

    // Inserts a new node in order
    public bool InsertInOrder(Data newData)
    {
        if (!ExistsData(newData))
        {
            if (Empty)
                InsertBeforeStart(newData);
            else if (previous == null && current != null)
                InsertBeforeStart(newData);
            else
                InsertInMiddle(newData);

            return true;
        }

        return false;
    }

    // Checks if a data exists in the list
    public bool ExistsData(Data otherData)
    {
        previous = null;
        current = first;

        // No way for the element to exist if the list is empty
        if (Empty)
            return false;

        // No way for the element to exist if it has a key smaller than the first element
        else if (otherData.CompareTo(first.Info) < 0)
            return false;

        // No way for the element to exist if it has a key greater than the last element
        else if (otherData.CompareTo(last.Info) > 0)
        {
            previous = last;
            current = null;
            return false;
        }

        bool found = false;
        bool end = false;

        while (!found && !end)
        {
            // If the search is over
            if (current == null)
                end = true;

            // If the element was found
            else if (otherData.CompareTo(current.Info) == 0)
                found = true;

            else if (current.Info.CompareTo(otherData) > 0)
                end = true;

            else
            {
                previous = current;
                current = current.Next;
            }
        }

        return found;
    }

    // Removes a data from the list
    public bool RemoveData(Data removeData)
    {
        // Check if the data exists to be removed
        if (ExistsData(removeData))
        {
            numberOfNodes--;

            // Wants to remove the first element of the list
            if (current == first)
            {
                first = first.Next;

                // Measure against case where the list becomes empty after removal
                if (first == null)
                    last = null;
            }

            // Wants to remove the last element of the list
            else if (current == last)
            {
                last = previous;
                last.Next = null;
            }

            // Wants to remove an element in the middle of the list
            else
            {
                previous.Next = current.Next;
                current.Next = null;
            }

            return true;
        }

        return false;
    }

    // Obtains the information of the node based on its index in the linked list
    public Data GetByIndex(int index)
    {
        // If it is not a valid index, stop the program
        if (index < 0 || index >= numberOfNodes)
            throw new Exception("Invalid index!");

        // Traverse the list sequentially until finding the index value
        current = first;
        for (int i = 0; i < index; i++)
            current = current.Next;

        return current.Info;
    }
}