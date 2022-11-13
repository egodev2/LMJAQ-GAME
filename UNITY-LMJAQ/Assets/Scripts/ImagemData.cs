using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ImagemData : RecursoData {
	
	[JsonProperty("caminho")]
	public string Caminho { get; set; }
	
	[JsonProperty("Width")]
	public int Width { get; set; }
	
	[JsonProperty("Height")]
	public int Height { get; set; }
}
