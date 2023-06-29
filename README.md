# Task 19 (Optional) - Reflection

Implement the string Serialize (object model) method in the CustomConverter class, accepting a data model and returning its serialized value in the provided serialization format.

1. A complex entity with nested fields must have a structure:

[section.begin]

Property1Name = Property1Value

Property2Name = Property2Value

[section.end]

2. A complex entity with complex nested fields that have nested fields must have a structure:

[section.begin]

Property1Name = Property1Value

Property2Name = Property2Value

     [section.begin]

     InnerProperty1Name = InnerProperty1Value

     InnerProperty2Name = InnerProperty2Value

     [section.end]

[section.end]

3. The level of possible nesting is unlimited

4. Neglect the ability to store collections

5. Serialize only properties, fields can be omited.
6. Serialize only properties that have the CustomSerializeAttribute attribute. If the Name attribute parameter is specified, take its value as the field name during serialization. If not specified, the name of the field in the text representation of the model is taken from the name of the class property.



