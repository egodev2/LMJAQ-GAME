using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ObjetoData {
	
	[JsonProperty("X")]
	public float X { get; set; }
	
	[JsonProperty("Y")]
	public float Y { get; set; }
	
	[JsonProperty("estaAtivo")]
	public bool estaAtivo { get; set; }
	
	[JsonProperty("Componentes")]
	public List<ComponenteData> Componentes { get; set; }
	
	[JsonProperty("Recursos")]
	public List<RecursoData> Recursos { get; set; }
}
