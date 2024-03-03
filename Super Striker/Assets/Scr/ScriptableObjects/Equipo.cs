using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipo", menuName = "Super Striker/Equipo")]
public class Equipo : ScriptableObject
{

	#region Fields
	public string nombre;
	public Color color1;
	public Color color2;
	public Tecnico tecnico;
	public Tactica tactica;

	public CartaJugador[] plantilla = new CartaJugador[7];

	#endregion

	#region Unity methods

	#endregion

	#region Private methods

	#endregion
	
	#region Public / Protected methods

	#endregion
}