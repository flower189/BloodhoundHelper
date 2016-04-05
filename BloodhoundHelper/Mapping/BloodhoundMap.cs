using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BloodhoundHelper.Mapping
{
    public abstract class BloodhoundMap<T> : BloodhoundMap
    {

        List<MapInfo> _tokenMapInfos = new List<MapInfo>();
        List<MapInfo> _dataMapInfos = new List<MapInfo>();
        private MapInfo _valueMapInfo = null;

        public void Token(Expression<Func<T, object>> expression, string format = null)
        {
            PropertyInfo propertyInfo = GetPropertyFromExpression(expression);
            var mapInfo = new MapInfo(propertyInfo, format);
            if (_tokenMapInfos.SingleOrDefault(x => x.PropertyInfo == mapInfo.PropertyInfo) == null)
            {
                _tokenMapInfos.Add(mapInfo);
            }
        }

        public void Data(Expression<Func<T, object>> expression, string format = null, string name = null)
        {
            PropertyInfo propertyInfo = GetPropertyFromExpression(expression);
            var mapInfo = new MapInfo(propertyInfo, format, name);
            if (_dataMapInfos.SingleOrDefault(x => x.PropertyInfo == mapInfo.PropertyInfo) == null)
            {
                _dataMapInfos.Add(mapInfo);
            }
        }

        public void Value(Expression<Func<T, object>> expression, string format = null)
        {
            PropertyInfo propertyInfo = GetPropertyFromExpression(expression);
            if (_valueMapInfo == null)
            {
                _valueMapInfo = new MapInfo(propertyInfo, format);
            }
            else
            {
                throw new Exception("Only one Value is permitted per type.");
            }
        }

        internal override EntityInfo BuildEntityInfo()
        {
            Type type = typeof(T);
            EntityInfo info = new EntityInfo(type);
            info.TokenPropertyInfos = _tokenMapInfos;
            info.DataPropertyInfos = _dataMapInfos;
            info.ValuePropertyInfos.Add(_valueMapInfo);
            return info;
        }

        private PropertyInfo GetPropertyFromExpression(Expression<Func<T, object>> GetPropertyLambda)
        {
            MemberExpression Exp = null;

            //this line is necessary, because sometimes the expression comes in as Convert(originalexpression)
            if (GetPropertyLambda.Body is UnaryExpression)
            {
                var UnExp = (UnaryExpression)GetPropertyLambda.Body;
                if (UnExp.Operand is MemberExpression)
                {
                    Exp = (MemberExpression)UnExp.Operand;
                }
                else
                    throw new ArgumentException();
            }
            else if (GetPropertyLambda.Body is MemberExpression)
            {
                Exp = (MemberExpression)GetPropertyLambda.Body;
            }
            else
            {
                throw new ArgumentException();
            }

            return (PropertyInfo)Exp.Member;
        }

    }
}
