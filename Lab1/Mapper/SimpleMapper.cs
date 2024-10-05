using Lab1.Database.DTOs;
using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Mapper;

/// <summary>
/// This is conventional mapper.
/// All ref-ce types are non-deep copied from object on which .Map method was called.
/// Mapper is case-insensetive and absorbs '_' prefix of filed name. 
/// </summary>

internal static class SimpleMapper
{
    private readonly struct CompositeTypeKey
    {
        public readonly KeyValuePair<Type, Type> Pair;

        public CompositeTypeKey(Type keyA, Type keyB) => Pair = new(keyA, keyB);

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is CompositeTypeKey compositeKey)
            {
                result = compositeKey.Pair.Key.Equals(Pair.Key)
                    && compositeKey.Pair.Value.Equals(Pair.Value); 
            }

                return result;
        }

        public override int GetHashCode() => Pair.GetHashCode();
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
            result = result.Concat(baseFields);
        }

        return result;
    }

    private static FieldInfo TryFindField(IEnumerable<FieldInfo> infos, string fieldName)
    {
        string searchFieldName = fieldName.ToUpperInvariant();
        
        if (fieldName.StartsWith('_'))
        {
            searchFieldName = searchFieldName.Substring(1);
        }

        foreach (var info in infos)
        {
            string searchName = info.Name.ToUpperInvariant();

            if (info.Name.StartsWith('_'))
            {
                searchName = searchName.Substring(1);
            }

            if (searchFieldName == searchName)
            {
                return info;
            }
        }

        return null;
    }

    private static LambdaExpression CreateMapFunction<Tin>(Tin instance, Type outType)
    {
        List<Expression> assignExpression = new();

        var inType = typeof(Tin);
        NewExpression newExpression = Expression.New(outType);

        var variable = Expression.Variable(outType, "outerVar");
        var inVariable = Expression.Variable(inType, "innerVar");
        var assignVarToNew = Expression.Assign(variable, newExpression);

        assignExpression.Add(assignVarToNew);
        var inInstanceFields = GetFieldInfoEnumerable(inType).ToArray();
        var outInstanceFileds = GetFieldInfoEnumerable(outType).ToArray();

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

    public static void Map<Tin, Tout>(Tin inInstance, out Tout outInstance)
    {
        var inInstanceType = inInstance.GetType();
        var outInstanceBaseType = typeof(Tout);
        var outInstanceType = Resolver.TryResolveType(inInstance);

        if(outInstanceType is null)
        {
            outInstance = default;
        }
        else
        {
            var compositeKey = new CompositeTypeKey(inInstanceType, outInstanceType);

            Func<Tin, Tout> mapFunc;

            if (_mapFunctions.TryGetValue(compositeKey, out var mapFunction))
            {
                mapFunc = (Func<Tin, Tout>)mapFunction;
            }
            else
            {
                var currentMapAttribute = Attribute
                    .GetCustomAttributes(typeof(Tin))
                    .Where(attribute => attribute.GetType() == typeof(MappableAttribute))
                    .Select(attribute => (MappableAttribute)attribute)
                    .FirstOrDefault(attribute => attribute.ToType == outInstanceBaseType);

                if (currentMapAttribute.ToType != outInstanceBaseType)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var newMapFunction = CreateMapFunction<Tin>(inInstance, outInstanceType);
                var @delegate = newMapFunction.Compile();

                _mapFunctions.Add(compositeKey, @delegate);
                mapFunc = (Func<Tin, Tout>)@delegate;
            }

            outInstance = mapFunc(inInstance);
        }
    }
}
