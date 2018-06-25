using System;
using System.Collections.Generic;
using System.Text;

namespace FileUploader
{
    public abstract class BaseModel<TResult>
    {
        public static DynamicAccessor<TResult> DynamicAccessor = new DynamicAccessor<TResult>();

        public object GetValue(string name)
        {
            return DynamicAccessor.GetValue(this, name);
        }

        public void SetValue(string name, object value)
        {
            DynamicAccessor.SetValue(this, name, value);
        }

    }
}
