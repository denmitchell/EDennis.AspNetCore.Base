using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.Serialization {

    /// <summary>
    /// Class for generating an inferred anonymous type from a dynamic type.
    /// ***<b>NOTE: This is to be used for testing scenarios only.</b>***
    /// </summary>
    ///
    public class WillyNilly {


        /// <summary>
        /// Entry point for generating a projection type.  Provide
        /// the subset of properties (property names) to be used for
        /// the projection type, and it will be generated.
        /// </summary>
        /// <param name="dynamicProps">property names (subset of TEntity property names)</param>
        /// <returns></returns>
        public static Type InferType(dynamic dynObj) {
            Type type = dynObj.GetType();
            PropertyInfo[] dynamicProps = type.GetProperties();
            var dict = GetInferredPropertyTypes(dynamicProps, dynObj);

            var key = Guid.NewGuid().ToString();
            return CreateNewType(key, dict);
        }

        public static Dictionary<string,Type> GetInferredPropertyTypes(PropertyInfo[] props, dynamic dynObj) {
            var dict = new Dictionary<string, Type>();
            foreach (var prop in props) {
                var value = prop.GetValue(dynObj).ToString();
                if (DateTime.TryParse(value, out DateTime _))
                    dict.Add(prop.Name, typeof(DateTime));
                else if (TimeSpan.TryParse(value, out TimeSpan _))
                    dict.Add(prop.Name, typeof(TimeSpan));
                else if (TimeSpan.TryParse(value, out TimeSpan _))
                    dict.Add(prop.Name, typeof(TimeSpan));
                else if (decimal.TryParse(value, out decimal _))
                    dict.Add(prop.Name, typeof(decimal));
                else if (long.TryParse(value, out long _))
                    dict.Add(prop.Name, typeof(long));
                else if (bool.TryParse(value, out bool _))
                    dict.Add(prop.Name, typeof(bool));
                else
                    dict.Add(prop.Name, typeof(string));

            }
            return dict;
        }



        /// <summary>
        /// Creates a new "projection" type with the provided key,
        /// based upon the provided list of property names
        /// 
        /// adapted from https://stackoverflow.com/a/41785168
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private static Type CreateNewType(string key, Dictionary<string,Type> dict) {
            var typeInfo = CompileResultTypeInfo(key, dict);
            var type = typeInfo.AsType();
            return type;
        }


        /// <summary>
        /// from https://stackoverflow.com/a/41785168
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static TypeInfo CompileResultTypeInfo(string typeName, Dictionary<string,Type> dict) {
            TypeBuilder tb = GetTypeBuilder(typeName);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var fieldDescriptors = dict.Select(p =>
                new FieldDescriptor(p.Key, p.Value)).ToList();

            var fieldList = fieldDescriptors;

            foreach (var field in fieldList)
                CreateProperty(tb, field.FieldName, field.FieldType);

            TypeInfo objectTypeInfo = tb.CreateTypeInfo();
            return objectTypeInfo;

        }


        /// <summary>
        /// from https://stackoverflow.com/a/41785168
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
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
                    TypeAttributes.AutoLayout|
                    TypeAttributes.SpecialName,
                   typeof(Projection));
            return tb;

        }


        /// <summary>
        /// from https://stackoverflow.com/a/41785168
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
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



}

