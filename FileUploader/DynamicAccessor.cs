using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FileUploader
{
    public class DynamicAccessor<TResult>
    {
        private Type _type;
        private Func<object, string, object> _getValueDelegate;
        private Action<object, string, object> _setValueDelegate;

        public DynamicAccessor()
        {
            _type = typeof(TResult);
            _getValueDelegate = GenerateGetValue();
            _setValueDelegate = GenerateSetValue();
        }

        public object GetValue(object instance, string memberName)
        {
            return _getValueDelegate(instance, memberName);
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            _setValueDelegate(instance, memberName, newValue == DBNull.Value ? null : newValue);
        }


        private Func<object, string, object> GenerateGetValue()
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var nameHash = Expression.Variable(typeof(int), "nameHash");

            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));
            var cases = new List<SwitchCase>();

            foreach (var propertyInfo in _type.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    var property = Expression.Property(Expression.Convert(instance, _type), propertyInfo.Name);
                    var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

                    cases.Add(Expression.SwitchCase(Expression.Convert(property, typeof(object)), propertyHash));
                }
            }

            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[]{ nameHash }, calHash, switchEx);

            return Expression.Lambda<Func<object, string, object>>(methodBody, instance, memberName).Compile();
        }

        private Action<object, string, object> GenerateSetValue()
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var newValue = Expression.Parameter(typeof(object), "newValue");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));

            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in _type.GetProperties())
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }
                var property = Expression.Property(Expression.Convert(instance, _type), propertyInfo.Name);
                var setValue = Expression.Assign(property, Expression.Convert(newValue, propertyInfo.PropertyType));
                var propertyHash = Expression.Constant(propertyInfo.Name.GetHashCode(), typeof(int));

                cases.Add(Expression.SwitchCase(Expression.Convert(setValue, typeof(object)), propertyHash));
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(new[] { nameHash }, calHash, switchEx);

            return Expression.Lambda<Action<object, string, object>>(methodBody, instance, memberName, newValue).Compile();

        }
    }
}
