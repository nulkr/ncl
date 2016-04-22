using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ncl
{
    namespace Equipment
    {
        /// Recipe PropertyDescriptor Class for PropertyGrid
        /// http://www.codeproject.com/Articles/9280/Add-Remove-_Items-to-from-PropertyGrid-at-Runtime
        public class RecipePropertyDescriptor : PropertyDescriptor
        {
            #region field

            private RecipeProperty _Property;

            #endregion field

            #region constructor

            public RecipePropertyDescriptor(ref RecipeProperty property, Attribute[] attrs)
                : base(property.Name, attrs)
            {
                _Property = property;
            }

            #endregion constructor

            #region override PropertyDescriptor

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get { return null; }
            }

            public override string Description //설명 내용
            {
                get { return _Property.Value.Comment; }
            }

            public override string Category
            {
                get { return _Property.Value.Category; }
            }

            public override string DisplayName //설명 제목
            {
                get { return _Property.Name; }
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override void ResetValue(object component)
            {
                //Have to implement
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            public override object GetValue(object component)
            {
                var item = _Property.Value as RecipeItem;

                if (item.IsBool())
                    return item.AsBool;
                else if (item.IsInt())
                    return item.AsInt;
                else if (item.IsString())
                    return item.Text;
                else
                    return item.Value;
            }

            public override void SetValue(object component, object value)
            {
                var item = _Property.Value as RecipeItem;

                if (item.IsBool())
                    item.AsBool = (bool)value;
                else if (item.IsInt())
                    item.AsInt = (int)value;
                else if (item.IsString())
                    item.Text = (string)value;
                else
                    item.Value = (double)value;
            }

            public override Type PropertyType
            {
                get
                {
                    var item = _Property.Value as RecipeItem;

                    if (item.IsBool())
                        return typeof(bool);
                    else if (item.IsInt())
                        return typeof(int);
                    else if (item.IsString())
                        return typeof(string);
                    else
                        return typeof(double);
                }
            }

            #endregion override PropertyDescriptor
        }

        /// RecipeItem Property := (Name + Value)
        public class RecipeProperty
        {
            #region field

            private string _Name = string.Empty;
            private RecipeItem _Value = null;

            #endregion field

            #region property

            public string Name
            {
                get { return _Name; }
            }

            public RecipeItem Value
            {
                get { return _Value; }
                set { _Value = value; }
            }

            #endregion property

            #region constructor

            public RecipeProperty(string sName, RecipeItem oValue)
            {
                _Name = sName;
                _Value = oValue;
            }

            #endregion constructor
        }

        /// Recipe TypeDescriptor
        public class RecipeTypeDescriptor : ICustomTypeDescriptor
        {
            #region field

            private List<RecipeProperty> _List = new List<RecipeProperty>();

            #endregion field

            #region property

            public RecipeProperty this[int index]
            {
                get { return _List[index]; }
                set { _List[index] = (RecipeProperty)value; }
            }

            #endregion property

            #region method

            public void Clear()
            {
                _List.Clear();
            }

            public void Add(string sName, RecipeItem item)
            {
                _List.Add(new RecipeProperty(sName, item));
            }

            #endregion method

            #region ICustomTypeDescriptor Implementation

            /// Get Class Name
            /// <returns>String</returns>
            public String GetClassName()
            {
                return TypeDescriptor.GetClassName(this, true);
            }

            /// GetAttributes
            /// <returns>AttributeCollection</returns>
            public AttributeCollection GetAttributes()
            {
                return TypeDescriptor.GetAttributes(this, true);
            }

            /// GetComponentName
            /// <returns>String</returns>
            public String GetComponentName()
            {
                return TypeDescriptor.GetComponentName(this, true);
            }

            /// GetConverter
            /// <returns>TypeConverter</returns>
            public TypeConverter GetConverter()
            {
                return TypeDescriptor.GetConverter(this, true);
            }

            /// GetDefaultEvent
            /// <returns>EventDescriptor</returns>
            public EventDescriptor GetDefaultEvent()
            {
                return TypeDescriptor.GetDefaultEvent(this, true);
            }

            /// GetDefaultProperty
            /// <returns>PropertyDescriptor</returns>
            public PropertyDescriptor GetDefaultProperty()
            {
                return TypeDescriptor.GetDefaultProperty(this, true);
            }

            /// GetEditor
            /// <param name="editorBaseType">editorBaseType</param>
            /// <returns>object</returns>
            public object GetEditor(Type editorBaseType)
            {
                return TypeDescriptor.GetEditor(this, editorBaseType, true);
            }

            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return TypeDescriptor.GetEvents(this, attributes, true);
            }

            public EventDescriptorCollection GetEvents()
            {
                return TypeDescriptor.GetEvents(this, true);
            }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                PropertyDescriptor[] newProps = new PropertyDescriptor[_List.Count];
                for (int i = 0; i < _List.Count; i++)
                {
                    RecipeProperty prop = (RecipeProperty)this[i];
                    newProps[i] = new RecipePropertyDescriptor(ref prop, attributes);
                }

                return new PropertyDescriptorCollection(newProps);
            }

            public PropertyDescriptorCollection GetProperties()
            {
                return TypeDescriptor.GetProperties(this, true);
            }

            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                return this;
            }

            #endregion ICustomTypeDescriptor Implementation
        }
    }
}