using Lab1.Database.DTOs;
using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Mapper
{
    internal static class SimpleMapper
    {
        private readonly struct CompositeTypeKey
        {
            public readonly KeyValuePair<Type, Type> Value;

            public CompositeTypeKey(Type keyA, Type keyB) => Value = new(keyA, keyB);

            public override bool Equals(object obj)
            {
                bool result = false;
                if (obj is CompositeTypeKey compositeKey)
                {
                    result = compositeKey.Value.Key.Equals(Value.Key)
                        && compositeKey.Value.Value.Equals(Value.Value); 
                }

                    return result;
            }

            public override int GetHashCode() => Value.GetHashCode();
        }

        private static readonly Dictionary<CompositeTypeKey, Delegate> _mapFunctions;

        static SimpleMapper()
        {
            _mapFunctions = new();
        }

        private const BindingFlags fieldAccessModifiers = 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static IEnumerable<FieldInfo> GetFieldInfoEnumerable(Type type)
        {
            IEnumerable<FieldInfo> result =
            type.GetFields(BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic);

            if (type.BaseType is not null)
            {
                var baseFields = GetFieldInfoEnumerable(type.BaseType);
                result.Concat(baseFields);
            }

            return result;
        }

        private static FieldInfo TryFindField(IEnumerable<FieldInfo> infos, string fieldName)
        {
            string searchFieldName = fieldName.ToLower();

            foreach (var info in infos)
            {
                string searchName = info.Name.ToLower();

                if (info.Name.StartsWith('_'))
                {
                    searchName = info.Name.Substring(1);
                }
                if (fieldName.StartsWith('_'))
                {
                    searchFieldName = fieldName.Substring(1);
                }

                if (searchFieldName == searchName)
                {
                    return info;
                }
            }

            return null;
        }

        private static LambdaExpression CreateMapFunction<Tin, Tout>()
        {
            List<Expression> assignExpression = new();
            var outType = typeof(Tout);
            var inType = typeof(Tin);
            var newExpression = Expression.New(outType);
            var variable = Expression.Variable(outType, "outerVar");
            var inVariable = Expression.Variable(inType, "innerVar");
            var assignVarToNew = Expression.Assign(variable, newExpression);

            assignExpression.Add(assignVarToNew);
            var inInstanceFields = GetFieldInfoEnumerable(inType);
            var outInstanceFileds = GetFieldInfoEnumerable(outType);

            foreach (var inField in inInstanceFields)
            {
                var existingOutInstanceField = TryFindField(outInstanceFileds, inField.Name);

                if (existingOutInstanceField is not null)
                {
                    var outInstanceVariable = Expression.Field(variable, existingOutInstanceField);
                    var field = Expression.Field(inVariable, inField.Name);

                    var assign = Expression.Assign(outInstanceVariable, field);
                    assignExpression.Add(assign);
                }
            }

            assignExpression.Add(variable);
            var blockExpression = Expression.Block(new[] { variable }, assignExpression);

            var lambda = Expression.Lambda(blockExpression, inVariable);

            return lambda;
        }


        // todo: finish this
        public static void Map<Tin, Tout>(Tin inInstance,  out Tout outInstance)
        {
            var inInstanceType = typeof(Tin);
            var outInstanceType = typeof(Tout);

           
            var mapAttributes = Attribute.GetCustomAttributes(typeof(Tin))
            .Where(atribute => atribute.GetType() == typeof(MappableAttribute));

            var currentMapAttribute = mapAttributes.FirstOrDefault(attribute => attribute.)

            var typeParamter = mapAttribute.get;
            var typeParameterValue = (Type)typeParamter.TypedValue.Value;
            //ArgumentOutOfRangeException.ThrowIfNotEqual<Type>(typeParameterValue, outInstanceType);
            if(typeParameterValue != outInstanceType)
            {
                throw new ArgumentOutOfRangeException();
            }

            var compositeKey = new CompositeTypeKey(inInstanceType, outInstanceType);

            Func<Tin, Tout> mapFunc;

            if (_mapFunctions.TryGetValue(compositeKey, out var mapFunction))
            {
                mapFunc = (Func<Tin, Tout>)mapFunction;
            }
            else
            {
                var newMapFunction = CreateMapFunction<Tin, Tout>();
                var @delegate = newMapFunction.Compile();

                _mapFunctions.Add(compositeKey, @delegate);
                mapFunc = (Func<Tin, Tout>)mapFunction;
            }

            outInstance = mapFunc(inInstance);
        }
    }
}
