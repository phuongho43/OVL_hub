using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PatientInfo {
	public string First_name {get; set;}
	public string Last_name {get; set;}

	public PatientInfo(string first_name, string last_name) {
		this.First_name = first_name;
		this.Last_name = last_name;
	}
}
