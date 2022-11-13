using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ModelData
{
	[JsonProperty("package")]
	public List<string> Packages {get; set;}
	
	[JsonProperty("class")]
	public ClassData Class {get; set;}
}
