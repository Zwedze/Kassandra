using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Kassandra.Core.Components
{
    /// http://stackoverflow.com/questions/9601707/how-to-set-property-value-using-expressions
    public static class LambdaExtensions
    {
        public static void SetPropertyValue<T>(this T target, Expression<Func<T, object>> memberLamda, object value)
        {
            MemberExpression memberExpression;
            if ((memberLamda.Body is UnaryExpression))
            {
                memberExpression = (memberLamda.Body as UnaryExpression).Operand as MemberExpression;
            }
            else
            {
                memberExpression = memberLamda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                PropertyInfo property = memberExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }

        public static Type GetExpressionType<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression;
            if ((expression.Body is UnaryExpression))
            {
                memberExpression = (expression.Body as UnaryExpression).Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            return memberExpression.Member is MethodInfo
                ? ((MethodInfo) memberExpression.Member).ReturnType
                : ((PropertyInfo) memberExpression.Member).PropertyType;
        }
    }
}