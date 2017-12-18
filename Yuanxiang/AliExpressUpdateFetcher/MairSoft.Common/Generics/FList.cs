//----------------------------------------------------------------------------
// FList.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2006 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// List template class, ie using Generics so we can create a list for any
// class type.

// REVISION HISTORY:
// Date          Author            Changes
// 14 Feb 2006   Francis Mair      1st implementation

//---------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FSuite.Generics
{
   /// <summary>
   /// Class definition for our generic list class
   /// </summary>
   /// <typeparam name="T"></typeparam>
   [Serializable]
   public class FList<T> : CollectionBase
   {
      /// <summary>
      /// Constructor
      /// </summary>
      public FList()
      {

      }

      /// <summary>
      /// Indexer
      /// </summary>
      /// <param name="pos"></param>
      /// <returns></returns>
      public T this[int pos]
      {
         get { return (T)List[pos]; }
      }

      /// <summary>
      /// Adds new item of specified type
      /// </summary>
      /// <param name="item"></param>
      public void Add(T item)
      {
         // use base class to process actual collection operation
         List.Add(item);                  
      }

      /// <summary>
      /// Replaces the specified event with the new event
      /// </summary>
      /// <param name="oldItem"></param>
      /// <param name="newItem"></param>
      public void Replace(T oldItem,T newItem)
      {
         int pos = 0;
         foreach(T item in List)
         {
            if(item.Equals(oldItem))
               break;
            pos++;
         }

         // insert new event
         List.Insert(pos,newItem);

         // remove old event
         List.Remove(oldItem);
      }
   }
}

//----------------------------------------------------------------------------
