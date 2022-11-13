using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ComponenteData 
{
	[JsonProperty("RecursoReference")]
	public RecursoData Recurso {get; set;}
}
