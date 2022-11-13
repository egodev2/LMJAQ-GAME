using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
				
public class ReferenceData
{
	[JsonProperty("Telas")]
	public List<string> TelaReferences {get; set;}
	
	[JsonProperty("Objetos")]
	public List<string> ObjetoReferences {get; set;}
	
	[JsonProperty("Recursos")]
	public List<string> RecursoReferences {get; set;}
	
	[JsonProperty("Componentes")]
	public List<string> ComponenteReferences {get; set;}
	
	[JsonProperty("Eventos")]
	public List<string> EventosReferences {get; set;}
	
	[JsonProperty("Alternativas")]
	public List<string> AlternativaReferences {get; set;}
}
