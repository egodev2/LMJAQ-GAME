using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
		
public class ClassData
{
	[JsonProperty("type")]
	public string Type {get; set;}
	
	[JsonProperty("name")]
	public string Name {get; set;}
	
	[JsonProperty("properties")]
	public PropertyData Properties {get; set;}
	
	[JsonProperty("references")]
	public ReferenceData References {get; set;}
}
