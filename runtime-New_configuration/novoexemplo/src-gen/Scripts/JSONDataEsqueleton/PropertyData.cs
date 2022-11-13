using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
		
public class PropertyData
{
	[JsonProperty("HashKeys")]
	public List<string> PropertiesKeys {get; set;}
	
	[JsonProperty("HashValues")]
	public List<string> PropertiesValues {get; set;}
}

