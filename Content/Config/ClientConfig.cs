using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace eslamio.Content.Config
{
	//[BackgroundColor(164, 153, 190)]
	public class ClientConfig : ModConfig
	{
		// There are 2 approaches to default values. One is applicable only to value types (int, bool, float, string, structs, etc) and the other to reference types (classes).
		// For value types, annotate the field with the DefaultValue attribute. Some structs, like Color and Vector2, accept a string that will be converted to a default value.
		// For reference types (classes), simply assign the value in the field initializer or constructor as you would typically do.

		public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool LlamasDelDesastre { get; set; }

		[DefaultValue(true)]
        public bool AguasDeLaTranquilidad { get; set; }

		[Range(0f, 2f)]
		[Increment(0.1f)]
		[DefaultValue(1f)]
		public float DVDPetSpeed { get; set; }

		[Range(0f, 2f)]
		[Increment(0.1f)]
		[DefaultValue(1f)]
		public float ModLogoSpeed { get; set; }

		[Range(0.01f, 1f)]
		[Increment(0.01f)]
		[DefaultValue(0.01f)]
		public float DopSpawnChance { get; set; }

		// Using DefaultValue, we can specify a default value.
		/*[DefaultValue(99)]
		public int SimpleDefaultInt;

		[DefaultValue(typeof(Color), "73, 94, 171, 255")] // needs 4 comma separated bytes. The Color struct has [TypeConverter(typeof(ColorConverter))] annotating it supplying a way to convert a text constant to a runtime default value.
		public Color SomeColor;

		[DefaultValue(typeof(Vector2), "0.23, 0.77")]
		public Vector2 SomeVector2;

		// OptionStrings makes a string appear as a choice rather than an input field. Remember that users can manually edit json files, so be aware that a value other than the Options in OptionStrings might populate the field.
		// TODO: Not working. Won't restore defaults
		[OptionStrings(new string[] { "Win", "Lose", "Give Up" })]
		[DefaultValue(new string[] { "Give Up", "Give Up" })]
		public string[] ArrayOfString;

		[DrawTicks]
		[OptionStrings(new string[] { "Pikachu", "Charmander", "Bulbasaur", "Squirtle" })]
		[DefaultValue("Bulbasaur")]
		public string FavoritePokemon;

		// DefaultListValue provides the default value to be added when the user clicks add in the UI.
		[DefaultListValue(123)]
		public List<int> ListOfInts = new List<int>();

		[DefaultListValue(typeof(Vector2), "0.1, 0.2")]
		public List<Vector2> ListOfVector2 = new List<Vector2>();

		// JsonDefaultListValue provides the default value for reference types/classes, expressed as JSON. If you are unsure of the JSON, you can copy from a saved config file itself.
		[JsonDefaultListValue("{\"name\": \"GoldBar\"}")]
		public List<ItemDefinition> ListOfItemDefinition = new List<ItemDefinition>();

		// For Dictionaries, additional attributes (DefaultDictionaryKeyValue or JsonDefaultDictionaryKeyValue) are used to specify a default value for the Key of the Dictionary entry. The Value uses the DefaultListValue or JsonDefaultListValue as List and HashSet do.
		[DefaultDictionaryKeyValue(0.3f)]
		[DefaultListValue(10)]
		public Dictionary<float, int> DictionaryDefaults = new Dictionary<float, int>();

		[JsonDefaultDictionaryKeyValue("{\"name\": \"GoldBar\"}")]
		[JsonDefaultListValue("{\"name\": \"SilverBar\"}")]
		public Dictionary<ItemDefinition, ItemDefinition> DictionaryDefaults2 = new Dictionary<ItemDefinition, ItemDefinition>();
		*/
	}
}
