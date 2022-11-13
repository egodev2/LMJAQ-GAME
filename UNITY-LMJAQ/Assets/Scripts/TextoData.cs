using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TextData : RecursoData {
	
	[JsonProperty("conteudo")]
	public string Conteudo { get; set; }
	
	[JsonProperty("tamanhoDaFonte")]
	public int TamanhoDaFonte {get; set;}
}
