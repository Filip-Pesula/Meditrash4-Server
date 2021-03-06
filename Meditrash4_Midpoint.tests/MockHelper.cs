using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint.tests
{
    internal class MockHelper
    {

        public static void SetPropertyValue(object target, string memberName, object newValue)
        {
            PropertyInfo prop = GetPropertyReference(target.GetType(), memberName);
            prop.SetValue(target, newValue, null);
        }
        private static PropertyInfo GetPropertyReference(Type targetType, string memberName)
        {
            PropertyInfo propInfo = targetType.GetProperty(memberName,
                                                  BindingFlags.Public |
                                                  BindingFlags.NonPublic |
                                                  BindingFlags.Instance);

            if (propInfo == null && targetType.BaseType != null)
            {
                //if the member isn't actually on the type we're working on, rather it's
                //defined in a base class as private, it won't be returned in the above call,
                //so we have to walk the type hierarchy until we find it.
                // See: http://agsmith.wordpress.com/2007/12/13/where-are-my-fields/

                return GetPropertyReference(targetType.BaseType, memberName);

            }
            return propInfo;
        }
    }

}
