using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TelaData {
	
	[JsonProperty("Objetos")]
	public List<ObjetoData> Objetos {get; set;}
}
