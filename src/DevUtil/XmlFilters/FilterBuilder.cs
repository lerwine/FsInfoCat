using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevUtil.XmlFilters
{
    public static class FilterBuilder
    {
        public static ParameterExpression ElementParameter1 { get; } = Expression.Parameter(typeof(XElement), "e1");

        public static ParameterExpression AttributeValue1 = Expression.Variable(typeof(XAttribute), "a1");

        public static MethodCallExpression InvokeAttributeMethod(Expression elementExpr, XName name) => Expression.Call(elementExpr, typeof(XElement).GetMethod(nameof(XElement.Attribute), new[] { typeof(XName) }),
            Expression.Constant(name));

        public static BinaryExpression AssignAttributeMethodResult(Expression elementExpr, ParameterExpression attributeVar, XName name) => Expression.Assign(attributeVar, InvokeAttributeMethod(elementExpr, name));

        public static MemberExpression GetAttributeValueProperty(Expression attributeExpr) => Expression.Property(attributeExpr, nameof(XAttribute.Value));

        public static BinaryExpression AssignAttributeValueProperty(Expression attributeExpr, ParameterExpression valueVar) => Expression.Assign(valueVar, GetAttributeValueProperty(attributeExpr));

        public static AttributeExistsBuilder AttributeExists(XName name) => new AttributeExistsBuilder(name);

        public static AttributeNotExistsBuilder AttributeNotExists(XName name) => new AttributeNotExistsBuilder(name);

        public static AttributeEqualsBuilder AttributeEquals(XName name, string value) => new AttributeExistsBuilder(name).AndEqualTo(value);

        public static AttributeNotEqualsBuilder AttributeNotEquals(XName name, string value) => new AttributeNotExistsBuilder(name).OrNotEqualTo(value);

        public class AttributeExistsBuilder
        {
            public ParameterExpression ElementParameter { get; } = Expression.Parameter(typeof(XElement));

            public ParameterExpression AttributeVariable { get; } = Expression.Variable(typeof(XAttribute));

            public BinaryExpression InitializeAttributeVar { get; }

            public BinaryExpression ConditionalExpression { get; }

            public AttributeExistsBuilder(XName name)
            {
                InitializeAttributeVar = AssignAttributeMethodResult(ElementParameter, AttributeVariable, name);
                ConditionalExpression = Expression.NotEqual(AttributeVariable, Expression.Constant(null));
            }

            public AttributeEqualsBuilder AndEqualTo(string value) => new AttributeEqualsBuilder(this, value);

            public AttributeNotEqualsBuilder AndNotEqualTo(string value) => new AttributeNotEqualsBuilder(this, value);

            public Func<XElement, bool> Build() => Expression.Lambda<Func<XElement, bool>>(Expression.Block(InitializeAttributeVar, ConditionalExpression), ElementParameter).Compile();
        }

        public class AttributeNotExistsBuilder
        {
            public ParameterExpression ElementParameter { get; } = Expression.Parameter(typeof(XElement));

            public ParameterExpression AttributeVariable { get; } = Expression.Variable(typeof(XAttribute));

            public BinaryExpression InitializeAttributeVar { get; }

            public BinaryExpression ConditionalExpression { get; }

            public AttributeNotExistsBuilder(XName name)
            {
                InitializeAttributeVar = AssignAttributeMethodResult(ElementParameter, AttributeVariable, name);
                ConditionalExpression = Expression.NotEqual(AttributeVariable, Expression.Constant(null));
            }

            public AttributeEqualsBuilder OrEqualTo(string value) => new AttributeEqualsBuilder(this, value);

            public AttributeNotEqualsBuilder OrNotEqualTo(string value) => new AttributeNotEqualsBuilder(this, value);

            public Func<XElement, bool> Build() => Expression.Lambda<Func<XElement, bool>>(Expression.Block(InitializeAttributeVar, ConditionalExpression), ElementParameter).Compile();
        }

        public class AttributeEqualsBuilder
        {
            public ParameterExpression ElementParameter { get; }

            public ParameterExpression AttributeVariable { get; }

            public BinaryExpression InitializeAttributeVar { get; }

            public BinaryExpression ConditionalExpression { get; }

            public AttributeEqualsBuilder(AttributeExistsBuilder conditional, string value)
            {
                ElementParameter = conditional.ElementParameter;
                AttributeVariable = conditional.AttributeVariable;
                InitializeAttributeVar = conditional.InitializeAttributeVar;
                ConditionalExpression = Expression.AndAlso(conditional.ConditionalExpression, Expression.Equal(Expression.Constant(value), GetAttributeValueProperty(conditional.AttributeVariable)));
            }

            public AttributeEqualsBuilder(AttributeNotExistsBuilder conditional, string value)
            {
                ElementParameter = conditional.ElementParameter;
                AttributeVariable = conditional.AttributeVariable;
                ConditionalExpression = Expression.OrElse(conditional.ConditionalExpression, Expression.Equal(Expression.Constant(value), GetAttributeValueProperty(conditional.AttributeVariable)));
            }

            public Func<XElement, bool> Build() => Expression.Lambda<Func<XElement, bool>>(Expression.Block(InitializeAttributeVar, ConditionalExpression), ElementParameter).Compile();
        }

        public class AttributeNotEqualsBuilder
        {
            public ParameterExpression ElementParameter { get; }

            public ParameterExpression AttributeVariable { get; }

            public BinaryExpression InitializeAttributeVar { get; }

            public BinaryExpression ConditionalExpression { get; }

            public AttributeNotEqualsBuilder(AttributeExistsBuilder conditional, string value)
            {
                ElementParameter = conditional.ElementParameter;
                AttributeVariable = conditional.AttributeVariable;
                InitializeAttributeVar = conditional.InitializeAttributeVar;
                ConditionalExpression = Expression.AndAlso(conditional.ConditionalExpression, Expression.NotEqual(Expression.Constant(value), GetAttributeValueProperty(conditional.AttributeVariable)));
            }
            public AttributeNotEqualsBuilder(AttributeNotExistsBuilder conditional, string value)
            {
                ElementParameter = conditional.ElementParameter;
                AttributeVariable = conditional.AttributeVariable;
                InitializeAttributeVar = conditional.InitializeAttributeVar;
                ConditionalExpression = Expression.OrElse(conditional.ConditionalExpression, Expression.NotEqual(Expression.Constant(value), GetAttributeValueProperty(conditional.AttributeVariable)));
            }

            public Func<XElement, bool> Build() => Expression.Lambda<Func<XElement, bool>>(Expression.Block(InitializeAttributeVar, ConditionalExpression), ElementParameter).Compile();
        }
    }
}
