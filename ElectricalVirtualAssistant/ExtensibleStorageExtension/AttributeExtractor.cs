using System;
using System.Reflection;

namespace EVA_S.ExtensibleStorageExtension
{
    internal class AttributeExtractor<TAttribute> where TAttribute : Attribute
    {
        public TAttribute GetAttribute(MemberInfo memberInfo)
        {
            //Получение пользовательских аттррибутов
            var attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), false);

            if (attributes.Length == 0)
                throw new InvalidOperationException(string.Format("MemberInfo {0} does not have a {1}", memberInfo, typeof(TAttribute)));
            
            var attribute = attributes[0] as TAttribute;
            if (attribute == null)
                throw new InvalidOperationException(string.Format("MemberInfo {0} does not have a {1}", memberInfo, typeof(TAttribute)));

            return attribute;
        }
    }
}
