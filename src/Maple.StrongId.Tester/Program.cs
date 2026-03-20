using Maple.StrongId;

Console.WriteLine($"Maple.StrongId version: {typeof(ItemTemplateId).Assembly.GetName().Version}");

var item = new ItemTemplateId(1002140);
Console.WriteLine($"ItemTemplateId(1002140).IsHat = {item.IsHat}");

var field = new FieldTemplateId(100000000);
Console.WriteLine($"FieldTemplateId(100000000).Continent = {field.Continent}");

Console.WriteLine("OK");
