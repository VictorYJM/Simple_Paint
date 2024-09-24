using System;
using System.Collections.Generic;

public class Node<Data> where Data : IComparable<Data>
{
    private Data info;
    private Node<Data> next;

    public Node(Data info, Node<Data> next)
    {
        Info = info;
        Next = next;
    }

    public Node(Data info)
    {
        Info = info;
        Next = null;
    }

    public Data Info
    {
        get { return info; }
        set
        {
            if (value == null)
                throw new Exception("Cannot assign empty data!");

            info = value;
        }
    }

    public Node<Data> Next
    {
        get { return next; }
        set { next = value; }
    }
}
