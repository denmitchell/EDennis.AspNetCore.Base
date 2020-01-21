using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.Serialization {

    //https://stackoverflow.com/a/41785168
    public class AnonymousTypes<TEntity> {


        public static Dictionary<string, PropertyInfo> ClassProperties { get; }
        public static Dictionary<string, PropertyInfo> ClassPropertiesLowerCase { get; }

        public static ConcurrentDictionary<string, Type> TypeDictionary { get; }

        /// <summary>
        /// Statically initializes the ClassProperties via reflection
        /// </summary>
        static AnonymousTypes() {
            ClassProperties = new Dictionary<string, PropertyInfo>();
            ClassPropertiesLowerCase = new Dictionary<string, PropertyInfo>();
            var props = typeof(TEntity).GetProperties();
            foreach (var prop in props) {
                ClassProperties.Add(prop.Name, prop);
                ClassPropertiesLowerCase.Add(prop.Name.ToLower(), prop);
            }
            TypeDictionary = new ConcurrentDictionary<string, Type>();
        }


        public static PagedResult<object> DeserializePagedResult(string json) {
            if (json == null)
                return null;
            using var document = JsonDocument.Parse(json);
            var pagedResult = new PagedResult<object> {
                PagingData = new PagingData()
            };

            foreach (var prop in document.RootElement.EnumerateObject()) {
                if (prop.Name.Equals("pagingData", StringComparison.OrdinalIgnoreCase)) {
                    foreach (var prop2 in prop.Value.EnumerateObject()) {
                        if (prop2.Name.Equals("recordCount", StringComparison.OrdinalIgnoreCase))
                            pagedResult.PagingData.RecordCount = prop2.Value.GetInt32();
                        else if (prop2.Name.Equals("pageCount", StringComparison.OrdinalIgnoreCase))
                            pagedResult.PagingData.PageCount = prop2.Value.GetInt32();
                        else if (prop2.Name.Equals("pageNumber", StringComparison.OrdinalIgnoreCase))
                            pagedResult.PagingData.PageNumber = prop2.Value.GetInt32();
                        else if (prop2.Name.Equals("pageSize", StringComparison.OrdinalIgnoreCase))
                            pagedResult.PagingData.PageSize = prop2.Value.GetInt32();
                        else if (prop2.Name.Equals("pageSize", StringComparison.OrdinalIgnoreCase))
                            pagedResult.PagingData.PageSize = prop2.Value.GetInt32();
                    }
                } else if (prop.Name.Equals("data", StringComparison.OrdinalIgnoreCase)) {
                    var objList = new List<object>();
                    Type anonType = null;
                    foreach (var obj in prop.Value.EnumerateArray())
                        objList.Add(DeserializeObject(obj.GetRawText(), ref anonType));
                    pagedResult.Data = objList;
                }
            }
            return pagedResult;
        }



        public static List<object> DeserializeList(string json) {
            if (json == null)
                return null;
            using var document = JsonDocument.Parse(json);
            var array = document.RootElement.EnumerateArray();

            var objList = new List<object>();
            Type anonType = null;
            foreach (var obj in array) {
                objList.Add(DeserializeObject(obj.GetRawText(),ref anonType));
            }
            return objList;
        }


        public static object DeserializeObject(string json, ref Type anonType) {
            if (json == null)
                return null;
            if (anonType == null) {
                using var document = JsonDocument.Parse(json);
                var props = document.RootElement.EnumerateObject().Select(x => x.Name.ToLower()).OrderBy(p => p);
                if (props.Count() == 0)
                    return null;
                var propsKey = string.Join('|', props);
                anonType = TypeDictionary.GetOrAdd(propsKey, CreateNewType(propsKey, props));
            }
            return JsonSerializer.Deserialize(json, anonType);
        }





        public static Type CreateNewType(string typeName, IEnumerable<string> dynamicProps) {
            PropertyInfo[] properties = ClassPropertiesLowerCase.Where(p => dynamicProps.Any(f => f == p.Key)).Select(f => f.Value).ToArray();
            var myTypeInfo = CompileResultTypeInfo(typeName, properties);
            var myType = myTypeInfo.AsType();
            return myType;
        }


        public static TypeInfo CompileResultTypeInfo(string typeName, PropertyInfo[] properties) {
            TypeBuilder tb = GetTypeBuilder(typeName);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var fieldDescriptors = properties.Select(p =>
                new FieldDescriptor(p.Name, p.PropertyType)).ToList();

            var yourListOfFields = fieldDescriptors;

            foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType);

            TypeInfo objectTypeInfo = tb.CreateTypeInfo();
            return objectTypeInfo;

        }


        private static TypeBuilder GetTypeBuilder(string typeName) {
            var typeSignature = typeName;
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                   null);
            return tb;

        }


        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType) {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });


            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);

        }

    }


    public class FieldDescriptor {

        public FieldDescriptor(string fieldName, Type fieldType) {
            FieldName = fieldName;
            FieldType = fieldType;
        }

        public string FieldName { get; }
        public Type FieldType { get; }

    }



}

