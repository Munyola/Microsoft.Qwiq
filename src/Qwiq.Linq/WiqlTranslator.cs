using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Qwiq.Linq.Fragments;
using Microsoft.Qwiq.Linq.WiqlExpressions;

namespace Microsoft.Qwiq.Linq
{
    // TODO: Centralize turning the query into text. Right now it is spread across this class and the TranslatedQuery class

    // WIQL Syntax: http://msdn.microsoft.com/en-US/library/bb130155(v=vs.90).aspx

    /// <summary>
    ///     This class visits the massaged Expression tree generated by the QueryTranslator.
    ///     The massaged tree has expression types for Where, which this class can then walk
    ///     more easily to turn into text. Remember that turning into text is a duty shared
    ///     between this class and the TranslatedQuery class
    /// </summary>
    public class WiqlTranslator : IWiqlTranslator
    {
        protected readonly IFieldMapper FieldMapper;

        public WiqlTranslator()
            : this(new CachingFieldMapper(new SimpleFieldMapper()))
        {
        }

        public WiqlTranslator(IFieldMapper fieldMapper)
        {
            FieldMapper = fieldMapper ?? throw new ArgumentNullException(nameof(fieldMapper));
        }

        protected virtual Translator GetTranslator()
        {
            return new Translator(FieldMapper);
        }

        public TranslatedQuery Translate(Expression expression)
        {
            var translator = GetTranslator();
            translator.Visit(expression);

            var query = translator.Query;

            query.UnderlyingQueryType = query.UnderlyingQueryType ?? TypeSystem.GetElementType(expression.Type);

            if (!query.Projections.Any())
            {
                query.Select = new SelectFragment(FieldMapper.GetFieldNames(query.UnderlyingQueryType).ToList());
            }
            else
            {
                // TODO: Enumerate the projections and build the list of fields
                query.Select = new SelectFragment(FieldMapper.GetFieldNames(query.UnderlyingQueryType).ToList());
            }


        var workItemTypeRestriction = FieldMapper.GetWorkItemType(query.UnderlyingQueryType).ToList();
            if (workItemTypeRestriction.Any())
            {
                query.WhereClauses.Enqueue(new TypeRestrictionFragment(workItemTypeRestriction));
            }

            return query;
        }

        protected class Translator : ExpressionVisitor
        {
            private readonly IFieldMapper _fieldMapper;
            public TranslatedQuery Query;
            private Queue<IFragment> _expressionInProgress;

            public Translator(IFieldMapper fieldMapper)
            {
                _fieldMapper = fieldMapper;
                Query = new TranslatedQuery();
                _expressionInProgress = new Queue<IFragment>();
            }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                {
                    return null;
                }

                switch ((WiqlExpressionType)node.NodeType)
                {
                    case WiqlExpressionType.Select:
                        return VisitSelect((SelectExpression)node);
                    case WiqlExpressionType.Where:
                        return VisitWhere((WhereExpression)node);
                    case WiqlExpressionType.In:
                        return VisitIn((InExpression)node);
                    case WiqlExpressionType.Under:
                        return VisitUnder((UnderExpression)node);
                    case WiqlExpressionType.Order:
                        return VisitOrder((OrderExpression)node);
                    case WiqlExpressionType.AsOf:
                        return VisitAsOf((AsOfExpression)node);
                    case WiqlExpressionType.Contains:
                        return VisitContains((ContainsExpression)node);
                    case WiqlExpressionType.Indexer:
                        return VisitIndexer((IndexerExpression) node);
                    case WiqlExpressionType.WasEver:
                        return VisitWasEver((WasEverExpression)node);
                    case WiqlExpressionType.InGroup:
                        return VisitInGroup((InGroupExpression)node);
                    case WiqlExpressionType.NotInGroup:
                        return VisitNotInGroup((NotInGroupExpression)node);
                    default:
                        return base.Visit(node);
                }
            }

            protected virtual Expression VisitSelect(SelectExpression expression)
            {
                Visit(expression.Select);

                Query.UnderlyingQueryType = Query.UnderlyingQueryType ??
                                            TypeSystem.GetElementType(expression.Select.Type);

                Query.Projections.Add(expression.Projection);
                return expression;
            }

            protected virtual Expression VisitWhere(WhereExpression expression)
            {
                Visit(expression.Source);
                Visit(expression.Filter);

                if (_expressionInProgress.Any())
                {
                    Query.WhereClauses.Enqueue(new CompoundFragment(_expressionInProgress));
                }

                _expressionInProgress = new Queue<IFragment>();

                return expression;
            }

            protected virtual Expression VisitIn(InExpression expression)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(expression.Subject);

                _expressionInProgress.Enqueue(new StringFragment(" IN "));

                Visit(expression.Target);

                _expressionInProgress.Enqueue(new GroupEndFragment());

                return expression;
            }

            protected virtual Expression VisitUnder(UnderExpression expression)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(expression.Subject);

                _expressionInProgress.Enqueue(new StringFragment(" UNDER "));

                Visit(expression.Target);
                _expressionInProgress.Enqueue(new GroupEndFragment());

                return expression;
            }

            protected virtual Expression VisitWasEver(WasEverExpression expression)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(expression.Subject);

                _expressionInProgress.Enqueue(new StringFragment(" EVER "));

                Visit(expression.Target);
                _expressionInProgress.Enqueue(new GroupEndFragment());

                return expression;
            }

            protected virtual Expression VisitInGroup(InGroupExpression expression)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(expression.Subject);

                _expressionInProgress.Enqueue(new StringFragment(" IN GROUP "));

                Visit(expression.Target);
                _expressionInProgress.Enqueue(new GroupEndFragment());

                return expression;
            }

            protected virtual Expression VisitNotInGroup(NotInGroupExpression expression)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(expression.Subject);

                _expressionInProgress.Enqueue(new StringFragment(" NOT IN GROUP "));

                Visit(expression.Target);
                _expressionInProgress.Enqueue(new GroupEndFragment());

                return expression;
            }

            protected virtual Expression VisitOrder(OrderExpression expression)
            {
                Visit(expression.Source);
                Visit(expression.OrderSelector);

                if (expression.Options == OrderOptions.Ascending)
                {
                    _expressionInProgress.Enqueue(new StringFragment(" asc"));
                }
                else if (expression.Options == OrderOptions.Descending)
                {
                    _expressionInProgress.Enqueue(new StringFragment(" desc"));
                }
                else
                {
                    throw new ArgumentException("OrderBy expression does not have a sort order");
                }

                Query.ThenOrderClauses.Enqueue(new CompoundFragment(_expressionInProgress));
                _expressionInProgress = new Queue<IFragment>();

                return expression;
            }

            protected virtual Expression VisitAsOf(AsOfExpression expression)
            {
                Query.AsOfDateTime = expression.AsOfDateTime;

                return expression;
            }

            protected virtual Expression VisitContains(ContainsExpression expression)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(expression.Subject);

                _expressionInProgress.Enqueue(new StringFragment(" CONTAINS "));

                Visit(expression.Target);
                _expressionInProgress.Enqueue(new GroupEndFragment());

                return expression;
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Not:
                        _expressionInProgress.Enqueue(new StringFragment(" NOT "));
                        Visit(node.Operand);
                        break;
                    case ExpressionType.TypeAs:
                        // Casting a type; do nothing because WIQL handles type conversion
                        Visit(node.Operand);
                        break;
                    default:
                        throw new NotSupportedException($"The unary operator '{node.NodeType}' is not supported");
                }

                return node;
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                _expressionInProgress.Enqueue(new GroupStartFragment());
                Visit(node.Left);

                switch (node.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        _expressionInProgress.Enqueue(new StringFragment(" AND "));
                        break;
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        _expressionInProgress.Enqueue(new StringFragment(" OR "));
                        break;
                    case ExpressionType.Equal:
                        _expressionInProgress.Enqueue(new StringFragment(" = "));
                        break;
                    case ExpressionType.NotEqual:
                        _expressionInProgress.Enqueue(new StringFragment(" <> "));
                        break;
                    case ExpressionType.LessThan:
                        _expressionInProgress.Enqueue(new StringFragment(" < "));
                        break;
                    case ExpressionType.LessThanOrEqual:
                        _expressionInProgress.Enqueue(new StringFragment(" <= "));
                        break;
                    case ExpressionType.GreaterThan:
                        _expressionInProgress.Enqueue(new StringFragment(" > "));
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        _expressionInProgress.Enqueue(new StringFragment(" >= "));
                        break;
                    default:
                        throw new NotSupportedException($"The binary operator '{node.NodeType}' is not supported");
                }

                Visit(node.Right);
                _expressionInProgress.Enqueue(new GroupEndFragment());

                return node;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (node.Value is IQueryable)
                {
                    // Assume constant nodes with IQueryables are table references
                    _expressionInProgress = new Queue<IFragment>();
                }
                else if (node.Value == null)
                {
                    _expressionInProgress.Enqueue(new ConstantFragment(string.Empty));
                }
                else
                {
                    switch (node.Value.GetType().FullName)
                    {
                        case "System.String":
                            _expressionInProgress.Enqueue(new ConstantFragment(node.Value.ToString()));
                            break;
                        case "System.DateTime":
                            _expressionInProgress.Enqueue(new DateTimeFragment(((DateTime)node.Value)));
                            break;
                        case "System.Int32[]":
                            var originalIntVals = (int[])node.Value;
                            _expressionInProgress.Enqueue(new NumberListFragment(originalIntVals));
                            break;
                        case "System.String[]":
                        case
                            "System.Linq.Enumerable+WhereArrayIterator`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
                            :
                        case "System.Linq.Enumerable+WhereSelectArrayIterator`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                            _expressionInProgress.Enqueue(
                                new ConstantListFragment((IEnumerable<string>)node.Value));
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Double":
                            _expressionInProgress.Enqueue(new StringFragment(node.Value.ToString()));
                            break;
                        default:
                            throw new NotSupportedException($"The constant for '{node.Value}' is not supported");
                    }
                }

                return node;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != null && (node.Expression.NodeType == ExpressionType.Parameter || node.Expression.NodeType == ExpressionType.Convert))
                {
                    _expressionInProgress.Enqueue(new MemberFragment(_fieldMapper, node.Member.Name));

                    return node;
                }

                if (node.Expression != null && node.NodeType == ExpressionType.MemberAccess && node.Member.Name == "Value")
                {
                    // Node is a obj.Property.Value call. The 'Value' can be ignored. Trim it off and continue.
                    return Visit(node.Expression);
                }

                throw new NotSupportedException($"The member '{node.Member.Name}' is not supported");
            }

            protected Expression VisitIndexer(IndexerExpression node)
            {
                _expressionInProgress.Enqueue(new MemberFragment(_fieldMapper, node.Target.Value.ToString()));

                return node;
            }
        }
    }
}

