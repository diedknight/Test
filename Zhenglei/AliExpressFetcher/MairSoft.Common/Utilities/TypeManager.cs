//===============================================================================
// 
// MairSoft.Common
// 
// Copyright (c) 2006 Francis Mair (frank@mair.net.nz)
//
// Description:
// This class is a base class which can be derived from to provide type safe
// management of a certain class type. This can be useful if we have a number
// of different objects of the same base type and need a central mechanism to keep
// track of them, and to retrieve them by their derived types.
// NOTE: There can only be one object of any type in our type manager.
//
//===============================================================================

using System;
using System.Collections;
using System.Reflection;
using MairSoft.Common.Utilities;

namespace MairSoft.Common
{
    /// <summary>
    /// Summary description for TypeManager.
    /// </summary>
    public class TypeManager
    {
        ArrayList objectList = new ArrayList();
        Hashtable objectTable = new Hashtable();
		
        /// <summary>
        /// Constructor
        /// </summary>
        public TypeManager()
        {
        }
						
        /// <summary>
        /// Add a new model to our internal list
        /// </summary>
        /// <param name="newObject"></param>
        public void AddType(object newObject)
        {
            // add to our internal list
            objectList.Add(newObject);
        }
		
        /// <summary>
        /// Add an array of objects to our internal list
        /// </summary>
        /// <param name="newObjects"></param>
        public void AddTypes(object[] newObjects)
        {
            foreach(object newObject in newObjects)
                AddType(newObject);
        }
			
        /// <remarks>
        /// Requests a specific object, may return null if model is not found
        /// </remarks>
        public object GetType(Type objectType)
        {
            object obj = objectTable[objectType];
            if(obj != null)
                return obj;
			
            foreach(object findObject in objectList)
            {
                if(EntityUtils.IsInstanceOfType(objectType,findObject.GetType()))
                {
                        objectTable[objectType] = findObject;
                    return findObject;
                }
            }
			
            return null;
        }

        /// <summary>
        /// Returns the list of types
        /// </summary>
        public ArrayList TypeList
        {
            get { return objectList; }
        }

        /// <summary>
        /// This method is a utility method for us to quickly add all sealed types
        /// derived from a specified base type and located in a certain namespace
        /// to our managed internal list quickly and easily. The advantage of doing
        /// this is that if other users decided to add new classes to the same
        /// namespace they will automatically be added to our internal list.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="requiredNamespace"></param>
        /// <returns></returns>
        public int AddTypes(Type baseType,string requiredNamespace)
        {
            int count = 0;

            Assembly assembly = Assembly.GetAssembly(baseType);

            Type[] types = assembly.GetTypes();

            foreach(Type type in types)
            {
                if(type.Namespace==null)
                    continue;
                // type needs to be in specified namespace
                // type needs to be derived from specified base type
                // type needs to be a sealed class (prevents us from instantiating base classes)
                if((type.Namespace.IndexOf(requiredNamespace) != -1) &&
                    (EntityUtils.IsInstanceOfType(baseType,type)) &&
                    (type.IsSealed == true))
                {
                    object newObject = Activator.CreateInstance(type);

                    AddType(newObject);   
                
                    count++;
                }
            }

            return count;
        }
    }
}

//===============================================================================
