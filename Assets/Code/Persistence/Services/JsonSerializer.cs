using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Systems.Inventory;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Persistence
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings settings;
        
        public JsonSerializer()
        {
            settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new JsonConverter[] 
                {
                    new ShapeConverter(),
                    new VolumetricMatrixConverter<Item>(),
                    new ItemConverter()
                }
            };
        }
        
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
    
    public class ShapeConverter : JsonConverter<Shape>
    {
        public override void WriteJson(JsonWriter writer, Shape value, Newtonsoft.Json.JsonSerializer serializer)
        {
            string code = value.ToString();
            
            writer.WriteStartObject();
            writer.WritePropertyName("Shape");
            writer.WriteValue(code);
            writer.WriteEndObject();
        }

        public override Shape ReadJson(JsonReader reader, Type objectType, Shape existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            string code = jsonObject["Shape"].ToString();
            
            return code.ToShape();
        }
    }

    public class ItemConverter : JsonConverter<Item>
    {
        public override void WriteJson(JsonWriter writer, Item value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var shape = value.Shape;
            var Id = value.detailsId;
            var quantity = value.quantity;
            var transformations = value.transformations;
            
            writer.WriteStartObject();
            writer.WritePropertyName("Shape");
            serializer.Serialize(writer, shape);
            writer.WritePropertyName("Quantity");
            writer.WriteValue(quantity);
            writer.WritePropertyName("Id");
            serializer.Serialize(writer, Id);
            writer.WritePropertyName("Rotation");
            writer.WriteValue(value.transformations.rotation);
            writer.WritePropertyName("Reflection");
            writer.WriteValue(value.transformations.reflection);
            writer.WriteEndObject();
        }

        public override Item ReadJson(JsonReader reader, Type objectType, Item existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            var shape = jsonObject["Shape"].ToObject<Shape>(serializer);
            var Id = jsonObject["Id"].ToObject<SerializableGuid>(serializer);
            var quantity = jsonObject["Quantity"].ToObject<int>();
            var rotation = jsonObject["Rotation"].ToObject<int>();
            var reflection = jsonObject["Reflection"].ToObject<int>();
            
            Item item = ItemDatabase.GetItemDetailsById(Id).Create(quantity);
            item.Shape = shape;
            item.transformations = new ShapeIsometryGroup(rotation, reflection);
            return item;
        }
    }

    public class VolumetricMatrixConverter<T> : JsonConverter<VolumetricMatrix<T>> where T : IVolumetricItem, ICloneable
    {
        public override void WriteJson(JsonWriter writer, VolumetricMatrix<T> value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var shape = value.shape;
            
            var uniqueItems = value.UniqueItems;
            if(uniqueItems == null) uniqueItems = new List<T>();
            
            writer.WriteStartObject();
            writer.WritePropertyName("Shape");
            serializer.Serialize(writer, shape);
            writer.WritePropertyName("UniqueItems");
            writer.WriteStartArray();
            foreach (var uniqueItem in uniqueItems)
            {
                serializer.Serialize(writer, uniqueItem);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override VolumetricMatrix<T> ReadJson(JsonReader reader, Type objectType, VolumetricMatrix<T> existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            var shape = jsonObject["Shape"].ToObject<Shape>(serializer);
            var uniqueItems = jsonObject["UniqueItems"].ToArray();
            var uniqueItemsList = new List<T>();
            foreach (var uniqueItem in uniqueItems)
            {
                var item = uniqueItem.ToObject<T>(serializer);
                uniqueItemsList.Add(item);
            }
            
            return new VolumetricMatrix<T>(shape, uniqueItemsList);
        }
    }
}