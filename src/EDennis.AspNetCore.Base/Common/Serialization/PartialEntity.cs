using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.Serialization {

    /// <summary>
    /// Wraps an object and records which class properties are
    /// active for the object ("ObjectProperties").
    /// 
    /// This object is mainly used for serialization purposes --
    /// for example, when it is required to output only certain
    /// properties of an object or a list of objects.
    /// </summary>
    /// <typeparam name="TEntity">The underlying type</typeparam>
    public class PartialEntity<TEntity>
        where TEntity : class, new() {

        /// <summary>
        /// The underlying entity.  Populate this object via
        /// Load, Create, or CreateList methods.
        /// </summary>
        public TEntity Entity { get; internal set; } = new TEntity();

        /// <summary>
        /// All properties of the underlying Entity type
        /// </summary>
        public static Dictionary<string, PropertyInfo> ClassProperties { get; }

        public static Dictionary<string, PropertyInfo> ClassPropertiesLowerCase { get; }

        private static JsonSerializerOptions jsonSerializerOptions;

        /// <summary>
        /// Active properties of the current object
        /// </summary>
        public List<string> ObjectProperties { get; private set; } = new List<string>();


        /// <summary>
        /// Statically initializes the ClassProperties via reflection
        /// </summary>
        static PartialEntity() {
            ClassProperties = new Dictionary<string, PropertyInfo>();
            ClassPropertiesLowerCase = new Dictionary<string, PropertyInfo>();
            var props = typeof(TEntity).GetProperties();
            foreach (var prop in props) {
                ClassProperties.Add(prop.Name, prop);
                ClassPropertiesLowerCase.Add(prop.Name.ToLower(), prop);
            }

            jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new PartialEntityConverter());
        }


        /// <summary>
        /// Populates Entity and ObjectProperties for the current object
        /// based upon the provided dynamic entity
        /// </summary>
        /// <param name="dynamicEntity">object containing one or more property settings for Entity</param>
        public void Load(dynamic dynamicEntity) {
            Type type = dynamicEntity.GetType();
            foreach (var dynamicProperty in type.GetProperties()) {
                try {
                    if (ClassProperties.TryGetValue(dynamicProperty.Name, out PropertyInfo entityProperty)) {
                        entityProperty.SetValue(Entity, dynamicProperty.GetValue(dynamicEntity));
                        ObjectProperties.Add(dynamicProperty.Name);
                    }
                } catch {
                }
            }
        }

        /// <summary>
        /// Factory method that creates a new PartialEntity from the
        /// provided dynamicEntity.
        /// </summary>
        /// <param name="dynamicEntity">object containing one or more property settings for Entity</param>
        /// <returns></returns>
        public static PartialEntity<TEntity> Create(dynamic dynamicEntity) {
            Type type = dynamicEntity.GetType();
            var pe = new PartialEntity<TEntity>();
            foreach (var dynamicProperty in type.GetProperties()) {
                try {
                    if (ClassProperties.TryGetValue(dynamicProperty.Name, out PropertyInfo entityProperty)) {
                        entityProperty.SetValue(pe.Entity, dynamicProperty.GetValue(dynamicEntity));
                        pe.ObjectProperties.Add(dynamicProperty.Name);
                    }
                } catch {
                }
            }
            return pe;
        }

        /// <summary>
        /// Factory method that creates a list of PartialEntities
        /// from a list of dynamic objects
        /// </summary>
        /// <param name="dynamicEntityList">list of objects, each holding
        /// one or more properties for an Entity</param>
        /// <returns></returns>
        public static List<PartialEntity<TEntity>> CreateList(List<dynamic> dynamicEntityList) {
            var list = new List<PartialEntity<TEntity>>();
            foreach (var item in dynamicEntityList) {
                list.Add(PartialEntity<TEntity>.Create(item));
            }
            return list;
        }

        /// <summary>
        /// Converts a PartialEntity list to an Entity list
        /// </summary>
        /// <param name="partialEntityList">the list to convert</param>
        /// <returns></returns>
        public static List<TEntity> Unwrap(List<PartialEntity<TEntity>> partialEntityList) {
            var list = new List<TEntity>();
            foreach (var item in partialEntityList) {
                list.Add(item.Entity);
            }
            return list;
        }

        /// <summary>
        /// Converts a dynamic entity list to a regular entity list
        /// </summary>
        /// <param name="dynamicEntityList"></param>
        /// <returns></returns>
        public static List<TEntity> CreateAndUnwrap(List<dynamic> dynamicEntityList)
            => Unwrap(CreateList(dynamicEntityList));


        public void Deserialize(string json) {
            var partialEntity = new PartialEntity<TEntity>();
            partialEntity.Entity = new TEntity();
            byte[] data = Encoding.UTF8.GetBytes(json);
            Utf8JsonReader reader = new Utf8JsonReader(data);
            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.PropertyName) {
                    var propertyNameLowerCase = reader.GetString().ToLower();
                    if (ClassPropertiesLowerCase.TryGetValue(propertyNameLowerCase, out PropertyInfo pInfo)) {
                        ObjectProperties.Add(pInfo.Name);
                    }
                }
            }
            Entity = JsonSerializer.Deserialize<TEntity>(json);
        }

        public string Serialize() {
            var json = JsonSerializer.Serialize(this, typeof(PartialEntity<TEntity>), jsonSerializerOptions);
            return json;
        }


        public void MergeInto(TEntity entity) {
            foreach (var key in ObjectProperties) {
                var prop = ClassProperties[key];
                prop.SetValue(entity, prop.GetValue(Entity));
            }
        }

    }
}
